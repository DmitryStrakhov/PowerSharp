using System;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Services {
    public interface ICreateInstanceServiceFactory {
        ICreateInstanceService GetService(ITypeMemberDeclaration memberDeclaration);
    }
}