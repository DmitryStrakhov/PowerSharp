using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using PowerSharp.Builders;

namespace PowerSharp.QuickFixes.Components {
    /// <summary>
    ///
    /// Base class for instance-generating services. Provides common
    /// templates methods which are extended in derived classes.
    /// 
    /// </summary>
    public abstract class CreateInstanceServiceBase : ICreateInstanceService {
        [NotNull] 
        readonly ITargetMember member;
        
        protected CreateInstanceServiceBase([NotNull] ITargetMember member) {
            Guard.IsNotNull(member, nameof(member));
            this.member = member;
        }

        public IReadOnlyList<IExpressionStatement> Execute() {
            IClassLikeDeclaration @class = (IClassLikeDeclaration)member.GetContainingTypeDeclaration().NotNull();
            
            bool needDefaultCtor = true;
            LocalList<IExpressionStatement> statementList = new();

            foreach(IConstructorDeclaration constructor in @class.ConstructorDeclarations) {
                if(IsAppropriateConstructor(constructor)) {
                    var stmt = AddObjectInstantiationStatement(constructor, member);
                    statementList.Add(stmt);
                    needDefaultCtor = false;
                }
            }
            if(needDefaultCtor) {
                IConstructorDeclaration defaultCtor = AddDefaultConstructor(@class);
                var stmt = AddObjectInstantiationStatement(defaultCtor, member);
                statementList.Add(stmt);
            }
            return statementList.ReadOnlyList();

            static IExpressionStatement AddObjectInstantiationStatement(IConstructorDeclaration constructor, ITargetMember member) {
                return (IExpressionStatement)new ConstructorBuilder(constructor)
                    .WithBody(codeBuilder => codeBuilder.CreateObjectInstantiationStatement(member));
            }
        }
        public bool IsAvailable() {
            return member.TypeModifiers() is {IsAbstract: false};
        }
        
        [NotNull]
        protected abstract IConstructorDeclaration AddDefaultConstructor([NotNull] IClassLikeDeclaration classDeclaration);
        protected abstract bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor);

        public interface ITargetMember : IMemberToInstantiate {
            ICSharpTypeDeclaration GetContainingTypeDeclaration();
            IModifiersOwner TypeModifiers();
        }
    }
}