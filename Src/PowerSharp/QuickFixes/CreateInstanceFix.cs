using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.QuickFixes;

namespace PowerSharp.QuickFixes {
    [QuickFix]
    public class CreateInstanceFix : QuickFixBase {
        readonly ITypeMemberDeclaration memberDeclaration;
        ICreateInstanceService serviceCore;

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
            return Service.IsAvailable(cache);
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            Service.Execute(solution, progress);
            return null;
        }
        private ICreateInstanceService Service {
            get { return serviceCore ?? (serviceCore = memberDeclaration.GetSolution().GetComponent<ICreateInstanceServiceFactory>().GetService(memberDeclaration)); }
        }
    }
}