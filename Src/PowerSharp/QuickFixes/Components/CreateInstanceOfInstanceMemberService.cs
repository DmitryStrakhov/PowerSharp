using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using PowerSharp.Builders;
using PowerSharp.Extensions;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// This service is used to generate instances of non-static entities.
    /// 
    /// </summary>
    public sealed class CreateInstanceOfInstanceMemberService : CreateInstanceServiceBase {
        public CreateInstanceOfInstanceMemberService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void AddObjectInstantiationStatement(IConstructorDeclaration constructor, string memberName, IType memberType) {
            new ConstructorBuilder(constructor)
                .WithBody(codeBuilder => codeBuilder.CreateObjectInstantiationStatement(memberName, memberType, true));
        }
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new MembersBuilder(classDeclaration)
                .AddConstructor().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            if(constructor.HasChainedCall()) return false;
            return true;
        }
    }
}