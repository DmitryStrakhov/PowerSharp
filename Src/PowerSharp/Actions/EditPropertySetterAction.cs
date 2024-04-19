using System;
using PowerSharp.Properties;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditPropertySetterActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 4613518)]
    public sealed class EditPropertySetterAction : EditPropertyActionBase {
        public EditPropertySetterAction()
            : base(CSharpAccessorKind.SETTER) {
        }
    }
}
