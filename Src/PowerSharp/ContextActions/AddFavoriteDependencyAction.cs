using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Util;
using JetBrains.Lifetimes;
using JetBrains.TextControl;
using PowerSharp.Utils;
using JetBrains.ProjectModel;
using PowerSharp.Services;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using PowerSharp.Refactorings.AddFavoriteDependency;
using JetBrains.Application.UI.Actions.ActionManager;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.Refactorings;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Add Favorite Dependency", Description = "Add your favorite dependency into a project", Group = "C#")]
    public sealed class AddFavoriteDependencyAction: ContextActionBase {
        [NotNull]
        readonly ICSharpContextActionDataProvider dataProvider;
        [NotNull] readonly ISolution solution;

        public AddFavoriteDependencyAction([NotNull] ICSharpContextActionDataProvider dataProvider) {
            this.dataProvider = dataProvider;
            this.solution = dataProvider.Solution;
        }
        public override string Text { get { return "Add favorite dependencies"; } }

        public override bool IsAvailable(IUserDataHolder cache) {
            IClassLikeDeclaration declaration = dataProvider.GetClassLikeDeclaration();
            if(!IntentionUtils.IsValid(declaration)) return false;
            Assertion.Assert(declaration != null, "declaration != null");

            IProject project = declaration.GetProject();
            if(project == null) return false;
            IFavoriteDependencyStorage dependencyStorage = solution.GetComponent<IFavoriteDependencyStorage>();
            return dependencyStorage.GetDependencies().Any(x => !x.IsInstalled(project));
        }
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return _ => {
                using(LifetimeDefinition lifetimeDefinition = Lifetime.Define(Lifetime.Eternal))
                    RefactoringActionUtil.ExecuteRefactoring(
                        solution.GetComponent<IActionManager>().DataContexts.CreateOnActiveControl(lifetimeDefinition.Lifetime),
                        new AddFavoriteDependencyWorkflow(solution));
            };
        }
    }
}