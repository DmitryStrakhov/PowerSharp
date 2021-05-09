using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.Util;
using PowerSharp.QuickFixes.Services;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Null-implementation of ICreateInstanceService service. Generates
    /// nothing.
    /// 
    /// </summary>
    public sealed class NullCreateInstanceService : ICreateInstanceService {
        public void Execute(ISolution solution, IProgressIndicator progress) {
        }
        public bool IsAvailable(IUserDataHolder cache) { return false; }
    }
}