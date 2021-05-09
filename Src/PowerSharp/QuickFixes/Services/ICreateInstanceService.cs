using System;
using JetBrains.Annotations;
using JetBrains.Util;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;

namespace PowerSharp.QuickFixes.Services {
    /// <summary>
    ///
    /// Service which is used for instance-generating purposes.
    /// 
    /// </summary>s
    public interface ICreateInstanceService {
        void Execute([NotNull] ISolution solution, [NotNull] IProgressIndicator progress);
        bool IsAvailable([NotNull] IUserDataHolder cache);
    }
}