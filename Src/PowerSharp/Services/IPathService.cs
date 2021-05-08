using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;

namespace PowerSharp.Services {
    public interface IPathsService {
        string GetUniqueFileName([NotNull] IProjectFolder folder, string proposedName);
        [NotNull] string GetExpectedClassName(string filePath);
        [NotNull] string Combine([NotNull] IProject project, string fileName);
    }
}