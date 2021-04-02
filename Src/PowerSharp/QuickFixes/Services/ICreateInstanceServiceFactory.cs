using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Services {
    public interface ICreateInstanceServiceFactory {
        [NotNull]
        ICreateInstanceService GetService([CanBeNull] ITypeMemberDeclaration memberDeclaration);
    }
}