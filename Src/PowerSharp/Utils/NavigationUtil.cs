using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.ReSharper.Feature.Services.Navigation;

namespace PowerSharp.Utils {
    public static class NavigationUtil {
        public static void NavigateTo([NotNull] ISolution solution, [NotNull] IProjectFile projectFile) {
            NavigationOptions options = NavigationOptions.FromWindowContext(Shell.Instance.GetComponent<IMainWindowPopupWindowContext>().Source, "");
            NavigationManager.GetInstance(solution).Navigate(new ProjectFileNavigationPoint(projectFile), options);
        }
    }
}