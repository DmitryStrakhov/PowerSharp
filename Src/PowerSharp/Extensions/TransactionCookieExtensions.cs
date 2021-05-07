using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.Util.Dotnet.TargetFrameworkIds;

namespace PowerSharp.Extensions {
    public static class ProjectModelTransactionCookieExtensions {
        public static void AddProjectReference(this IProjectModelTransactionCookie @this, [NotNull] IProject targetProject, [NotNull] IProject referencedProject) {
            if(!ReferenceEquals(targetProject, referencedProject)) {
                @this.AddModuleReference(targetProject, referencedProject, TargetFrameworkId.Default);
            }
        }
    }
}