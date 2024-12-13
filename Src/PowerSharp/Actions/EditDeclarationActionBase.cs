using System;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.TextControl.DataContext;
using JetBrains.Diagnostics;

namespace PowerSharp.Actions {
    public abstract class EditDeclarationActionBase<TDeclaration> : IExecutableAction
        where TDeclaration : class, ITreeNode {

        TDeclaration declaration;

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return (declaration = GetDeclaration(context)) != null;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute) {
            declaration.NotNull();

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            if(editor == null)
                return;

            DocumentRange range = new DocumentRange(declaration.GetDocumentStartOffset(), declaration.GetDocumentEndOffset());

            if(range.IsValid())
                editor.Selection.SetRange(range);
        }
        [CanBeNull]
        protected abstract TDeclaration GetDeclaration(IDataContext context);
    }
}
