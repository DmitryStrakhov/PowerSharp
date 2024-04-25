using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.Util;
using JetBrains.TextControl;
using PowerSharp.Utils;
using PowerSharp.Extensions;
using PowerSharp.Services;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Application.Progress;
using PowerSharp.Refactorings.CreateTests;
using JetBrains.Application.UI.Actions.ActionManager;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Create Tests", Description = "Creates a test fixture for a class", GroupType = typeof(CSharpContextActions))]
    public sealed class CreateTestsContextAction : ContextActionBase {
        [NotNull]
        readonly ICSharpContextActionDataProvider dataProvider;
        [NotNull] readonly ISolution solution;
        [CanBeNull] IClassLikeDeclaration declaration;

        public CreateTestsContextAction([NotNull] ICSharpContextActionDataProvider dataProvider) {
            this.dataProvider = dataProvider;
            this.solution = dataProvider.Solution;
        }
        public override string Text { get { return "Create tests"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            ITypeElementResolutionService service = solution.GetComponent<ITypeElementResolutionService>();
            if(!solution.ContainsProject(x => service.ContainsClrType(x, NUnitUtil.MarkerClrName)))
                return false;

            declaration = dataProvider.TryGetClassLikeDeclaration();
            return IntentionUtils.IsValid(declaration) && (declaration is IClassDeclaration || declaration is IStructDeclaration);
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return _ => {
                using(LifetimeDefinition lifetimeDefinition = Lifetime.Define(Lifetime.Eternal))
                    RefactoringActionUtil.ExecuteRefactoring(
                        solution.GetComponent<IActionManager>().DataContexts.CreateOnActiveControl(lifetimeDefinition.Lifetime),
                        new CreateTestsWorkflow(solution, declaration.NotNull()));
            };
        }
    }
}