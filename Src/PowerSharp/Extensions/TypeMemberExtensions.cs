using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace PowerSharp.Extensions {
    public static class TypeMemberExtensions {
        [NotNull]
        public static IType MemberType(this ITypeMember @this) {
            return ((ITypeOwner)@this).Type;
        }
        [NotNull]
        public static string MemberName(this ITypeMember @this) {
            return @this.ShortName;
        }
    }
}