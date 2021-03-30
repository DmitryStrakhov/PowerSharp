using System;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace PowerSharp.QuickFixes.Services {
    public interface ICreateInstanceService {
        void Execute([NotNull] ISolution solution, [NotNull] IProgressIndicator progress);
        bool IsAvailable([NotNull] IUserDataHolder cache);
    }
}