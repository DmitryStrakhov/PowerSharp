using System;
using JetBrains.ProjectModel;
using JetBrains.Collections.Viewable;
using JetBrains.IDE.UI.Extensions.Validation;

namespace PowerSharp.Refactorings.CreateTests {
    public class FileCannotBeCreatedRule : SimpleValidationRuleOnProperty<string> {
        public FileCannotBeCreatedRule(IViewableProperty<string> propertyToValidate, ISolution solution, CreateTestsDataModel model)
            : base(propertyToValidate, solution.Locks) {

            string message = "File doesn't exist and cannot be created";
            string rejectReason;
            ValidateAction = s => {
                //bool projectFile = solution.GetComponent<Type2PartialManager>().CanCreateProjectFile(model, out rejectReason, s);
                //Message = message + ": " + rejectReason;
                //return projectFile;
                return true;
            };
        }
    }
}