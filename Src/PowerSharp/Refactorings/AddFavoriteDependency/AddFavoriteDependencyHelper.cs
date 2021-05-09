using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Refactorings.Workflow;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    public class AddFavoriteDependencyHelper : IWorkflowExec {
        public AddFavoriteDependencyHelper() {
        }
        public IDeclaration GetTypeDeclaration(IDataContext context) {
            return RefactoringWorkflowUtil.GetTypeDeclaration<ITypeDeclaration, ITypeElement>(context, out bool _);
        }
        public bool IsLanguageSupported { get { return false; } }
    }

}