using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Util;
using JetBrains.TextControl;
using PowerSharp.Utils;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.Progress;
using PowerSharp.Refactorings.CreateTests;
using JetBrains.Application.UI.Actions.ActionManager;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Create Tests", Description = "Creates a test fixture for a class", Group = "C#")]
    public class CreateTestsContextAction : ContextActionBase {
        [NotNull]
        readonly ICSharpContextActionDataProvider dataProvider;

        public CreateTestsContextAction([NotNull] ICSharpContextActionDataProvider dataProvider) {
            this.dataProvider = dataProvider;
        }
        public override string Text { get { return "Create tests"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            IClassLikeDeclaration declaration = GetClassLikeDeclaration();
            if(!IntentionUtils.IsValid(declaration)) return false;
            return declaration is IClassDeclaration || declaration is IStructDeclaration;
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return textControl => {
                using(LifetimeDefinition lifetimeDefinition = Lifetime.Define(Lifetime.Eternal))
                    RefactoringActionUtil.ExecuteRefactoring(
                        solution.GetComponent<IActionManager>().DataContexts.CreateOnActiveControl(lifetimeDefinition.Lifetime),
                        new CreateTestsWorkflow(solution, null));
            };
        }

        [CanBeNull]
        private IClassLikeDeclaration GetClassLikeDeclaration() {
            ICSharpIdentifier selectedElement = dataProvider.GetSelectedElement<ICSharpIdentifier>();
            if(selectedElement == null) return null;

            TreeTextRange textRange = selectedElement.GetTreeTextRange();
            ref TreeTextRange local1 = ref textRange;
            TreeTextRange selectedTreeRange = dataProvider.SelectedTreeRange;
            ref TreeTextRange local2 = ref selectedTreeRange;
            return local1.Contains(in local2) ? ClassLikeDeclarationNavigator.GetByNameIdentifier(selectedElement) : null;
        }
    }
}