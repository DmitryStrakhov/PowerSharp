using System;
using JetBrains.Annotations;
using PowerSharp.Builders;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Components {
    public sealed class StaticMemberCreateInstanceService : CreateInstanceServiceBase {
        public StaticMemberCreateInstanceService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void AddObjectInstantiationStatement(IConstructorDeclaration constructor, string memberName, IType type) {
            new ConstructorBuilder(constructor)
                .WithBody(codeBuilder => codeBuilder.CreateObjectInstantiationStatement(memberName, type, false));
        }
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new ClassBuilder(classDeclaration)
                .AddConstructor(AccessRights.NONE)
                .SetStatic().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            return constructor.IsStatic;
        }
    }
}