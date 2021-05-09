using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using PowerSharp.Services;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Generates annotations (attributes).
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class AnnotationBuilder {
        [NotNull] readonly IAttributesOwnerDeclaration owner;
        [NotNull] readonly CSharpElementFactory factory;

        public AnnotationBuilder([NotNull] IAttributesOwnerDeclaration owner) {
            Guard.IsNotNull(owner, nameof(owner));
            this.owner = owner;
            this.factory = CSharpElementFactory.GetInstance(owner);
        }
        [NotNull]
        public AnnotationBuilder WithAttribute([NotNull] string attributeClassName) {
            Guard.IsNotEmpty(attributeClassName, nameof(attributeClassName));

            ITypeElement typeElement = CreateAttributeTypeElement(attributeClassName);
            owner.AddAttributeAfter(factory.CreateAttribute(typeElement), null);
            return this;
        }
        [NotNull]
        private ITypeElement CreateAttributeTypeElement([NotNull] string attributeClassName) {
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