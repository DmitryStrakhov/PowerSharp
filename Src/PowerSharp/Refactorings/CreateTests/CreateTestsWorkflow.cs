using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using PowerSharp.Utils;
using PowerSharp.Builders;
using PowerSharp.Extensions;
using JetBrains.Util.Extension;
using PowerSharp.Services;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public class CreateTestsWorkflow : DrivenRefactoringWorkflow2<CreateTestsHelper> {
        CreateTestsDataModel model;
        [NotNull] readonly IPathsService pathsService;

        public CreateTestsWorkflow([NotNull] ISolution solution, [CanBeNull] string actionId = null)
            : base(solution, actionId) {
            this.pathsService = solution.GetComponent<IPathsService>();
        }

        public CreateTestsDataModel Model { get { return model; } }

        public override bool Initialize(IDataContext context) {
            (IDeclaration declaration, IDeclaredElement declaredElement) = IsAvailableCore(context);
            Assertion.Assert(declaration != null, "declaration != null");
            Assertion.Assert(declaredElement != null, "declaredElement != null");

            IProjectFile sourceFile = declaration.GetContainingFile()?.GetSourceFile()?.ToProjectFile();
            Assertion.Assert(sourceFile != null, "sourceFile != null");

            ITypeElementResolutionService service = Solution.GetComponent<ITypeElementResolutionService>();
            IProject testProjectCandidate =  Solution.FindProject(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName));
            Assertion.Assert(testProjectCandidate != null, "testProjectCandidate != null");

            model = new CreateTestsDataModel {SetUpMethod = true, TearDownMethod = true};
            model.Declaration = declaration;
            model.SourceFile = sourceFile;
            model.DefaultTargetProject = testProjectCandidate;
            
            string fileName = pathsService.GetUniqueFileName(testProjectCandidate, declaredElement.ShortName + "Tests.cs");
            model.TargetFilePath = pathsService.Combine(testProjectCandidate, fileName);
            model.SelectionScope = Solution.GetAllRegularProjectsWhere(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName)).ToList(x => (IProjectFolder)x);
            model.SuggestFilter = Helper[declaredElement.PresentationLanguage].CanSuggestProjectFile;
            return true;
        }
        public override bool IsAvailable(IDataContext context) {
            (IDeclaration declaration, IDeclaredElement _) = IsAvailableCore(context);
            return declaration != null;
        }
        private (IDeclaration, IDeclaredElement) IsAvailableCore([NotNull] IDataContext context) {
            IDeclaredElement declaredElement = context.GetData(RefactoringDataConstants.DeclaredElementWithoutSelection);
            if(declaredElement == null) return (null, null);
            return (Helper[declaredElement.PresentationLanguage].GetTypeDeclaration(context), declaredElement);
        }
        public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver) {
            return new CreateTestsRefactoring(this, Solution, driver);
        }
        protected override CreateTestsHelper CreateUnsupportedHelper() {
            return new CreateTestsHelper();
        }
        protected override CreateTestsHelper CreateHelper(IRefactoringLanguageService service) {
            return new CreateTestsHelper();
        }
        public override IRefactoringPage FirstPendingRefactoringPage {
            get { return new CreateTestsPageStartPage(this); }
        }
        public override bool PreExecute(IProgressIndicator pi) {
            using(IProjectModelTransactionCookie transactionCookie = Solution.CreateTransactionCookie(DefaultAction.Commit, "Create Tests", NullProgressIndicator.Create())) {
                try {
                    IProjectFolder projectFolder = ProjectModelUtil.MapPathToFolder(Solution, model.SelectionScope, out string actualFolderPath, model.TargetFilePath, model.DefaultTargetProject);

                    string[] pathParts = model.TargetFilePath.SubstringAfter(actualFolderPath).TrimStart('\\', '/').Split('\\', '/');
                    string fileName = pathParts[pathParts.Length - 1];

                    for(int n = 0; n < pathParts.Length - 1; n++)
                        projectFolder = transactionCookie.AddFolder(projectFolder, pathParts[n]);

                    FileSystemPath path = projectFolder.Location.Combine(fileName);
                    if(!transactionCookie.CanAddFile(projectFolder, path, out string _)) return false;

                    model.TestClassFile = transactionCookie.AddFile(projectFolder, path);

                    if(Solution.GetComponent<IPsiTransactions>().Execute("Create Tests", () => {
                        ICSharpFile primaryPsiFile = (ICSharpFile)model.TestClassFile.GetPrimaryPsiFile().NotNull();
                        string className = pathsService.GetExpectedClassName(model.TargetFilePath);

                        MembersBuilder membersBuilder = new PsiFileBuilder(primaryPsiFile)
                            .AddUsingDirective(NUnitUtil.NUnitRootNamespace)
                            .AddExpectedNamespace()
                            .AddClass(className, AccessRights.PUBLIC)
                            .WithAttribute(NUnitUtil.TestFixtureAttributeClrName)
                            .WithMembers();

                        if(Model.OneTimeSetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeSetUp", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.OneTimeSetUpAttributeClrName);
                        }

                        if(Model.OneTimeTearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeTearDown", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.OneTimeTearDownAttributeClrName);
                        }

                        if(Model.SetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("SetUp", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.SetUpAttributeClrName);
                        }

                        if(Model.TearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("TearDown", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.TearDownAttributeClrName);
                        }

                        IProject targetProject = model.TestClassFile.GetProject().NotNull();
                        IProject sourceProject = model.SourceFile.GetProject().NotNull();
                        transactionCookie.AddProjectReference(targetProject, sourceProject);

                    }).Succeded)
                        transactionCookie.Commit(NullProgressIndicator.Create());
                    else {
                        transactionCookie.Rollback();
                    }
                }
                catch {
                    transactionCookie.Rollback();
                    throw;
                }
            }
            return true;
        }
        public override void SuccessfulFinish(IProgressIndicator pi) {
            if(model.TestClassFile != null) NavigationUtil.NavigateTo(Solution, model.TestClassFile);
            base.SuccessfulFinish(pi);
        }
        public override string Title { get { return "Create Tests"; } }
        public override string HelpKeyword { get { return "Refactorings__Create_Tests"; } }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Blessed; } }
    }
}