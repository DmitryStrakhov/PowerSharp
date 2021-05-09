
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.Util.Dotnet.TargetFrameworkIds;

namespace PowerSharp.Extensions {
    /// <summary>
    ///
    /// Extension-methods for IProjectModelTransactionCookie class.
    /// 
    /// </summary>
    public static class ProjectModelTransactionCookieExtensions {
        public static void AddProjectReference(this IProjectModelTransactionCookie @this, [NotNull] IProject targetProject, [NotNull] IProject referencedProject) {
            Guard.IsNotNull(targetProject, nameof(targetProject));
            Guard.IsNotNull(referencedProject, nameof(referencedProject));

            if(!ReferenceEquals(targetProject, referencedProject)) {
                @this.AddModuleReference(targetProject, referencedProject, TargetFrameworkId.Default);
            }
        }
    }
}