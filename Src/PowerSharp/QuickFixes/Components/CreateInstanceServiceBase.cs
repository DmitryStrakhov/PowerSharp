using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using PowerSharp.Extensions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Base class for instance-generating services. Provides common
    /// templates methods which are extended in derived classes.
    /// 
    /// </summary>
    public abstract class CreateInstanceServiceBase : ICreateInstanceService {
        [NotNull] readonly ITypeMemberDeclaration memberDeclaration;
        [NotNull] readonly ITypeMember memberElement;

        protected CreateInstanceServiceBase([NotNull] ITypeMemberDeclaration memberDeclaration) {
            Guard.IsNotNull(memberDeclaration, nameof(memberDeclaration));
            this.memberDeclaration = memberDeclaration;
            this.memberElement = memberDeclaration.DeclaredElement.NotNull();
        }

        public void Execute() {
            IClassLikeDeclaration @class = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();

            IType memberType = memberElement.MemberType();
            string memberName = memberElement.MemberName();
            bool shouldCreateDefaultCtor = true;

            foreach(IConstructorDeclaration constructor in @class.ConstructorDeclarations) {
                if(IsAppropriateConstructor(constructor)) {
                    AddObjectInstantiationStatement(constructor, memberName, memberType);
                    shouldCreateDefaultCtor = false;
                }
            }
            if(shouldCreateDefaultCtor) {
                IConstructorDeclaration defaultCtor = AddDefaultConstructor(@class);
                AddObjectInstantiationStatement(defaultCtor, memberName, memberType);
            }
        }
        public bool IsAvailable() {
            IModifiersOwner modifiers = memberDeclaration.TypeModifiers();
            if(modifiers == null || modifiers.IsAbstract) return false;
            return true;
        }
        protected abstract void AddObjectInstantiationStatement([NotNull] IConstructorDeclaration constructor, [NotNull] string memberName, [NotNull] IType memberType);
        protected abstract bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor);
        [NotNull] protected abstract IConstructorDeclaration AddDefaultConstructor([NotNull] IClassLikeDeclaration classDeclaration);
    }
}