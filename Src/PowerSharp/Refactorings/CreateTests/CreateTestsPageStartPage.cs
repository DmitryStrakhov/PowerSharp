using System;
using JetBrains.DataFlow;
using PowerSharp.Builders;
using PowerSharp.Extensions;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsPageStartPage : SingleBeRefactoringPage {
        readonly CreateTestsDataModel model;
        readonly PageBuilder pageBuilder;

        public IProperty<string> TestClassName { get; }

        public IProperty<bool> SetUpMethod { get; }
        public IProperty<bool> TearDownMethod { get; }
        public IProperty<bool> OneTimeSetUpMethod { get; }
        public IProperty<bool> OneTimeTearDownMethod { get; }

        public CreateTestsPageStartPage(CreateTestsWorkflow workflow)
            : base(workflow.WorkflowExecuterLifetime) {
            this.model = workflow.Model;
            this.pageBuilder = new PageBuilder(Lifetime);

            this.TestClassName = this.Property(nameof(model.TestClassName), model.TestClassName);
            this.SetUpMethod = this.Property(nameof(SetUpMethod), model.SetUpMethod);
            this.TearDownMethod = this.Property(nameof(TearDownMethod), model.TearDownMethod);
            this.OneTimeSetUpMethod = this.Property(nameof(OneTimeSetUpMethod), model.OneTimeSetUpMethod);
            this.OneTimeTearDownMethod = this.Property(nameof(OneTimeTearDownMethod), model.OneTimeTearDownMethod);
        }
        public override BeControl GetPageContent() {
            return pageBuilder.TextBox(TestClassName, "N_ame:")
                .StartGroupBox()
                .CheckBox(SetUpMethod, "SetUp")
                .CheckBox(TearDownMethod, "TearDown")
                .CheckBox(OneTimeSetUpMethod, "OneTimeSetUp")
                .CheckBox(OneTimeTearDownMethod, "OneTimeTearDown")
                .EndGroupBox("Methods")
                .Content();
        }
        public override void Commit() {
            model.TestClassName = TestClassName.Value;
            model.SetUpMethod = SetUpMethod.Value;
            model.TearDownMethod = TearDownMethod.Value;
            model.OneTimeSetUpMethod = OneTimeSetUpMethod.Value;
            model.OneTimeTearDownMethod = OneTimeTearDownMethod.Value;
        }
        public override string Title { get { return "Customize your tests-class"; } }
        public override string Description { get { return "Specify options you want to see"; } }
    }
}