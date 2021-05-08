using System;
using System.IO;
using System.Linq;
using PowerSharp.Services;
using JetBrains.ProjectModel;

namespace PowerSharp.Components {
    [SolutionComponent]
    public class DefaultPathsService : IPathsService {
        public DefaultPathsService() {
        }

        #region GetUniqueFileName

        string IPathsService.GetUniqueFileName(IProjectFolder folder, string proposedFileName) {
            Guard.IsNotNull(folder, nameof(folder));
            Guard.IsNotEmpty(proposedFileName, nameof(proposedFileName));
            
            if(!FileExists(folder, proposedFileName)) return proposedFileName;

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(proposedFileName);
            string extension = Path.GetExtension(proposedFileName);

            for(int n = 1;; n++) {
                string fileName = fileNameWithoutExt + n.ToString() + extension;
                if(!FileExists(folder, fileName)) return fileName;
            }
        }
        string IPathsService.GetExpectedClassName(string filePath) {
            Guard.IsNotEmpty(filePath, nameof(filePath));
            return Path.GetFileNameWithoutExtension(filePath);
        }
        string IPathsService.Combine(IProject project, string fileName) {
            return project.Name + "\\" + fileName;
        }

        #endregion

        private bool FileExists(IProjectFolder projectFolder, string fileName) {
            return projectFolder.GetSubItems().OfType<IProjectFile>().Any(x => x.Name == fileName);
        }
    }
}