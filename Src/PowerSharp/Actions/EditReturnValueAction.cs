using System;
using PowerSharp.Utils;
using PowerSharp.Properties;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditReturnValueActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 3545589)]
    public class EditReturnValueAction : EditDeclarationActionBase<ITreeNode> {
        protected override ITreeNode GetDeclaration(IDataContext context) {
            foreach(TreeElement e in context.EnumerateSelectedTreeElementAndAncestors()) {
                switch(e) {
                    case ILocalFunctionDeclaration localFunc:
                        return localFunc.TypeUsage;
                    case IMethodDeclaration method:
                        return method.TypeUsage;
                    case IIndexerDeclaration indexer:
                        return indexer.TypeUsage;
                    case IPropertyDeclaration property:
                        return property.TypeUsage;
                }
            }
            return null;
        }
    }
}
