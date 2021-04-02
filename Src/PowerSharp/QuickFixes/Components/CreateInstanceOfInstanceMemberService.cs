using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using PowerSharp.Builders;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    public sealed class CreateInstanceOfInstanceMemberService : CreateInstanceServiceBase {
        public CreateInstanceOfInstanceMemberService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void AddObjectInstantiationStatement(IConstructorDeclaration constructor, string memberName, IType type) {
            new ConstructorBuilder(constructor)
                .WithBody(codeBuilder => codeBuilder.CreateObjectInstantiationStatement(memberName, type, true));
        }
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new ClassBuilder(classDeclaration).AddConstructor().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            return true;
        }
    }
}