using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.TextControl;
using JetBrains.TextControl.DataContext;

namespace PowerSharp.Utils {
    public static class ActionDataProviderExtensions {
        [CanBeNull, Pure]
        public static IClassLikeDeclaration TryGetClassLikeDeclaration([NotNull] this IContextActionDataProvider @this) {
            return TryGetSelectedDeclaration(
                () => @this.GetSelectedElement<ICSharpIdentifier>(),
                () => @this.SelectedTreeRange,
                identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier)
            );
        }
        [CanBeNull, Pure]
        public static IClassLikeDeclaration TryGetClassLikeDeclaration([NotNull] this IDataContext @this) {
            return TryGetSelectedDeclaration(
                () => @this.GetSelectedTreeNode<ICSharpIdentifier>(),
                null,
                identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier)
            );
        }
        [NotNull, Pure]
        public static IEnumerable<TreeElement> EnumerateSelectedTreeElementAndAncestors([NotNull] this IDataContext context, bool considerSelection = true) {
            TreeElement treeElement = context.GetSelectedTreeNode<TreeElement>(considerSelection);

            for(TreeElement e = treeElement; e != null; e = e is ISandBox holder ? (TreeElement) holder.GetParentNode() : e.parent)
                yield return e;
        }
        
        [CanBeNull, Pure]
        public static TNode GetSelectedTreeNode<TNode>(this IDataContext @this, bool considerSelection)
            where TNode : class, ITreeNode {
            
            ITextControl textControl = @this.GetData(TextControlDataConstants.TEXT_CONTROL);
            if(textControl == null)
                return null;

            ISolution solution = @this.GetData(ProjectModelDataConstants.SOLUTION);
            if(solution == null)
                return null;

            TNode element = TextControlToPsi.GetElementFromCaretPosition<TNode>(solution, textControl);
            if(element == null && considerSelection) {
                DocumentRange range = textControl.SingleSelectionRange();
                if(range != DocumentRange.InvalidRange) {
                    IFile file = TextControlToPsi
                        .GetPsiFilesFromDocument(solution, range, SourceFilesMask.ALL_FOR_PROJECT_FILE, PsiLanguageCategories.All)
                        .SingleOrDefault();

                    element = file?.FindNodeAt(range)?.GetContainingNode<TNode>();
                }
            }
            return element;
        }

        [CanBeNull]
        static TOutputNode TryGetSelectedDeclaration<TSelectedNode, TOutputNode>(
            [NotNull] Func<TSelectedNode> getSelectedNode,
            [CanBeNull] Func<TreeTextRange> getSelectedTextRange,
            [NotNull] Func<TSelectedNode, TOutputNode> getOutputNode
        )
            where TSelectedNode : class, ITreeNode
            where TOutputNode : class, ITreeNode {

            TSelectedNode selectedNode = getSelectedNode();
            if(selectedNode == null)
                return null;

            if(getSelectedTextRange == null)
                return getOutputNode(selectedNode);

            TreeTextRange textRange = selectedNode.GetTreeTextRange();
            TreeTextRange selectedTreeRange = getSelectedTextRange();
            ref TreeTextRange local2 = ref selectedTreeRange;
            ref TreeTextRange local1 = ref textRange;
            return local1.Contains(in local2) ? getOutputNode(selectedNode) : null;
        }
    }
}
