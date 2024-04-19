using System;
using JetBrains.TextControl;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl.DataContext;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;

namespace PowerSharp.Actions {
    public abstract class EditActionBase : IExecutableAction {
        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return IsAvailable(context);
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            if(editor == null)
                return;

            ICSharpFunctionDeclaration function = GetTargetFunction(context);
            if(function == null)
                return;

            DocumentRange range = CalculateSelectionRange(function);

            if(range.IsValid())
                editor.Selection.SetRange(range);
        }

        #endregion

        private DocumentRange CalculateSelectionRange(ICSharpFunctionDeclaration function) {
            if(function.Body != null) {
                return new DocumentRange(
                    function.Body.LBrace.GetNextNonWhitespaceSibling().GetDocumentStartOffset(),
                    function.Body.RBrace.GetPreviousNonWhitespaceSibling().GetDocumentEndOffset()
                );
            }

            if(function.ArrowClause != null && function.LastChild != null) {
                return new DocumentRange(
                    function.ArrowClause.Expression.GetDocumentStartOffset(),
                    function.LastChild.GetDocumentEndOffset()
                );
            }
            return DocumentRange.InvalidRange;
        }
        protected abstract bool IsAvailable(IDataContext context);

        [CanBeNull]
        protected abstract ICSharpFunctionDeclaration GetTargetFunction(IDataContext context);
    }
}
