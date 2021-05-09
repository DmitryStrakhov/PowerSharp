using JetBrains.DataFlow;
using JetBrains.Annotations;
using PowerSharp.Builders;
using PowerSharp.Extensions;
using System.Collections.Generic;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    public sealed class AddFavoriteDependencyPageStartPage : SingleBeRefactoringPage {
        [NotNull] readonly AddFavoriteDependencyDataModel model;
        [NotNull] readonly PageBuilder pageBuilder;
        readonly List<FavoriteDependencyViewModel> dependencyViewList;

        public AddFavoriteDependencyPageStartPage([NotNull] AddFavoriteDependencyWorkflow workflow)
            : base(workflow.WorkflowExecuterLifetime) {
            this.model = workflow.Model;
            this.pageBuilder = new PageBuilder(Lifetime);
            this.dependencyViewList = new List<FavoriteDependencyViewModel>(16);
        }

        public override BeControl GetPageContent() {
            PageBuilder builder = pageBuilder.StartGroupBox();

            foreach(FavoriteDependency dependency in model.DependencyList) {
                bool isInstalled = dependency.IsInstalled(model.Project);
                IProperty<bool> property = this.Property(dependency.Id, isInstalled);

                dependencyViewList.Add(new FavoriteDependencyViewModel(dependency, isInstalled, property));
                builder = builder.CheckBox(property, dependency.Id, !isInstalled);
            }
            return builder.EndGroupBox("NUnit Packages").Content();
        }
        public override void Commit() {
            foreach(FavoriteDependencyViewModel dependencyView in dependencyViewList) {
                if(dependencyView.NeedToInstallProperty.Value && !dependencyView.IsAlreadyInstalled)
                    model.NeedToInstallList.Add(dependencyView.Dependency);
            }
        }

        #region Dependency ViewModel

        private class FavoriteDependencyViewModel {
            public FavoriteDependencyViewModel([NotNull] FavoriteDependency dependency, bool isAlreadyInstalled, [NotNull] IProperty<bool> needToInstallProperty) {
                this.Dependency = dependency;
                this.IsAlreadyInstalled = isAlreadyInstalled;
                this.NeedToInstallProperty = needToInstallProperty;
            }
            public readonly bool IsAlreadyInstalled;
            [NotNull] public readonly FavoriteDependency Dependency;
            [NotNull] public IProperty<bool> NeedToInstallProperty { get; }
        }

        #endregion

        public override string Title { get { return "Add your favorite dependencies"; } }
        public override string Description { get { return "Specify options you want to see"; } }
    }
}