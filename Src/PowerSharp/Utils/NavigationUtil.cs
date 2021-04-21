using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Application.UI.PopupLayout;
using JetBrains.ReSharper.Feature.Services.Navigation;

namespace PowerSharp.Utils {
    public static class NavigationUtil {
        public static void NavigateTo([NotNull] ISolution solution, [NotNull] IProjectFile projectFile) {
            IMainWindowPopupWindowContext context = Shell.Instance.GetComponent<IMainWindowPopupWindowContext>();
            NavigationManager.GetInstance(solution).Navigate(new ProjectFileNavigationPoint(projectFile), NavigationOptions.FromWindowContext(context.Source, ""));
        }
    }
}