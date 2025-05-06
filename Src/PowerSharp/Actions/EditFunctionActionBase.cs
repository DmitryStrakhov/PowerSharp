using System;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.TextControl.DataContext;

namespace PowerSharp.Actions {
    public abstract class EditFunctionActionBase : IExecutableAction {
        IEditFunctionTarget targetFunction;

        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return (targetFunction = GetTargetFunction(context)) != null;
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            targetFunction.NotNull();

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            if(editor == null)
                return;

            DocumentRange range = CalculateSelectionRange(targetFunction);

            if(range.IsValid())
                editor.Selection.SetRange(range);

        }

        #endregion

        private DocumentRange CalculateSelectionRange(IEditFunctionTarget function) {
            if(function.Body != null) {
                return new DocumentRange(
                    function.Body.LBrace.GetNextNonWhitespaceSibling().GetDocumentStartOffset(),
                    function.Body.RBrace.GetPreviousNonWhitespaceSibling().GetDocumentEndOffset()
                );
            }

            if(function.ArrowClause != null && function.Node.LastChild != null) {
                return new DocumentRange(
                    function.ArrowClause.Expression.GetDocumentStartOffset(),
                    function.Node.LastChild.GetDocumentEndOffset()
                );
            }
            return DocumentRange.InvalidRange;
        }
        [CanBeNull]
        protected abstract IEditFunctionTarget GetTargetFunction(IDataContext context);

        protected interface IEditFunctionTarget {
            IBlock Body { get; }
            IArrowExpressionClause ArrowClause { get; }
            ITreeNode Node { get; }
        }
    }
}
