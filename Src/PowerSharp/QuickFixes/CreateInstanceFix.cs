using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using PowerSharp.QuickFixes.Services;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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
            var statementList = Service.Execute();
            if(statementList.Count != 1)
                return null;
            
            IExpressionStatement initializeStmt = statementList[0];
            return textControl => {
                IObjectCreationExpression objectCreationExpression = (IObjectCreationExpression)((IAssignmentExpression)initializeStmt.Expression).Source;
                DocumentOffset targetOffset = objectCreationExpression.LBound.GetDocumentStartOffset();
                textControl.Caret.MoveTo(targetOffset + 1, CaretVisualPlacement.DontScrollIfVisible);
            };
        }
        private ICreateInstanceService Service {
            get { return serviceCore ??= memberDeclaration.GetSolution().GetComponent<ICreateInstanceServiceFactory>().GetService(memberDeclaration); }
        }
    }
}