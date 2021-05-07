using JetBrains.ProjectModel;

namespace PowerSharp.Extensions {
    public static class ProjectFileExtensions {
        public static string ExtensionWithDot(this IProjectFile @this) {
            return @this.Location.ExtensionWithDot;
        }
    }
}