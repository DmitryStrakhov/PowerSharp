using System;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Feature.Services.ContextActions;

namespace PowerSharp.ContextActions.FieldInstance {
    [ContextAction(Name = "CreateFieldInstance", Description = "Creates instance of a field", Group = "C#", Disabled = false, Priority = 1)]
    public class CreateFieldInstanceContextAction : ContextActionBase {
        public CreateFieldInstanceContextAction(LanguageIndependentContextActionDataProvider dataProvider) {
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return x => MessageBox.ShowInfo(nameof(CreateFieldInstanceContextAction));
        }
        public override bool IsAvailable(IUserDataHolder cache) {
            return true;
        }
        public override string Text { get { return "Create Instance"; } }
    }
}