using System;
using JetBrains.Annotations;
using PowerSharp.Builders;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// This service is used to generate instances of static entities.
    /// 
    /// </summary>
    public sealed class CreateInstanceOfStaticMemberService([NotNull] CreateInstanceServiceBase.ITargetMember member)
        : CreateInstanceServiceBase(member) {
        
        protected override IConstructorDeclaration AddDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            return new MembersBuilder(classDeclaration)
                .AddConstructor(AccessRights.NONE)
                .SetStatic().Unwrap();
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            return constructor.IsStatic;
        }
    }
}