using JetBrains.Util;
using JetBrains.Annotations;
using JetBrains.DataFlow;
using PowerSharp.Builders;
using JetBrains.ProjectModel;
using PowerSharp.Extensions;
using JetBrains.ReSharper.Psi;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.IDE.UI.Extensions.Validation;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Feature.Services.UI.Automation;
using JetBrains.ReSharper.Feature.Services.UI.Validation;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsPageStartPage : SingleBeRefactoringPage {
        [NotNull] readonly CreateTestsDataModel model;
        [NotNull] readonly PageBuilder pageBuilder;

        public CreateTestsPageStartPage([NotNull] CreateTestsWorkflow workflow)
            : base(workflow.WorkflowExecuterLifetime) {
            this.model = workflow.Model;
            this.pageBuilder = new PageBuilder(Lifetime);

            this.TargetFilePath = this.Property(nameof(model.TargetFilePath), model.TargetFilePath);
            this.SetUpMethod = this.Property(nameof(SetUpMethod), model.SetUpMethod);
            this.TearDownMethod = this.Property(nameof(TearDownMethod), model.TearDownMethod);
            this.OneTimeSetUpMethod = this.Property(nameof(OneTimeSetUpMethod), model.OneTimeSetUpMethod);
            this.OneTimeTearDownMethod = this.Property(nameof(OneTimeTearDownMethod), model.OneTimeTearDownMethod);
        }
        
        public IProperty<string> TargetFilePath { get; }
        public IProperty<bool> SetUpMethod { get; }
        public IProperty<bool> TearDownMethod { get; }
        public IProperty<bool> OneTimeSetUpMethod { get; }
        public IProperty<bool> OneTimeTearDownMethod { get; }

        public override BeControl GetPageContent() {
            return pageBuilder.TextBox(TargetFilePath, "N_ame:", x => {
                    ISolution solution = model.SourceFile.GetSolution();

                    x.WithTextNotEmpty(Lifetime, null)
                    .WithAllowedExtensions(model.SourceFile.ExtensionWithDot(), Lifetime, null)
                    .WithValidationRule(Lifetime, p => new FileAlreadyExistsRule(p, solution, model))
                    .WithValidationRule(Lifetime, p => new FileCannotBeCreatedRule(p, solution, model))
                    .WithProjectItemCompletion(solution, Lifetime, model.Declaration?.Language ?? KnownLanguage.ANY, model.SelectionScope.ToList(p => (IProjectModelElement)p), model.SuggestFilter);
                })
                .StartCollapsiblePanel()
                .CheckBox(SetUpMethod, "SetUp")
                .CheckBox(TearDownMethod, "TearDown")
                .CheckBox(OneTimeSetUpMethod, "OneTimeSetUp")
                .CheckBox(OneTimeTearDownMethod, "OneTimeTearDown")
                .EndCollapsiblePanel("Methods")
                .Content();
        }
        public override void Commit() {
            model.TargetFilePath = TargetFilePath.Value;
            model.SetUpMethod = SetUpMethod.Value;
            model.TearDownMethod = TearDownMethod.Value;
            model.OneTimeSetUpMethod = OneTimeSetUpMethod.Value;
            model.OneTimeTearDownMethod = OneTimeTearDownMethod.Value;
        }
        public override string Title { get { return "Customize your tests-class"; } }
        public override string Description { get { return "Specify options you want to see"; } }
    }
}