using System;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;

namespace PowerSharp.Utils {
    public static class ActionDataProviderExtensions {
        public static IClassLikeDeclaration TryGetClassLikeDeclaration([NotNull] this IContextActionDataProvider @this) {
            return TryGetSelectedDeclaration(() => @this.GetSelectedElement<ICSharpIdentifier>(), () => @this.SelectedTreeRange, identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        public static IClassLikeDeclaration TryGetClassLikeDeclaration([NotNull] this IDataContext @this) {
            return TryGetSelectedDeclaration(() => @this.GetSelectedTreeNode<ICSharpIdentifier>(), null, identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier));
        }

        [CanBeNull]
        public static IMethodDeclaration TryGetMethodDeclaration([NotNull] this IDataContext @this) {
            return TryGetSelectedDeclaration(() => @this.GetSelectedTreeNode<ICSharpTreeNode>(), null, csharpTreeNode => csharpTreeNode.GetContainingNode<IMethodDeclaration>(true));
        }
        [CanBeNull]
        public static IPropertyDeclaration TryGetPropertyDeclaration([NotNull] this IDataContext @this) {
            return TryGetSelectedDeclaration(() => @this.GetSelectedTreeNode<ICSharpTreeNode>(), null, csharpTreeNode => csharpTreeNode.GetContainingNode<IPropertyDeclaration>());
        }
        [CanBeNull]
        public static IAccessorDeclaration TryGetAccessorDeclaration([NotNull] this IDataContext @this) {
            return TryGetSelectedDeclaration(() => @this.GetSelectedTreeNode<ICSharpTreeNode>(), null, csharpTreeNode => csharpTreeNode.GetContainingNode<IAccessorDeclaration>());
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
