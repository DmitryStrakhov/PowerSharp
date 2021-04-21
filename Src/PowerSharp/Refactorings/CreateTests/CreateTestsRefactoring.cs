using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    public sealed class CreateTestsRefactoring : DrivenRefactoring<CreateTestsWorkflow, RefactoringExecBase<CreateTestsWorkflow, CreateTestsRefactoring>> {
        public CreateTestsRefactoring(
            [NotNull] CreateTestsWorkflow workflow,
            [NotNull] ISolution solution,
            [NotNull] IRefactoringDriver driver)
            : base(workflow, solution, driver) {
        }
        public override bool Execute(IProgressIndicator pi) {
            return true;
        }
    }
}