using System;
using JetBrains.ProjectModel;
using JetBrains.Util;
using JetBrains.TextControl;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Feature.Services.ContextActions;

namespace PowerSharp.ContextActions.TestFixture {
    [ContextAction(Name = "CreateTestFixture", Description = "Creates a test fixture for a class", Group = "C#", Disabled = false, Priority = 1)]
    public class CreateTestFixtureContextAction : ContextActionBase {
        public CreateTestFixtureContextAction(LanguageIndependentContextActionDataProvider dataProvider) {
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return x => MessageBox.ShowInfo(nameof(CreateTestFixtureContextAction));
        }
        public override bool IsAvailable(IUserDataHolder cache) {
            return true;
        }
        public override string Text { get { return "Create TestFixture"; } }
    }
}