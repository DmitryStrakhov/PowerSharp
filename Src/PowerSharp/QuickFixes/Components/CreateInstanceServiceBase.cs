using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using PowerSharp.QuickFixes.Services;

namespace PowerSharp.QuickFixes.Components {
    public abstract class CreateInstanceServiceBase : ICreateInstanceService {
        readonly ITypeMemberDeclaration memberDeclaration;

        public CreateInstanceServiceBase([NotNull] ITypeMemberDeclaration memberDeclaration) {
            this.memberDeclaration = memberDeclaration;
        }
        
        public void Execute(ISolution solution, IProgressIndicator progress) {
            IClassLikeDeclaration classDeclaration = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();
            bool shouldCreateDefaultCtor = true;
            
            foreach(IConstructorDeclaration constructor in classDeclaration.ConstructorDeclarations) {
                if(IsAppropriateConstructor(constructor)) {
                    SetAssignmentStatement(constructor);
                    shouldCreateDefaultCtor = false;
                }
            }
            if(shouldCreateDefaultCtor) {
                IConstructorDeclaration defaultConstructor = CreateDefaultConstructor(classDeclaration);
                SetAssignmentStatement(defaultConstructor);
            }
        }
        public bool IsAvailable(IUserDataHolder cache) {
            return true;
        }
        protected ITypeMemberDeclaration MemberDeclaration { get { return memberDeclaration; } }
        protected abstract void SetAssignmentStatement([NotNull] IConstructorDeclaration constructor);
        protected abstract bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor);
        protected abstract IConstructorDeclaration CreateDefaultConstructor([NotNull] IClassLikeDeclaration classDeclaration);
    }
}