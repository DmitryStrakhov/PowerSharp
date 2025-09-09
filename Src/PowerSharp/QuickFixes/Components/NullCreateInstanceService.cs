using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using PowerSharp.QuickFixes.Services;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Null-implementation of ICreateInstanceService service. Generates
    /// nothing.
    /// 
    /// </summary>
    public sealed class NullCreateInstanceService : ICreateInstanceService {
        private NullCreateInstanceService() {
        }
        public static readonly NullCreateInstanceService Instance = new();

        public IReadOnlyList<IExpressionStatement> Execute() {
            return null;
        }
        public bool IsAvailable() { return false; }
    }
}