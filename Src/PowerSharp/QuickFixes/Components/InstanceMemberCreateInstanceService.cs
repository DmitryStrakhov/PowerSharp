using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Components {
    public sealed class InstanceMemberCreateInstanceService : CreateInstanceServiceBase {
        public InstanceMemberCreateInstanceService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void SetAssignmentStatement(IConstructorDeclaration constructor) {
            CSharpElementFactory factory = CSharpElementFactory.GetInstance(constructor);

            IType type = ((ITypeOwner)MemberDeclaration.DeclaredElement).NotNull().Type;
            string memberName = MemberDeclaration.DeclaredElement.NotNull().ShortName;

            ICSharpStatement assignment = factory.CreateStatement("this.$0 = $1;", memberName, factory.CreateExpression("new $0()", type));
            constructor.Body.AddStatementAfter(assignment, null);
        }
        protected override IConstructorDeclaration CreateDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            IConstructorDeclaration decl = CSharpElementFactory.GetInstance(classDeclaration).CreateConstructorDeclaration();
            IConstructorDeclaration ctor = classDeclaration.AddClassMemberDeclaration(decl);

            AccessRights accessRights = classDeclaration.IsAbstract
                ? AccessRights.PROTECTED
                : AccessRights.PUBLIC;

            ctor.SetName(classDeclaration.DeclaredName);
            ctor.SetAccessRights(accessRights);
            return ctor;
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            return true;
        }
    }
}