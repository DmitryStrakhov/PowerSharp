using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Application.DataContext;
using PowerSharp.ContextActions;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsWorkflow : DrivenRefactoringWorkflow2<CreateTestsHelper> {
        public CreateTestsWorkflow([NotNull] ISolution solution, string actionId)
            : base(solution, actionId) {
        }
        public override bool Initialize(IDataContext context) {
            return true;
        }
        public override bool IsAvailable(IDataContext context) {
            return true;
        }
        public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver) {
            return new CreateTestsRefactoring(this, Solution, driver);
        }
        protected override CreateTestsHelper CreateUnsupportedHelper() {
            throw new NotImplementedException();
        }
        protected override CreateTestsHelper CreateHelper(IRefactoringLanguageService service) {
            throw new NotImplementedException();
        }
        public override IRefactoringPage FirstPendingRefactoringPage {
            get { return new CreateTestsPageStartPage(this); }
        }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override string Title { get { return "Create Tests"; } }
        public override string HelpKeyword { get { return "Refactorings__Create_Tests"; } }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Blessed; } }
    }
}