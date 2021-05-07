using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.UI;

namespace PowerSharp.Utils {
    public static class ProjectModelUtil {
        [CanBeNull]
        public static IProjectFolder MapPathToFolder(
            [NotNull] ISolution solution,
            [NotNull] IEnumerable<IProjectFolder> selectionScope,
            [NotNull] out string folderPath,
            string filePath) {

            IProjectFolder projectFolder;
            folderPath = filePath.TrimEnd('\\', '/');

            while(folderPath.Contains('\\') || folderPath.Contains('/')) {
                projectFolder = ChooseProjectFolderController.ParseFolderName(solution, folderPath);

                if(projectFolder != null) {
                    if(projectFolder.GetParentFolders().Intersect(selectionScope).Any())
                        return projectFolder;
                    folderPath = string.Empty;
                    return null;
                }
                int length = folderPath.LastIndexOfAny(new[] {'\\', '/'});
                if(length == 0) return null;

                if(length >= 0) {
                    folderPath = folderPath.Substring(0, length);
                }
            }
            projectFolder = ChooseProjectFolderController.ParseFolderName(solution, folderPath);
            if(projectFolder == null) folderPath = string.Empty;
            return projectFolder;
        }
    }
}