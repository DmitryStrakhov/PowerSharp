using System.Collections.Generic;
using JetBrains.ProjectModel;

namespace PowerSharp.Extensions {
    public static class SolutionExtensions {
        public static IReadOnlyCollection<IProject> GetAllRegularProjects(this ISolution @this) {
            ICollection<IProject> allProjectList = @this.GetAllProjects();
            List<IProject> regularProjectList = new List<IProject>(allProjectList.Count);

            foreach(IProject project in allProjectList) {
                if(project.ProjectProperties.ProjectKind == ProjectKind.REGULAR_PROJECT) regularProjectList.Add(project);
            }
            return regularProjectList;
        }
    }
}