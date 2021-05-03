using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;
using PowerSharp.Utils;
using PowerSharp.Builders;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.Application.Progress;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public class CreateTestsWorkflow : DrivenRefactoringWorkflow2<CreateTestsHelper> {
        CreateTestsDataModel model;

        public CreateTestsWorkflow([NotNull] ISolution solution, [CanBeNull] string actionId = null)
            : base(solution, actionId) {
        }

        public CreateTestsDataModel Model { get { return model; } }

        public override bool Initialize(IDataContext context) {
            IDeclaration declaration = IsAvailableCore(context);
            if(declaration == null) return false;

            model = CreateTestsDataModel(declaration);
            return true;
        }
        static CreateTestsDataModel CreateTestsDataModel(IDeclaration declaration) {
            CreateTestsDataModel model = new CreateTestsDataModel();

            model.Declaration = declaration;
            model.SetUpMethod = true;
            model.TearDownMethod = true;
            model.TestClassName = declaration.DeclaredElement.NotNull().ShortName + "Tests";
            return model;
        }
        public override bool IsAvailable(IDataContext context) {
            return IsAvailableCore(context) != null;
        }
        [CanBeNull]
        private IDeclaration IsAvailableCore([NotNull] IDataContext context) {
            IDeclaredElement declaredElement = context.GetData(RefactoringDataConstants.DeclaredElementWithoutSelection);
            if(declaredElement == null) return null;
            return Helper[declaredElement.PresentationLanguage].GetTypeDeclaration(context);
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
            IProjectFolder projectFolder = model.Declaration.GetSourceFile().ToProjectFile()?.ParentFolder;
            if(projectFolder == null) return false;
            
            using(IProjectModelTransactionCookie transactionCookie = Solution.CreateTransactionCookie(DefaultAction.Commit, "Create Tests", NullProgressIndicator.Create())) {
                try {
                    FileSystemPath path = projectFolder.Location.Combine(Model.TestClassName + ".cs");
                    if(transactionCookie.CanAddFile(projectFolder, path, out string _)) {
                        model.TestClassFile = transactionCookie.AddFile(projectFolder, path);
                    }
                    
                    if(Solution.GetComponent<IPsiTransactions>().Execute("Create Tests", () => {
                        ICSharpFile primaryPsiFile = (ICSharpFile)model.TestClassFile.GetPrimaryPsiFile().NotNull();

                        MembersBuilder membersBuilder = new PsiFileBuilder(primaryPsiFile)
                            .AddUsingDirective("NUnit.Framework")
                            .AddExpectedNamespace()
                            .AddClass(model.TestClassName, AccessRights.PUBLIC)
                            .WithAttribute("NUnit.Framework.TestFixtureAttribute")
                            .WithMembers();

                        if(Model.OneTimeSetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeSetUp", AccessRights.PUBLIC)
                                .WithAttribute("NUnit.Framework.OneTimeSetUpAttribute");
                        }

                        if(Model.OneTimeTearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("OneTimeTearDown", AccessRights.PUBLIC)
                                .WithAttribute("NUnit.Framework.OneTimeTearDownAttribute");
                        }

                        if(Model.SetUpMethod) {
                            membersBuilder
                                .AddVoidMethod("SetUp", AccessRights.PUBLIC)
                                .WithAttribute("NUnit.Framework.SetUpAttribute");
                        }

                        if(Model.TearDownMethod) {
                            membersBuilder
                                .AddVoidMethod("TearDown", AccessRights.PUBLIC)
                                .WithAttribute("NUnit.Framework.TearDownAttribute");
                        }

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