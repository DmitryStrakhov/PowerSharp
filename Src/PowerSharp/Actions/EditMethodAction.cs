﻿using System;
using PowerSharp.Utils;
using PowerSharp.Properties;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditMethodActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 1552586)]
    public sealed class EditMethodAction : EditFunctionActionBase {
        protected override ICSharpFunctionDeclaration GetTargetFunction(IDataContext context) {
            ICSharpFunctionDeclaration function = context.TryGetFunctionDeclaration();
            return IntentionUtils.IsValid(function) && function.HasCodeBody() ? function : null;
        }
    }
}
