using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;

namespace PowerSharp.Utils {
    public static class ContextActionDataProviderExtensions {
        [CanBeNull]
        public static IClassLikeDeclaration GetClassLikeDeclaration([NotNull] this IContextActionDataProvider @this) {
            return GetTreeNodeBySelectedIdentifier(@this, identifier => ClassLikeDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IMethodDeclaration GetMethodDeclaration([NotNull] this IContextActionDataProvider @this) {
            return GetTreeNodeBySelectedIdentifier(@this, identifier => MethodDeclarationNavigator.GetByNameIdentifier(identifier));
        }
        [CanBeNull]
        public static IPropertyDeclaration GetPropertyDeclaration([NotNull] this IContextActionDataProvider @this) {
            return GetTreeNodeBySelectedIdentifier(@this, identifier => PropertyDeclarationNavigator.GetByNameIdentifier(identifier));
        }

        [CanBeNull]
        static T GetTreeNodeBySelectedIdentifier<T>(IContextActionDataProvider dataProvider, Func<ICSharpIdentifier, T> getNode)
            where T : class, ITreeNode {

            ICSharpIdentifier selectedElement = dataProvider.GetSelectedElement<ICSharpIdentifier>();
            if(selectedElement == null) return null;

            TreeTextRange textRange = selectedElement.GetTreeTextRange();
            ref TreeTextRange local1 = ref textRange;
            TreeTextRange selectedTreeRange = dataProvider.SelectedTreeRange;
            ref TreeTextRange local2 = ref selectedTreeRange;
            return local1.Contains(in local2) ? getNode(selectedElement) : null;
        }
    }
}
