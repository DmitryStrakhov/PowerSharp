using System;
using System.IO;
using PowerSharp.Utils;
using PowerSharp.Services;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Collections.Viewable;
using JetBrains.IDE.UI.Extensions.Validation;

namespace PowerSharp.Refactorings.CreateTests {
    public class FileCannotBeCreatedRule : SimpleValidationRuleOnProperty<string> {
        readonly CreateTestsDataModel model;

        public FileCannotBeCreatedRule(IViewableProperty<string> propertyToValidate, ISolution solution, CreateTestsDataModel model)
            : base(propertyToValidate, solution.Locks) {
            this.model = model;

            ValidateAction = s => {
                if(CanCreateProjectFile(out string rejectReason, s)) return true;

                Message = "File cannot be created: " + rejectReason;
                return false;
            };
        }
        private bool CanCreateProjectFile([NotNull] out string rejectReason, string filePath) {
            if(string.IsNullOrWhiteSpace(filePath)) {
                rejectReason = "path is empty";
                return false;
            }
            if(filePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0) {
                rejectReason = "path contains invalid characters";
                return false;
            }
            
            IProject project = ProjectModelUtil.MapPathToFolder(model.GetSolution(), model.SelectionScope, out string _, filePath, model.DefaultTargetProject)?.GetProject();
            if(project != null) {
                ITypeElementResolutionService service = model.GetSolution().GetComponent<ITypeElementResolutionService>();
                if(!service.ContainsClrType(project, NUnitUtil.MarkerClrName)) {
                    rejectReason = "project doesn't have NUnit framework as a dependency";
                    return false;
                }
            }
            rejectReason = string.Empty;
            return true;
        }
    }
}