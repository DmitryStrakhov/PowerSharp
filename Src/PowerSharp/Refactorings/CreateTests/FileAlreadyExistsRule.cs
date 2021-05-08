using PowerSharp.Utils;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using PowerSharp.Extensions;
using JetBrains.Collections.Viewable;
using JetBrains.IDE.UI.Extensions.Validation;

namespace PowerSharp.Refactorings.CreateTests {
    public class FileAlreadyExistsRule : SimpleValidationRuleOnProperty<string> {
        [NotNull] readonly CreateTestsDataModel model;

        public FileAlreadyExistsRule(IViewableProperty<string> propertyToValidate, ISolution solution, CreateTestsDataModel model)
            : base(propertyToValidate, solution.Locks) {
            this.model = model;

            Message = "File already exists";
            ValidateAction = x => !this.model.GetSolution().ContainsFile(this.model.SelectionScope, x, this.model.DefaultTargetProject);
        }
    }
}