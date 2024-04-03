using System;
using PowerSharp.Utils;
using JetBrains.Util;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Edit Method", Description = "Start editing a method", Group = "C#")]
    public sealed class EditMethodContextAction : EditContextActionBase {
        [NotNull] readonly ICSharpContextActionDataProvider dataProvider;

        public EditMethodContextAction([NotNull] ICSharpContextActionDataProvider dataProvider) {
            this.dataProvider = dataProvider;
        }
        public override string Text { get { return "Edit method"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            IMethodDeclaration method = dataProvider.GetMethodDeclaration();
            return IntentionUtils.IsValid(method) && method.HasCodeBody();
        }
        protected override ICSharpFunctionDeclaration GetTargetFunction() {
            return dataProvider.GetMethodDeclaration();
        }
    }
}
