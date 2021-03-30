using System;
using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.QuickFixes.Components {
    public sealed class StaticMemberCreateInstanceService : CreateInstanceServiceBase {
        public StaticMemberCreateInstanceService([NotNull] ITypeMemberDeclaration memberDeclaration)
            : base(memberDeclaration) {
        }

        protected override void SetAssignmentStatement(IConstructorDeclaration constructor) {
            CSharpElementFactory factory = CSharpElementFactory.GetInstance(constructor);

            IType type = ((ITypeOwner)MemberDeclaration.DeclaredElement).NotNull().Type;
            string memberName = MemberDeclaration.DeclaredElement.NotNull().ShortName;

            ICSharpStatement assignment = factory.CreateStatement("$0 = $1;", memberName, factory.CreateExpression("new $0()", type));
            constructor.Body.AddStatementAfter(assignment, null);
        }
        protected override bool IsAppropriateConstructor(IConstructorDeclaration constructor) {
            return constructor.IsStatic;
        }
        protected override IConstructorDeclaration CreateDefaultConstructor(IClassLikeDeclaration classDeclaration) {
            IConstructorDeclaration decl = CSharpElementFactory.GetInstance(classDeclaration).CreateConstructorDeclaration();
            IConstructorDeclaration ctor = classDeclaration.AddClassMemberDeclaration(decl);

            ctor.SetStatic(true);
            ctor.SetName(classDeclaration.DeclaredName);
            ctor.SetAccessRights(AccessRights.NONE);
            return ctor;
        }
    }
}