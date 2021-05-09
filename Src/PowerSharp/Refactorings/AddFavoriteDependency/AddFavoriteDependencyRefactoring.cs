using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    public sealed class AddFavoriteDependencyRefactoring : DrivenRefactoring<AddFavoriteDependencyWorkflow, RefactoringExecBase<AddFavoriteDependencyWorkflow, AddFavoriteDependencyRefactoring>> {
        public AddFavoriteDependencyRefactoring(
            [NotNull] AddFavoriteDependencyWorkflow workflow,
            [NotNull] ISolution solution,
            [NotNull] IRefactoringDriver driver)
            : base(workflow, solution, driver) {
        }
        public override bool Execute(IProgressIndicator pi) {
            return true;
        }
    }
}