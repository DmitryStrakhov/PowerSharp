using System;
using System.Collections.Generic;
using PowerSharp.ContextActions;
using JetBrains.Application.DataContext;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.CreateTests {
    [RefactoringWorkflowProvider]
    public class CreateTestsWorkflowProvider : IRefactoringWorkflowProvider {
        public IEnumerable<IRefactoringWorkflow> CreateWorkflow(IDataContext dataContext) {
            ISolution solution = dataContext.GetData(ProjectModelDataConstants.SOLUTION).NotNull();

            return new IRefactoringWorkflow[] {
                new CreateTestsWorkflow(solution, "CreateTests")
            };
        }
    }
}