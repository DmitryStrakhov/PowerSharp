using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using PowerSharp.Services;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.NuGet.PackageManagement;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    public class AddFavoriteDependencyWorkflow : DrivenRefactoringWorkflow2<AddFavoriteDependencyHelper> {
        public AddFavoriteDependencyWorkflow(ISolution solution, [NotNull] IProject project)
            : this(solution) {
            Model.Project = project;
        }

        public AddFavoriteDependencyWorkflow([NotNull] ISolution solution, [CanBeNull] string actionId = null)
            : base(solution, actionId) {
            Model = new AddFavoriteDependencyDataModel();
        }

        public AddFavoriteDependencyDataModel Model { get; }

        public override bool Initialize(IDataContext context) {
            IProject project = IsAvailableCore(context)?.GetProject();
            Assertion.Assert(project != null, "project != null");

            if(project != null)
                Model.Project = project;

            Model.DependencyList = Solution.GetComponent<IFavoriteDependencyStorage>().GetDependencies();
            return Model.DependencyList.Count != 0;
        }
        public override bool IsAvailable(IDataContext context) {
            IDeclaration declaration = IsAvailableCore(context);
            return declaration?.GetProject() != null;
        }

        private IDeclaration IsAvailableCore([NotNull] IDataContext context) {
            IDeclaredElement declaredElement = context.GetData(RefactoringDataConstants.DeclaredElementWithoutSelection);
            if(declaredElement == null) return null;
            return Helper[declaredElement.PresentationLanguage].GetTypeDeclaration(context);
        }
        public override IRefactoringExecuter CreateRefactoring(IRefactoringDriver driver) {
            return new AddFavoriteDependencyRefactoring(this, Solution, driver);
        }
        protected override AddFavoriteDependencyHelper CreateUnsupportedHelper() {
            return new AddFavoriteDependencyHelper();
        }
        protected override AddFavoriteDependencyHelper CreateHelper(IRefactoringLanguageService service) {
            return new AddFavoriteDependencyHelper();
        }
        public override IRefactoringPage FirstPendingRefactoringPage {
            get { return new AddFavoriteDependencyPageStartPage(this); }
        }
        public override bool PreExecute(IProgressIndicator pi) {
            List<FavoriteDependency> installList = Model.NeedToInstallList;
            if(installList.Count == 0) return true;

            NuGetNativePackageManager packageManager = Solution.GetComponent<NuGetNativePackageManager>();

            foreach(FavoriteDependency dependency in installList) {
                packageManager.InstallLatestPackage(Model.Project, dependency.Id);
            }
            return true;
        }
        public override string Title { get { return "Add Favorite Dependency"; } }
        public override string HelpKeyword { get { return "Refactorings__Add_Favorite_Dependency"; } }
        public override bool MightModifyManyDocuments { get { return true; } }
        public override RefactoringActionGroup ActionGroup { get { return RefactoringActionGroup.Blessed; } }
    }
}