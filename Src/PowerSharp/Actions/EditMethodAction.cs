using System;
using PowerSharp.Utils;
using PowerSharp.Properties;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.EditMethodActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 1552586)]
    public sealed class EditMethodAction : EditFunctionActionBase {
        protected override IEditFunctionTarget GetTargetFunction(IDataContext context) {
            foreach(TreeElement element in context.EnumerateSelectedTreeElementAndAncestors()) {
                switch(element) {
                    case ICSharpFunctionDeclaration method
                        when IntentionUtils.IsValid(method) && method.HasCodeBody():
                        return new CSharpFunctionTarget(method);
                    
                    case ILocalFunctionDeclaration localFunction
                        when IntentionUtils.IsValid(localFunction) && localFunction.HasCodeBody():
                        return new LocalFunctionTarget(localFunction);
                }
            }
            return null;
        }

        class CSharpFunctionTarget(ICSharpFunctionDeclaration method)
            : IEditFunctionTarget {
            
            readonly ICSharpFunctionDeclaration method = method;
            public IBlock Body => method.Body;
            public ITreeNode Node => method;
            public IArrowExpressionClause ArrowClause => method.ArrowClause;
        }

        class LocalFunctionTarget(ILocalFunctionDeclaration localFunction)
            : IEditFunctionTarget {
            
            readonly ILocalFunctionDeclaration localFunction = localFunction;
            public IBlock Body => localFunction.Body;
            public ITreeNode Node => localFunction;
            public IArrowExpressionClause ArrowClause => localFunction.ArrowClause;
        }
    }
}
