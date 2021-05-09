using System;
using PowerSharp.QuickFixes.Services;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Null-implementation of ICreateInstanceService service. Generates
    /// nothing.
    /// 
    /// </summary>
    public sealed class NullCreateInstanceService : ICreateInstanceService {
        public void Execute() {
        }
        public bool IsAvailable() { return false; }
    }
}