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
using JetBrains.Application.Help;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public class CreateTestsWorkflow : DrivenRefactoringWorkflow2<CreateTestsHelper> {
        [NotNull] readonly IPathsService pathsService;
        IDeclaration declaration;

        public CreateTestsWorkflow([NotNull] ISolution solution, [NotNull] IDeclaration declaration)
            : this(solution) {
            this.declaration = declaration;
        }
        public CreateTestsWorkflow([NotNull] ISolution solution, [CanBeNull] string actionId = null)
            : base(solution, actionId) {
            pathsService = solution.GetComponent<IPathsService>();
            Model = new CreateTestsDataModel {SetUpMethod = true, TearDownMethod = true};
        }

        public CreateTestsDataModel Model { get; }

        public override bool Initialize(IDataContext context) {
            IDeclaration decl = context.TryGetClassLikeDeclaration();

            if(decl != null)
                declaration = decl;

            Assertion.Assert(declaration != null, "declaration != null");

            IProjectFile sourceFile = declaration.GetContainingFile()?.GetSourceFile()?.ToProjectFile();
            Assertion.Assert(sourceFile != null, "sourceFile != null");

            ITypeElementResolutionService service = Solution.GetComponent<ITypeElementResolutionService>();

            // get default test-project
            //
            IProject defaultTestProject = Solution.FindProject(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName));
            Assertion.Assert(defaultTestProject != null, "defaultTestProject != null");

            Model.Declaration = declaration;
            Model.SourceFile = sourceFile;
            Model.DefaultTargetProject = defaultTestProject;

            // build default file name
            //
            string defaultFileName = pathsService.GetUniqueFileName(defaultTestProject, declaration.DeclaredName + "Tests.cs");
            Model.TargetFilePath = pathsService.Combine(defaultTestProject, defaultFileName);

            // selection scope contains all projects which have NUnit dependency installed
            //
            Model.SelectionScope = Solution.GetAllRegularProjectsWhere(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName)).ToList(x => (IProjectFolder) x);
            Model.SuggestFilter = Helper[declaration.Language].CanSuggestProjectFile;
            return true;
        }
        public override bool IsAvailable(IDataContext context) {
            return context.TryGetClassLikeDeclaration() != null;
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
                    IProjectFolder projectFolder = ProjectModelUtil.MapPathToProjectFolder(Solution, Model.SelectionScope, out string actualFolderPath, Model.TargetFilePath, Model.DefaultTargetProject);
                    Assertion.Assert(projectFolder != null, "projectFolder != null");

                    // parse path specified
                    //
                    string[] pathParts = Model.TargetFilePath.SubstringAfter(actualFolderPath).TrimStart('\\', '/').Split('\\', '/');
                    string fileName = pathParts[pathParts.Length - 1];

                    // create folders if required
                    //
                    for(int n = 0; n < pathParts.Length - 1; n++)
                        projectFolder = transactionCookie.AddFolder(projectFolder, pathParts[n]);

                    // add target file
                    //
                    VirtualFileSystemPath path = projectFolder.Location.Combine(fileName);
                    Model.TestClassFile = transactionCookie.AddFile(projectFolder, path);

                    // start code generation
                    //
                    if(Solution.GetComponent<IPsiTransactions>().Execute("Create Tests", () => {
                        ICSharpFile primaryPsiFile = (ICSharpFile)Model.TestClassFile.GetPrimaryPsiFile().NotNull();
                        string className = pathsService.GetExpectedClassName(Model.TargetFilePath);

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
                        IProject targetProject = Model.TestClassFile.GetProject().NotNull();
                        IProject sourceProject = Model.SourceFile.GetProject().NotNull();
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
            if(Model.TestClassFile != null) NavigationUtil.NavigateTo(Solution, Model.TestClassFile);
            base.SuccessfulFinish(pi);
        }
        public override string Title { get { return "Create Tests"; } }
        public override HelpId HelpKeyword { get { return HelpId.None; } }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Blessed; } }
    }
}