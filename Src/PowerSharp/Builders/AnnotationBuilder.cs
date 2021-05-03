using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    public class AnnotationBuilder {
        [NotNull] readonly IAttributesOwnerDeclaration owner;
        [NotNull] readonly CSharpElementFactory factory;

        public AnnotationBuilder([NotNull] IAttributesOwnerDeclaration owner) {
            this.owner = owner;
            this.factory = CSharpElementFactory.GetInstance(owner);
        }
        [NotNull]
        public AnnotationBuilder WithAttribute(string attributeClassName) {
            if(string.IsNullOrEmpty(attributeClassName)) {
                throw new ArgumentException(nameof(attributeClassName));
            }
            
            ITypeElement typeElement = CreateAttributeTypeElement(attributeClassName);
            owner.AddAttributeAfter(factory.CreateAttribute(typeElement), null);
            return this;
        }
        [NotNull]
        private ITypeElement CreateAttributeTypeElement(string attributeClassName) {
            IDeclaredType declaredType = TypeFactory.CreateTypeByCLRName(attributeClassName, owner.GetPsiModule());
            return declaredType.GetTypeElement().NotNull();
        }

        [NotNull]
        public MembersBuilder WithMembers() {
            IClassLikeDeclaration classDeclaration = owner as IClassLikeDeclaration;
            return classDeclaration != null ? new MembersBuilder(classDeclaration) : throw new InvalidOperationException();
        }
    }
}