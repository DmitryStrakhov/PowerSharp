using JetBrains.ProjectModel;

namespace PowerSharp.Extensions {
    /// <summary>
    /// 
    /// Extension-methods for IProjectFile interface.
    /// 
    /// </summary>
    public static class ProjectFileExtensions {
        public static string ExtensionWithDot(this IProjectFile @this) {
            return @this.Location.ExtensionWithDot;
        }
    }
}