using System;
using Should;
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
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace PowerSharp.Actions {
    [Action(typeof(PowerSharpResource), nameof(PowerSharpResource.NewMemberLineActionText), ShortcutScope = ShortcutScope.TextEditor, Id = 1552586)]
    public class NewMemberLineAction : IExecutableAction {
        ITreeNode insertionPoint;
        static readonly string NewLineString = Environment.NewLine + " ";

        #region IExecutableAction

        bool IExecutableAction.Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate) {
            return (insertionPoint = GetInsertionPoint(context)) != null;
        }

        void IExecutableAction.Execute(IDataContext context, DelegateExecute nextExecute) {
            insertionPoint.NotNull();
            DocumentOffset insertionPointOffset = insertionPoint.GetDocumentEndOffset();
            insertionPointOffset.IsValid().ShouldBeTrue();

            ITextControl editor = context.GetData(TextControlDataConstants.TEXT_CONTROL);
            editor.NotNull();
            ISolution solution = context.GetData(ProjectModelDataConstants.SOLUTION);
            solution.NotNull();
            IPsiServices psiServices = insertionPoint.GetPsiServices();

            using(psiServices.Solution.UsingCommand(nameof(NewMemberLineAction))) {
                using(psiServices.Transactions.DocumentTransactionManager.CreateTransactionCookie(DefaultAction.Commit, nameof(NewMemberLineAction))) {
                    using(WriteLockCookie.Create()) {
                        editor.Document.InsertText(insertionPoint.GetDocumentEndOffset(), NewLineString);
                    }

                    psiServices.Files.CommitAllDocuments();
                }
            }
            editor.Caret.MoveTo(insertionPointOffset + Environment.NewLine.Length, CaretVisualPlacement.DontScrollIfVisible);
            TypingHelper(solution).DoSmartIndentOnEnterImpl(editor);
        }
        [NotNull]
        static CSharpTypingAssistBase.CSharpIndentTypingHelper TypingHelper([NotNull] ISolution solution) {
            CSharpTypingAssist typingAssist = solution.GetComponent<CSharpTypingAssist>();
            return new CSharpTypingAssistBase.CSharpIndentTypingHelper(typingAssist);
        }

        #endregion

        [CanBeNull]
        static ITreeNode GetInsertionPoint([NotNull] IDataContext context) {
            TreeElement treeElement = context.GetSelectedTreeNode<TreeElement>();

            for(TreeElement e = treeElement; e != null; e = e is ISandBox holder ? (TreeElement) holder.GetParentNode() : e.parent) {
                switch (e) {
                    case IAccessorDeclaration _:
                        break;
                    case ICSharpFunctionDeclaration function:
                        if(function.HasCodeBody())
                            return function.Body.RBrace;
                        break;
                    case IPropertyDeclaration property:
                        if(!property.IsAbstract)
                            return property.RBrace;
                        break;
                    case IClassLikeDeclaration @class:
                        if(@class.MethodDeclarations.Count != 0) {
                            return @class.MemberDeclarations.Last();
                        }
                        break;
                }
            }
            return null;
        }
    }
}
