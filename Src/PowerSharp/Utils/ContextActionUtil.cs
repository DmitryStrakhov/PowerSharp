using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.Utils {
    public static class ContextActionUtil {
        [CanBeNull]
        public static IClassLikeDeclaration GetClassLikeDeclaration([NotNull] ICSharpContextActionDataProvider dataProvider) {
            Guard.IsNotNull(dataProvider, nameof(dataProvider));
            ICSharpIdentifier selectedElement = dataProvider.GetSelectedElement<ICSharpIdentifier>();
            if(selectedElement == null) return null;

            TreeTextRange textRange = selectedElement.GetTreeTextRange();
            ref TreeTextRange local1 = ref textRange;
            TreeTextRange selectedTreeRange = dataProvider.SelectedTreeRange;
            ref TreeTextRange local2 = ref selectedTreeRange;
            return local1.Contains(in local2) ? ClassLikeDeclarationNavigator.GetByNameIdentifier(selectedElement) : null;
        }
    }
}