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
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.Application.DataContext;
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

            model = new CreateTestsDataModel();
            model.Declaration = declaration;
            model.TestClassName = declaration.DeclaredElement.NotNull().ShortName + "Tests";
            model.AddSetUpMethod = true;
            model.AddTearDownMethod = true;
            model.AddNUnitPackage = true;
            model.AddFluentAssertionsPackage = true;
            return true;
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
            throw new NotImplementedException();
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
                    model.TestClassFile = transactionCookie.AddFile(projectFolder, path);
                    
                    if (Solution.GetComponent<IPsiTransactions>().Execute("Create Tests", () => {
                        ICSharpFile primaryPsiFile = (ICSharpFile)model.TestClassFile.GetPrimaryPsiFile().NotNull();

                        MembersBuilder membersBuilder = new PsiFileBuilder(primaryPsiFile)
                            .AddUsingDirective("NUnit.Framework")
                            .AddUsingDirective("FluentAssertions")
                            .AddExpectedNamespace()
                            .AddClass(model.TestClassName, AccessRights.PUBLIC)
                            .WithAttribute("NUnit.Framework.TestFixtureAttribute")
                            .WithMembers();

                        membersBuilder
                            .AddVoidMethod("SetUp", AccessRights.PUBLIC)
                            .WithAttribute("NUnit.Framework.SetUpAttribute");
                        membersBuilder
                            .AddVoidMethod("TearDown", AccessRights.PUBLIC)
                            .WithAttribute("NUnit.Framework.TearDownAttribute");

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
            if(model.TestClassFile != null) {
                NavigationUtil.NavigateTo(Solution, model.TestClassFile);
            }
            base.SuccessfulFinish(pi);
        }
        public override string Title { get { return "Create Tests"; } }
        public override string HelpKeyword { get { return "Refactorings__Create_Tests"; } }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Blessed; } }
    }
}