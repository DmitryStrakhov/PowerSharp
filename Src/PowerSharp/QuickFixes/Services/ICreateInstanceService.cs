using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Services {
    /// <summary>
    ///
    /// Service which is used for instance-generating purposes.
    /// 
    /// </summary>
    public interface ICreateInstanceService {
        bool IsAvailable();
        IReadOnlyList<IExpressionStatement> Execute();
    }
}