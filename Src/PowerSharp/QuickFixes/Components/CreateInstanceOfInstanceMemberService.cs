using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using PowerSharp.Builders;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using PowerSharp.Extensions;

namespace PowerSharp.QuickFixes.Components {
    public sealed class CreateInstanceOfInstanceMemberService : CreateInstanceServiceBase {
        public CreateInstanceOfInstanceMemberService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void AddObjectInstantiationStatement(IConstructorDeclaration constructor, string memberName, IType memberType) {
            new ConstructorBuilder(constructor)
                .WithBody(codeBuilder => codeBuilder.CreateObjectInstantiationStatement(memberName, memberType, true));
        }
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new ClassBuilder(classDeclaration)
                .AddConstructor().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            if(constructor.HasChainedCall()) return false;
            return true;
        }
    }
}