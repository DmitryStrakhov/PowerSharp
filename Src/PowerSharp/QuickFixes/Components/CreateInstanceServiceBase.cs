using JetBrains.Util;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.QuickFixes.Components {
    public abstract class CreateInstanceServiceBase : ICreateInstanceService {
        readonly ITypeMemberDeclaration memberDeclaration;

        protected CreateInstanceServiceBase([NotNull] ITypeMemberDeclaration memberDeclaration) {
            this.memberDeclaration = memberDeclaration;
        }
        
        public void Execute(ISolution solution, IProgressIndicator progress) {
            IClassLikeDeclaration classDeclaration = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();
            ITypeMember declaredElement = memberDeclaration.DeclaredElement.NotNull();

            IType memberType = ((ITypeOwner)declaredElement).Type;
            string memberName = declaredElement.ShortName;
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
            return true;
        }
        protected abstract void AddObjectInstantiationStatement([NotNull] IConstructorDeclaration constructor, [NotNull] string memberName, [NotNull] IType type);
        protected abstract bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor);
        [NotNull] protected abstract IConstructorDeclaration AddDefaultConstructor([NotNull] IClassLikeDeclaration classDeclaration);
    }
}