using System;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using PowerSharp.Extensions;
using PowerSharp.QuickFixes.Services;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Default implementation of ICreateInstanceServiceFactory. Is used to
    /// instantiate certain code-generation service.
    /// 
    /// </summary>
    [SolutionComponent(Instantiation.DemandAnyThreadSafe)]
    public sealed class DefaultCreateInstanceServiceFactory : ICreateInstanceServiceFactory {
        public ICreateInstanceService GetService(ITypeMemberDeclaration memberDeclaration) {
            CreateInstanceServiceBase.ITargetMember targetMember = null;

            if(memberDeclaration is IFieldDeclaration field && CanBeInstantiatedWithNew(field)) {
                targetMember = new FieldMember(field);
            }
            else if(memberDeclaration is IPropertyDeclaration property && CanBeInstantiatedWithNew(property)) {
                targetMember = new PropertyMember(property);
            }

            if(targetMember is not null) {
                return ((IModifiersOwner)memberDeclaration).IsStatic
                    ? new CreateInstanceOfStaticMemberService(targetMember)
                    : new CreateInstanceOfInstanceMemberService(targetMember);
            }
            return NullCreateInstanceService.Instance;

            static bool CanBeInstantiatedWithNew(ITypeOwnerDeclaration typeOwner) {
                return typeOwner.Type is IDeclaredType declaredType
                       && !declaredType.IsEnumType() && !declaredType.IsSimplePredefined();
            }
        }

        abstract class TargetMember<T>(T member)
            : CreateInstanceServiceBase.ITargetMember where T : IClassMemberDeclaration {
            
            protected readonly T Member = member;

            public string Name() {
                return Member.DeclaredElement.MemberName();
            }
            public ICSharpTypeDeclaration GetContainingTypeDeclaration() {
                return Member.GetContainingTypeDeclaration();
            }
            public IModifiersOwner TypeModifiers() {
                return Member.TypeModifiers();
            }
            public abstract ITypeUsage TypeUsage();
        }

        class FieldMember(IFieldDeclaration field) : TargetMember<IFieldDeclaration>(field) {
            public override ITypeUsage TypeUsage() => Member.TypeUsage;
        }
        
        class PropertyMember(IPropertyDeclaration property) : TargetMember<IPropertyDeclaration>(property) {
            public override ITypeUsage TypeUsage() => Member.TypeUsage;
        }
    }
}