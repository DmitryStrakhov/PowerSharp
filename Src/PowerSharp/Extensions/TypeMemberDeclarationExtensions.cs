using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Extensions {
    /// <summary>
    ///
    /// Extension-methods for ITypeMemberDeclaration interface.
    /// 
    /// </summary>
    public static class TypeMemberDeclarationExtensions {
        [CanBeNull]
        public static IModifiersOwner TypeModifiers(this ITypeMemberDeclaration @this) {
            IDeclaredType declaredType = @this.DeclaredElement.MemberType() as IDeclaredType;
            return declaredType?.Resolve().DeclaredElement as IModifiersOwner;
        }
    }
}