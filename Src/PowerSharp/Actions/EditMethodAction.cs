using System;
using PowerSharp.Utils;
using PowerSharp.Properties;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditMethodActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 1552586)]
    public sealed class EditMethodAction : EditActionBase {
        protected override bool IsAvailable(IDataContext context) {
            IMethodDeclaration method = context.TryGetMethodDeclaration();
            return IntentionUtils.IsValid(method) && method.HasCodeBody();
        }
        protected override ICSharpFunctionDeclaration GetTargetFunction(IDataContext context) {
            return context.TryGetMethodDeclaration();
        }
    }
}
