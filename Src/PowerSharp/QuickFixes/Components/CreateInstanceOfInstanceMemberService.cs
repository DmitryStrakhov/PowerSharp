using JetBrains.Annotations;
using PowerSharp.Builders;
using PowerSharp.Extensions;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// This service is used to generate instances of non-static entities.
    /// 
    /// </summary>
    public sealed class CreateInstanceOfInstanceMemberService([NotNull] CreateInstanceServiceBase.ITargetMember member)
        : CreateInstanceServiceBase(member) {
        
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new MembersBuilder(classDeclaration).AddConstructor().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            if(constructor.HasChainedCall()) return false;
            return true;
        }
    }
}