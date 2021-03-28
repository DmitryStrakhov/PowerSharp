using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle.Suggestions;

namespace PowerSharp.QuickFixes {
    [QuickFix]
    public class CreateInstanceFix : QuickFixBase {
        readonly ITypeMemberDeclaration memberDeclaration;

        public CreateInstanceFix([CanBeNull] ITypeMemberDeclaration memberDeclaration) {
            this.memberDeclaration = memberDeclaration;
        }

        public CreateInstanceFix([NotNull] UnusedMemberWarningBase warning)
            : this(warning.Declaration as ITypeMemberDeclaration) {
        }
        public CreateInstanceFix([NotNull] UnusedFieldWarningBase warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnusedFieldCompilerWarning warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnassignedFieldWarningBase warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnassignedFieldCompilerWarning warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnassignedReadonlyFieldWarning warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnassignedReadonlyFieldCompilerWarning warning)
            : this(warning.Declaration) {
        }
        public CreateInstanceFix([NotNull] UnassignedGetOnlyAutoPropertyWarning warning)
            : this(warning.Declaration) {
        }
        public override string Text { get { return "Create Instance"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            if(memberDeclaration == null) return false;

            return true;
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            IClassLikeDeclaration classLikeDeclaration = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();
            IConstructorDeclaration ctor = AddConstructorDeclaration(classLikeDeclaration);
            AddAssignmentStatement(ctor, memberDeclaration);
            return null;
        }

        [Pure]
        private IConstructorDeclaration AddConstructorDeclaration([NotNull] IClassLikeDeclaration classLikeDeclaration) {
            IConstructorDeclaration decl = CSharpElementFactory.GetInstance(classLikeDeclaration).CreateConstructorDeclaration();
            IConstructorDeclaration ctor = classLikeDeclaration.AddClassMemberDeclaration(decl);

            AccessRights accessRights = classLikeDeclaration.IsAbstract
                ? AccessRights.PROTECTED
                : AccessRights.PUBLIC;

            ctor.SetName(classLikeDeclaration.DeclaredName);
            ctor.SetAccessRights(accessRights);
            return ctor;
        }
        private void AddAssignmentStatement([NotNull] IConstructorDeclaration constructorDeclaration, [NotNull] ITypeMemberDeclaration member) {
            CSharpElementFactory factory = CSharpElementFactory.GetInstance(constructorDeclaration);

            string memberName = member.DeclaredElement.NotNull().ShortName;

            ITypeOwner type = member.DeclaredElement as ITypeOwner;
            IType typeType = type.Type;

            IExpression objectCreationExpression = factory.CreateExpression("new $0()", typeType);
            ICSharpStatement assignStatement = factory.CreateStatement("this.$0 = $1;", memberName, objectCreationExpression);
            constructorDeclaration.Body.AddStatementAfter(assignStatement, null);
        }
    }
}
