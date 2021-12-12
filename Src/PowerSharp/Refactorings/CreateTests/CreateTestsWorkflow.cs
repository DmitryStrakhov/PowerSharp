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
            
            // get default test-project
            //
            IProject defaultTestProject = Solution.FindProject(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName));
            Assertion.Assert(defaultTestProject != null, "defaultTestProject != null");

            model = new CreateTestsDataModel {SetUpMethod = true, TearDownMethod = true};
            model.Declaration = declaration;
            model.SourceFile = sourceFile;
            model.DefaultTargetProject = defaultTestProject;
            
            // build default file name
            //
            string defaultFileName = pathsService.GetUniqueFileName(defaultTestProject, declaredElement.ShortName + "Tests.cs");
            model.TargetFilePath = pathsService.Combine(defaultTestProject, defaultFileName);
            
            // selection scope contains all projects which have NUnit dependency installed
            //
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
                    IProjectFolder projectFolder = ProjectModelUtil.MapPathToProjectFolder(Solution, model.SelectionScope, out string actualFolderPath, model.TargetFilePath, model.DefaultTargetProject);
                    Assertion.Assert(projectFolder != null, "projectFolder != null");

                    // parse path specified
                    //
                    string[] pathParts = model.TargetFilePath.SubstringAfter(actualFolderPath).TrimStart('\\', '/').Split('\\', '/');
                    string fileName = pathParts[pathParts.Length - 1];

                    // create folders if required
                    //
                    for(int n = 0; n < pathParts.Length - 1; n++)
                        projectFolder = transactionCookie.AddFolder(projectFolder, pathParts[n]);

                    // add target file
                    //
                    VirtualFileSystemPath path = projectFolder.Location.Combine(fileName);
                    model.TestClassFile = transactionCookie.AddFile(projectFolder, path);

                    // start code generation
                    //
                    if(Solution.GetComponent<IPsiTransactions>().Execute("Create Tests", () => {
                        ICSharpFile primaryPsiFile = (ICSharpFile)model.TestClassFile.GetPrimaryPsiFile().NotNull();
                        string className = pathsService.GetExpectedClassName(model.TargetFilePath);

                        // add test class declaration
                        //
                        MembersBuilder membersBuilder = new PsiFileBuilder(primaryPsiFile)
                            .AddUsingDirective(NUnitUtil.NUnitRootNamespace)
                            .AddExpectedNamespace()
                            .AddClass(className, AccessRights.PUBLIC)
                            .WithAttribute(NUnitUtil.TestFixtureAttributeClrName)
                            .WithMembers();

                        // OneTimeSetUp method
                        //
                        if(Model.OneTimeSetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeSetUp", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.OneTimeSetUpAttributeClrName);
                        }

                        // OneTimeTearDown method
                        //
                        if(Model.OneTimeTearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeTearDown", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.OneTimeTearDownAttributeClrName);
                        }

                        // SetUp method
                        //
                        if(Model.SetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("SetUp", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.SetUpAttributeClrName);
                        }

                        // TearDown method
                        //
                        if(Model.TearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("TearDown", AccessRights.PUBLIC)
                                .WithAttribute(NUnitUtil.TearDownAttributeClrName);
                        }

                        // add reference to source project if required
                        // 
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