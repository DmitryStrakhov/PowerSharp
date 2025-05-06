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
using PowerSharp.Utils;

namespace PowerSharp.QuickFixes {
    [QuickFix]
    public class CreateInstanceFix([CanBeNull] ITypeMemberDeclaration memberDeclaration) : QuickFixBase {
        readonly ITypeMemberDeclaration memberDeclaration = memberDeclaration;
        ICreateInstanceService serviceCore;

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
        public override string Text { get { return "Create instance"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            if(!IntentionUtils.IsValid(memberDeclaration)) return false;
            return Service.IsAvailable();
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            Service.Execute();
            return null;
        }
        private ICreateInstanceService Service {
            get { return serviceCore ??= memberDeclaration.GetSolution().GetComponent<ICreateInstanceServiceFactory>().GetService(memberDeclaration); }
        }
    }
}