using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Util.Extension;
using JetBrains.ReSharper.Feature.Services.UI;

namespace PowerSharp.Utils {
    /// <summary>
    ///
    /// Utils methods which are mostly used to map
    /// paths like ProjectName\Folder1\Folder2\FileName into elements of project model.
    /// 
    /// </summary>
    public static class ProjectModelUtil {
        [CanBeNull]
        public static IProjectFolder MapPathToProjectFolder(
            [NotNull] ISolution solution,
            [NotNull] IEnumerable<IProjectFolder> selectionScope,
            [NotNull] out string folderPath,
            [NotNull] string filePath,
            [CanBeNull] IProjectFolder defaultFolder = null) {

            Guard.IsNotNull(solution, nameof(solution));
            Guard.IsNotNull(selectionScope, nameof(selectionScope));
            Guard.IsNotEmpty(filePath, nameof(filePath));

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
            if(projectFolder == null) {
                folderPath = string.Empty;
                projectFolder = defaultFolder;
            }
            return projectFolder;
        }
        [CanBeNull]
        public static IProjectFile GetSelectedFile(
            [NotNull] ISolution solution,
            [NotNull] IEnumerable<IProjectFolder> selectionScope,
            string filePath,
            IProjectFolder defaultFolder = null) {

            Guard.IsNotNull(solution, nameof(solution));
            Guard.IsNotNull(selectionScope, nameof(selectionScope));
            Guard.IsNotEmpty(filePath, nameof(filePath));

            IProjectFolder projectFolder = MapPathToProjectFolder(solution, selectionScope, out string folderPath, filePath, defaultFolder);
            if(projectFolder == null) return null;
            string unknownPart = filePath.SubstringAfter(folderPath).TrimStart('\\', '/');
            return projectFolder.GetSubItems().OfType<IProjectFile>().FirstOrDefault(x => x.Name == unknownPart);
        }
        public static bool ContainsFile(
            [NotNull] ISolution solution,
            [NotNull] IEnumerable<IProjectFolder> selectionScope,
            string filePath,
            IProjectFolder defaultFolder = null) {

            Guard.IsNotNull(solution, nameof(solution));
            Guard.IsNotNull(selectionScope, nameof(selectionScope));
            Guard.IsNotEmpty(filePath, nameof(filePath));
            return GetSelectedFile(solution, selectionScope, filePath, defaultFolder) != null;
        }
    }
}