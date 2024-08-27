﻿using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.TextControl;
using JetBrains.TextControl.DataContext;

namespace PowerSharp.Actions {
    public abstract class EditActionBase : IExecutableAction {
        ICSharpFunctionDeclaration targetMethod;

        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return (targetMethod = GetTargetFunction(context)) != null;
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            targetMethod.NotNull();

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            if(editor == null)
                return;

            DocumentRange range = CalculateSelectionRange(targetMethod);

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
        [CanBeNull]
        protected abstract ICSharpFunctionDeclaration GetTargetFunction(IDataContext context);
    }
}
