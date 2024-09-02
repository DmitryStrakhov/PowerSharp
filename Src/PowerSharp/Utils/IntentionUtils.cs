using JetBrains.Annotations;
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Utils {
    public static class IntentionUtils {
        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsValid([CanBeNull] ITypeMemberDeclaration memberDeclaration) {
            return memberDeclaration?.DeclaredElement != null && ValidUtils.Valid(memberDeclaration);
        }
        [Pure]
        [ContractAnnotation("null => false")]
        public static bool IsValid([CanBeNull] IFunctionDeclaration functionDeclaration) {
            return functionDeclaration?.DeclaredElement != null && ValidUtils.Valid(functionDeclaration);
        }
    }
}