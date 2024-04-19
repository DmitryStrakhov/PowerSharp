using System;
using PowerSharp.Properties;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.Shortcuts.ShortcutManager;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditPropertyGetterActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 2612587)]
    public sealed class EditPropertyGetterAction : EditPropertyActionBase {
        public EditPropertyGetterAction()
            : base(CSharpAccessorKind.GETTER) {
        }
    }
}
