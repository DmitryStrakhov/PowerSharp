using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using PowerSharp.Services;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

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
            IProject project = owner.GetProject();
            if(project == null) throw new InvalidOperationException();

            ITypeElementResolutionService service = owner.GetSolution().GetComponent<ITypeElementResolutionService>();
            ITypeElement typeElement = service.Resolve(project, attributeClassName);
            return typeElement ?? throw new InvalidOperationException();
        }

        [NotNull]
        public MembersBuilder WithMembers() {
            IClassLikeDeclaration classDeclaration = owner as IClassLikeDeclaration;
            return classDeclaration != null ? new MembersBuilder(classDeclaration) : throw new InvalidOperationException();
        }
    }
}