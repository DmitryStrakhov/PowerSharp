using System;
using PowerSharp.Properties;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.Application.Shortcuts.ShortcutManager;
using JetBrains.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl.DataContext;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.ForceMethodCallActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 2542689)]
    public class ForceMethodCallAction : IExecutableAction {
        ITreeNode selectedNode;
        const string MethodCallString = "();";

        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            selectedNode = context.GetSelectedTreeNode<ITreeNode>();
            return selectedNode?.GetContainingNode<IClassLikeDeclaration>() != null;
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            selectedNode.NotNull();

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            editor.NotNull();

            IPsiServices psiServices = selectedNode.GetPsiServices();

            using(psiServices.Solution.UsingCommand(nameof(NewMemberLineAction))) {
                using(psiServices.Transactions.DocumentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, nameof(NewMemberLineAction))) {
                    using(WriteLockCookie.Create()) {
                        editor.Document.InsertText(editor.Caret.DocumentOffset(), MethodCallString);
                    }

                    psiServices.Files.CommitAllDocuments();
                }
            }
            editor.Caret.MoveTo(editor.Caret.DocumentOffset() - 2, CaretVisualPlacement.DontScrollIfVisible);
        }

        #endregion
    }
}
