using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Extensions {
    /// <summary>
    /// 
    /// Extension-methods for IConstructorDeclaration interface.
    /// 
    /// </summary>
    public static class ConstructorDeclarationExtensions {
        public static bool HasChainedCall(this IConstructorDeclaration @this) {
            return @this.Initializer != null
                   && @this.Initializer.Kind == ConstructorInitializerKind.THIS;
        }
    }
}