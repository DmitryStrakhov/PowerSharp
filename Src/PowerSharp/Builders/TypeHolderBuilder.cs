using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Generates class-like declaration.
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class TypeHolderBuilder {
        [NotNull] readonly ICSharpTypeAndNamespaceHolderDeclaration typeHolder;
        [NotNull] readonly CSharpElementFactory factory;

        public TypeHolderBuilder([NotNull] ICSharpTypeAndNamespaceHolderDeclaration typeHolder) {
            Guard.IsNotNull(typeHolder, nameof(typeHolder));
            this.typeHolder = typeHolder;
            this.factory = CSharpElementFactory.GetInstance(typeHolder);
        }

        [NotNull]
        public AnnotationBuilder AddClass([NotNull] string className, AccessRights accessRights) {
            Guard.IsNotEmpty(className, nameof(className));
            ICSharpTypeDeclaration declaration = (IClassLikeDeclaration)factory.CreateTypeMemberDeclaration("class $0 {}", className);
            declaration.SetAccessRights(accessRights);
            return new AnnotationBuilder((IClassLikeDeclaration)typeHolder.AddTypeDeclarationAfter(declaration, null));
        }
    }
}