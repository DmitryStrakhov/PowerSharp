using System;

namespace PowerSharp.QuickFixes.Services {
    /// <summary>
    ///
    /// Service which is used for instance-generating purposes.
    /// 
    /// </summary>
    public interface ICreateInstanceService {
        bool IsAvailable();
        void Execute();
    }
}