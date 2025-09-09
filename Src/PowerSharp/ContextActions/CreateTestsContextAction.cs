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
        [NotNull] readonly ICSharpContextActionDataProvider dataProvider;
        [CanBeNull] IClassLikeDeclaration declaration;

        public CreateTestsContextAction([NotNull] ICSharpContextActionDataProvider dataProvider) {
            this.dataProvider = dataProvider;
        }
        public override string Text { get { return "Create tests"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            declaration = dataProvider.TryGetClassLikeDeclaration();
            
            if(IntentionUtils.IsValid(declaration) && declaration is IClassDeclaration or IStructDeclaration) {
                return dataProvider.Solution.ContainsProject(x =>
                    dataProvider.Solution.GetComponent<ITypeElementResolutionService>().ContainsClrType(x, NUnitUtil.MarkerClrName));
            }
            return false;
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            using LifetimeDefinition lifetimeDefinition = Lifetime.Define(Lifetime.Eternal);

            RefactoringActionUtil.ExecuteRefactoring(
                solution.GetComponent<IActionManager>().DataContexts.CreateOnActiveControl(lifetimeDefinition.Lifetime),
                new CreateTestsWorkflow(solution, declaration.NotNull()));

            return null;
        }
    }
}