using System;
using JetBrains.Util;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using PowerSharp.Properties;
using JetBrains.Application.UI.Actions;
using JetBrains.ProjectModel.DataContext;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl.DataContext;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.Application.Shortcuts.ShortcutManager;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.ReSharper.Feature.Services.CSharp.TypingAssist;
using PowerSharp.Utils;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.NewMemberLineActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 1412132)]
    public class NewMemberLineAction : IExecutableAction {
        (ITreeNode node, DocumentOffset docOffset)? insertionPointNullable;
        static readonly string NewLineString =
#if RIDER
            "\n ";
#else
            "\r\n ";
#endif

        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return (insertionPointNullable = GetInsertionPoint(context)) != null;
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            insertionPointNullable.NotNull();
            (ITreeNode node, DocumentOffset docOffset) = insertionPointNullable.Value;

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            editor.NotNull();
            ISolution solution = context.GetData(ProjectModelDataConstants.SOLUTION);
            solution.NotNull();
            IPsiServices psiServices = node.GetPsiServices();

            using(psiServices.Solution.UsingCommand(nameof(NewMemberLineAction))) {
                using(psiServices.Transactions.DocumentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, nameof(NewMemberLineAction))) {
                    using(WriteLockCookie.Create()) {
                        editor.Document.InsertText(docOffset, NewLineString);
                    }
                    psiServices.Files.CommitAllDocuments();
                }
            }
            editor.Caret.MoveTo(docOffset + NewLineString.Length, CaretVisualPlacement.DontScrollIfVisible);
            TypingHelper(solution).DoSmartIndentOnEnterImpl(editor);
        }
        [NotNull]
        static CSharpTypingAssistBase.CSharpIndentTypingHelper TypingHelper([NotNull] ISolution solution) {
            CSharpTypingAssist typingAssist = solution.GetComponent<CSharpTypingAssist>();
            return new CSharpTypingAssistBase.CSharpIndentTypingHelper(typingAssist);
        }

        #endregion

        static (ITreeNode, DocumentOffset)? GetInsertionPoint([NotNull] IDataContext context) {
            (ITreeNode, InsertionKind)? anchorNullable = GetAnchor(context);

            if(anchorNullable != null) {
                DocumentOffset docOffset;
                (ITreeNode node, InsertionKind insertionKind) = anchorNullable.Value;

                switch(insertionKind) {
                    case InsertionKind.Before:
                        docOffset = node.GetPreviousNonWhitespaceSibling().GetDocumentEndOffset();
                        break;
                    case InsertionKind.After:
                        docOffset = node.GetDocumentEndOffset();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"InsertionKind={insertionKind} is not supported.");
                }
                if(docOffset.IsValid())
                    return (node, docOffset);
            }
            return null;
        }
        [CanBeNull]
        static (ITreeNode, InsertionKind)? GetAnchor([NotNull] IDataContext context) {
            foreach(TreeElement e in context.EnumerateSelectedTreeElementAndAncestors()) {
                switch(e) {
                    case IAccessorDeclaration _:
                        break;
                    case ICSharpFunctionDeclaration function:
                        if(function.HasCodeBody())
                            return (function.Body.RBrace, InsertionKind.After);
                        break;
                    case IPropertyDeclaration property:
                        if(!property.IsAbstract)
                            return (property.RBrace, InsertionKind.After);
                        break;
                    case IClassLikeDeclaration @class:
                        return (@class.RBrace, InsertionKind.Before);
                }
            }
            return null;
        }

        enum InsertionKind {
            Before,
            After
        }
    }
}
