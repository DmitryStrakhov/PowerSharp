using System;
using System.Linq;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using PowerSharp.Services;
using JetBrains.ReSharper.Psi.Modules;

namespace PowerSharp.Components {
    /// <summary>
    /// 
    /// Default implementation of ITypeElementResolutionService service. Resolves
    /// types for specified projects. It is very need for code-generation purposes.
    /// 
    /// </summary>
    [SolutionComponent(Instantiation.DemandAnyThreadSafe)]
    public class DefaultTypeElementResolutionService : ITypeElementResolutionService {

        #region ITypeElementResolutionService

        ITypeElement ITypeElementResolutionService.Resolve(IProject project, string clrTypeName) {
            Guard.IsNotNull(project, nameof(project));
            Guard.IsNotEmpty(clrTypeName, nameof(clrTypeName));

            IPsiModule psiModule = project.GetPsiModules().FirstOrDefault();
            if(psiModule == null) return null;

            return project.GetSolution().GetPsiServices()
                .Symbols.GetSymbolScope(psiModule, true, true)
                .GetTypeElementByCLRName(clrTypeName);
        }
        bool ITypeElementResolutionService.ContainsClrType(IProject project, string clrTypeName) {
            Guard.IsNotNull(project, nameof(project));
            Guard.IsNotEmpty(clrTypeName, nameof(clrTypeName));

            ITypeElementResolutionService @this = this;
            return @this.Resolve(project, clrTypeName) != null;
        }

        #endregion
    }
}