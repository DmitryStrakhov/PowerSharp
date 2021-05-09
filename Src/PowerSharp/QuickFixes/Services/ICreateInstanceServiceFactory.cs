using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Services {
    /// <summary>
    ///
    /// Service which is used to instantiate certain instance-generating service.
    /// 
    /// </summary>
    public interface ICreateInstanceServiceFactory {
        [NotNull]
        ICreateInstanceService GetService([CanBeNull] ITypeMemberDeclaration memberDeclaration);
    }
}