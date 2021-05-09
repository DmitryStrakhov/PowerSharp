using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Default implementation of ICreateInstanceServiceFactory. Is used to
    /// instantiate certain code-generation service.
    /// 
    /// </summary>
    [SolutionComponent]
    public sealed class DefaultCreateInstanceServiceFactory : ICreateInstanceServiceFactory {
        public ICreateInstanceService GetService(ITypeMemberDeclaration memberDeclaration) {
            if(memberDeclaration is IFieldDeclaration || memberDeclaration is IPropertyDeclaration) {
                return ((IModifiersOwner)memberDeclaration).IsStatic
                    ? (ICreateInstanceService)new CreateInstanceOfStaticMemberService(memberDeclaration)
                    : new CreateInstanceOfInstanceMemberService(memberDeclaration);
            }
            return new NullCreateInstanceService();
        }
    }
}