using System;
using PowerSharp.Properties;
using PowerSharp.Utils;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.Application.Shortcuts.ShortcutManager;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditParametersActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 7733589)]
    public class EditParametersAction : EditDeclarationActionBase<ICSharpTreeNode> {
        protected override ICSharpTreeNode GetDeclaration(IDataContext context) {
            foreach(TreeElement e in context.EnumerateSelectedTreeElementAndAncestors()) {
                switch(e) {
                    case ILocalFunctionDeclaration localFunc:
                        return localFunc.ParameterList;
                    case IMethodDeclaration method:
                        return method.Params;
                    case IIndexerDeclaration indexer:
                        return indexer.Params;
                }
            }
            return null;
        }
    }
}
