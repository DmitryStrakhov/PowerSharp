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
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IMethodDeclaration TryGetMethodDeclaration([NotNull] this IContextActionDataProvider @this) {
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => MethodDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IPropertyDeclaration TryGetPropertyDeclaration([NotNull] this IContextActionDataProvider @this) {
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => PropertyDeclarationNavigator.GetByNameIdentifier(identifier));
        }

        public static IClassLikeDeclaration TryGetClassLikeDeclaration([NotNull] this IDataContext @this) {
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IMethodDeclaration TryGetMethodDeclaration([NotNull] this IDataContext @this) {
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => MethodDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IPropertyDeclaration TryGetPropertyDeclaration([NotNull] this IDataContext @this) {
            return TryGetTreeNodeBySelectedIdentifier(@this, identifier => PropertyDeclarationNavigator.GetByNameIdentifier(identifier));
        }


        [CanBeNull]
        static T TryGetTreeNodeBySelectedIdentifier<T>(IContextActionDataProvider dataProvider, Func<ICSharpIdentifier, T> getNode)
            where T : class, ITreeNode {
            return TryGetSelectedDeclaration(() => dataProvider.GetSelectedElement<ICSharpIdentifier>(), () => dataProvider.SelectedTreeRange, getNode);
        }
        [CanBeNull]
        static T TryGetTreeNodeBySelectedIdentifier<T>(IDataContext context, Func<ICSharpIdentifier, T> getNode)
            where T : class, ITreeNode {
            return TryGetSelectedDeclaration(() => context.GetSelectedTreeNode<ICSharpIdentifier>(), null, getNode);
        }

        [CanBeNull]
        static T TryGetSelectedDeclaration<T>(
            [NotNull] Func<ICSharpIdentifier> getSelectedIdentifier,
            [CanBeNull] Func<TreeTextRange> getSelectedTextRange,
            [NotNull] Func<ICSharpIdentifier, T> getNode
        )
            where T : class, ITreeNode {

            var selectedId = getSelectedIdentifier();
            if(selectedId == null) return null;

            TreeTextRange textRange = selectedId.GetTreeTextRange();

            if(getSelectedTextRange == null)
                return getNode(selectedId);

            var selectedTreeRange = getSelectedTextRange();
            ref TreeTextRange local2 = ref selectedTreeRange;
            ref TreeTextRange local1 = ref textRange;
            return local1.Contains(in local2) ? getNode(selectedId) : null;
        }
    }
}
