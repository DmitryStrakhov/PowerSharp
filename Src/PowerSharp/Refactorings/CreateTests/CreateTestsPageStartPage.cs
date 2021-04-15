using JetBrains.IDE.UI.Extensions;
using JetBrains.Application.Progress;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsPageStartPage : SingleBeRefactoringPage {
        public CreateTestsPageStartPage(CreateTestsWorkflow workflow)
            : base(workflow.WorkflowExecuterLifetime) {
        }
        public override BeControl GetPageContent() {
            return BeControls.GetGrid(GridOrientation.Vertical);
        }
        public override bool RefreshContents(IProgressIndicator pi) {
            return base.RefreshContents(pi);
        }
        public override bool Initialize(IProgressIndicator progressIndicator) {
            return base.Initialize(progressIndicator);
        }
        public override IRefactoringPage Commit(IProgressIndicator pi) {
            return base.Commit(pi);
        }
        public override bool DoNotShow { get { return base.DoNotShow; } }

        public override string Title { get { return "Title"; } }
        public override string Description { get { return "Description"; } }
        public override string PageDescription { get { return "PageDescription";} }
    }
}