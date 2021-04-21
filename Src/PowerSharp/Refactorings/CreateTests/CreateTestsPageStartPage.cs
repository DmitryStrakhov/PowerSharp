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

        public IProperty<bool> AddSetUpMethod { get; }
        public IProperty<bool> AddTearDownMethod { get; }

        public IProperty<bool> AddNUnitPackage { get; }
        public IProperty<bool> AddFluentAssertionsPackage { get; }

        public CreateTestsPageStartPage(CreateTestsWorkflow workflow)
            : base(workflow.WorkflowExecuterLifetime) {
            this.model = workflow.Model;
            this.pageBuilder = new PageBuilder(Lifetime);

            this.TestClassName = this.Property("CreateTests.ClassName", model.TestClassName);
            this.AddSetUpMethod = this.Property("CreateTests.SetUp", model.AddSetUpMethod);
            this.AddTearDownMethod = this.Property("CreateTests.TearDown", model.AddTearDownMethod);
            this.AddNUnitPackage = this.Property("CreateTests.NUnit", model.AddNUnitPackage);
            this.AddFluentAssertionsPackage = this.Property("CreateTests.FluentAssertions", model.AddFluentAssertionsPackage);
        }
        public override BeControl GetPageContent() {
            return pageBuilder.TextBox(TestClassName, "N_ame:")
                .StartGroupBox()
                .CheckBox(AddSetUpMethod, "SetUp")
                .CheckBox(AddTearDownMethod, "TearDown")
                .EndGroupBox("Methods")
                .StartGroupBox()
                .CheckBox(AddNUnitPackage, "NUnit")
                .CheckBox(AddFluentAssertionsPackage, "Fluent.Assertions")
                .EndGroupBox("NuGet Packages")
                .Content();
        }
        public override void Commit() {
            model.TestClassName = TestClassName.Value;
            model.AddSetUpMethod = AddSetUpMethod.Value;
            model.AddTearDownMethod = AddTearDownMethod.Value;
            model.AddNUnitPackage = AddNUnitPackage.Value;
            model.AddFluentAssertionsPackage = AddFluentAssertionsPackage.Value;
        }
        public override string Title { get { return "Customize your tests-class"; } }
        public override string Description { get { return "Specify options you want to see"; } }
    }
}