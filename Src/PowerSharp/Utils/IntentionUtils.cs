using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Intentions.Util;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Utils {
    public static class IntentionUtils {
        [Pure]
        public static bool IsValid([CanBeNull] ITypeMemberDeclaration memberDeclaration) {
            if(memberDeclaration?.DeclaredElement == null || !ValidUtils.Valid(memberDeclaration)) return false;
            return true;
        }
    }
}