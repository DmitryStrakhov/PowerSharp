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
            IClassLikeDeclaration classDeclaration = (IClassLikeDeclaration)memberDeclaration.GetContainingTypeDeclaration().NotNull();
            
            ForEachAppropriateConstructor(classDeclaration, 
                (@class, @constructor) => {
                    AddAssignmentStatement(constructor, memberDeclaration);
                },
                @class => {
                    IConstructorDeclaration defaultCtor = AddConstructorDeclaration(@class);
                    AddAssignmentStatement(defaultCtor, memberDeclaration);
                }
            );
            return null;
        }
        
        private void ForEachAppropriateConstructor(
            [NotNull] IClassLikeDeclaration classDeclaration,
            [NotNull] Action<IClassLikeDeclaration, IConstructorDeclaration> action,
            [NotNull] Action<IClassLikeDeclaration> fallbackAction) {

            bool hasAny = false;
            foreach(IConstructorDeclaration constructor in classDeclaration.ConstructorDeclarations) {
                if(!IsAppropriateConstructor(constructor)) continue;
                hasAny = true;
                action(classDeclaration, constructor);
            }
            if(!hasAny) {
                fallbackAction(classDeclaration);
            }
        }
        [Pure]
        private bool IsAppropriateConstructor([NotNull] IConstructorDeclaration constructor) {
            return true;
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

            IType typeType = ((ITypeOwner)member.DeclaredElement).NotNull().Type;
            string memberName = member.DeclaredElement.NotNull().ShortName;

            ICSharpStatement assignment = factory.CreateStatement("this.$0 = $1;", memberName, factory.CreateExpression("new $0()", typeType));
            constructorDeclaration.Body.AddStatementAfter(assignment, null);
        }
    }
}
