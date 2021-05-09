using JetBrains.Diagnostics;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    [RefactoringWorkflowProvider]
    public class AddFavoriteDependencyWorkflowProvider : IRefactoringWorkflowProvider {
        public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext) {
            ISolution solution = dataContext.GetData(ProjectModelDataConstants.SOLUTION).NotNull();

            return new IRefactoringWorkflow[] {
                new AddFavoriteDependencyWorkflow(solution, "AddFavoriteDependency")
            };
        }
    }
}