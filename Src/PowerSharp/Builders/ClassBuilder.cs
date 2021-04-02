using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    public class ClassBuilder {
        readonly IClassLikeDeclaration classDeclaration;
        readonly CSharpElementFactory factory;

        public ClassBuilder([NotNull] IClassLikeDeclaration classDeclaration) {
            Guard.IsNotNull(classDeclaration, nameof(classDeclaration));
            this.classDeclaration = classDeclaration;
            this.factory = CSharpElementFactory.GetInstance(classDeclaration);
        }
        [NotNull]
        public ConstructorBuilder AddConstructor(AccessRights? accessRights = null) {
            IConstructorDeclaration ctor = classDeclaration.AddClassMemberDeclaration(factory.CreateConstructorDeclaration());

            AccessRights rights = accessRights ?? (classDeclaration.IsAbstract
                ? AccessRights.PROTECTED
                : AccessRights.PUBLIC);

            ctor.SetName(classDeclaration.DeclaredName);
            ctor.SetAccessRights(rights);
            return new ConstructorBuilder(ctor);
        }
    }
}