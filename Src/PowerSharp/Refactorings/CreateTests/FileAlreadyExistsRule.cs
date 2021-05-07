using JetBrains.ProjectModel;
using JetBrains.Collections.Viewable;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.IDE.UI.Extensions.Validation;

namespace PowerSharp.Refactorings.CreateTests {
    public class FileAlreadyExistsRule : SimpleValidationRuleOnProperty<string> {
        public FileAlreadyExistsRule(IViewableProperty<string> propertyToValidate, ISolution solution, CreateTestsDataModel model)
            : base(propertyToValidate, solution.Locks, ValidationStates.validationWarning) {

            Message = "File already exists";
            ValidateAction = s => {
                //IProjectFile selectedFile = solution.GetComponent<Type2PartialManager>().GetSelectedFile(model, s);
                //return selectedFile == null || selectedFile.IsGeneratedFile();
                return true;
            };
        }
    }
}