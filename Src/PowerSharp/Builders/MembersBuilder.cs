using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Generates type members like constructors, methods, properties etc.
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class MembersBuilder {
        [NotNull] readonly IClassLikeDeclaration classDeclaration;
        [NotNull] readonly CSharpElementFactory factory;

        public MembersBuilder([NotNull] IClassLikeDeclaration classDeclaration) {
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
        [NotNull]
        public AnnotationBuilder AddVoidMethod([NotNull] string methodName, AccessRights accessRights) {
            Guard.IsNotEmpty(methodName, nameof(methodName));

            IMethodDeclaration method = (IMethodDeclaration)factory.CreateTypeMemberDeclaration("void Foo() {}");
            method.SetAccessRights(accessRights);
            method.SetName(methodName);
            return new AnnotationBuilder(classDeclaration.AddClassMemberDeclaration(method));
        }
    }
}