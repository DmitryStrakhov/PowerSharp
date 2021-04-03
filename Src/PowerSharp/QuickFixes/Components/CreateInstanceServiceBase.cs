using System;
using JetBrains.Util;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using PowerSharp.Extensions;

namespace PowerSharp.QuickFixes.Components {
    public abstract class CreateInstanceServiceBase : ICreateInstanceService {
        [NotNull] readonly ITypeMemberDeclaration memberDeclaration;
        [NotNull] readonly ITypeMember memberElement;

        protected CreateInstanceServiceBase([NotNull] ITypeMemberDeclaration memberDeclaration) {
            this.memberDeclaration = memberDeclaration;
            this.memberElement = memberDeclaration.DeclaredElement.NotNull();
        }

        public void Execute(ISolution solution, IProgressIndicator progress) {
            IClassLikeDeclaration classDeclaration = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();

            IType memberType = memberElement.MemberType();
            string memberName = memberElement.MemberName();
            bool shouldCreateDefaultCtor = true;

            foreach(IConstructorDeclaration constructor in classDeclaration.ConstructorDeclarations) {
                if(IsAppropriateConstructor(constructor)) {
                    AddObjectInstantiationStatement(constructor, memberName, memberType);
                    shouldCreateDefaultCtor = false;
                }
            }
            if(shouldCreateDefaultCtor) {
                IConstructorDeclaration defaultCtor = AddDefaultConstructor(classDeclaration);
                AddObjectInstantiationStatement(defaultCtor, memberName, memberType);
            }
        }
        public bool IsAvailable(IUserDataHolder cache) {
            IModifiersOwner modifiers = memberDeclaration.TypeModifiers();
            if(modifiers == null || modifiers.IsAbstract) return false;
            return true;
        }
        protected abstract void AddObjectInstantiationStatement([NotNull] IConstructorDeclaration constructor, [NotNull] string memberName, [NotNull] IType memberType);
        protected abstract bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor);
        [NotNull] protected abstract IConstructorDeclaration AddDefaultConstructor([NotNull] IClassLikeDeclaration classDeclaration);
    }
}