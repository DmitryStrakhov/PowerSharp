using System;
using System.Linq;
using JetBrains.Annotations;
using PowerSharp.Utils;
using JetBrains.ProjectModel;
using System.Collections.Generic;

namespace PowerSharp.Extensions {
    /// <summary>
    ///
    /// Extension-methods for ISolution class.
    /// 
    /// </summary>
    public static class SolutionExtensions {
        public static IReadOnlyCollection<IProject> GetAllRegularProjects(this ISolution @this) {
            ICollection<IProject> allProjectList = @this.GetAllProjects();
            List<IProject> regularProjectList = new List<IProject>(allProjectList.Count);

            foreach(IProject project in allProjectList) {
                if(project.ProjectProperties.ProjectKind == ProjectKind.REGULAR_PROJECT) regularProjectList.Add(project);
            }
            return regularProjectList;
        }
        public static IReadOnlyCollection<IProject> GetAllRegularProjectsWhere(this ISolution @this, [NotNull] Func<IProject, bool> predicate) {
            Guard.IsNotNull(predicate, nameof(predicate));

            List<IProject> projectList = new List<IProject>(64);

            foreach(IProject project in @this.GetAllRegularProjects()) {
                if(predicate(project)) projectList.Add(project);
            }
            return projectList;
        }
        [CanBeNull]
        public static IProject FindProject(this ISolution @this, [NotNull] Func<IProject, bool> predicate) {
            Guard.IsNotNull(predicate, nameof(predicate));

            return @this.GetAllRegularProjects().FirstOrDefault(predicate);
        }
        public static bool ContainsProject(this ISolution @this, [NotNull] Func<IProject, bool> predicate) {
            Guard.IsNotNull(predicate, nameof(predicate));

            return @this.FindProject(predicate) != null;
        }
        public static bool ContainsFile(this ISolution @this,
            [ItemNotNull] IEnumerable<IProjectFolder> selectionScope,
            [NotNull] string filePath,
            [CanBeNull] IProjectFolder defaultFolder) {
            Guard.IsNotEmpty(filePath, nameof(filePath));

            return ProjectModelUtil.ContainsFile(@this, selectionScope, filePath, defaultFolder);
        }
    }
}