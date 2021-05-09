using System;
using System.Diagnostics;
using JetBrains.Annotations;
using JetBrains.ProjectModel;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    /// <summary>
    ///
    /// Favorite dependency. It contains nuget-package id and a delegate which allows
    /// to check if certain dependency exists in project or not.
    /// 
    /// </summary>
    [DebuggerDisplay("FavoriteDependency(Id: {" + nameof(Id) + "})")]
    public class FavoriteDependency {
        public FavoriteDependency(string id, [NotNull] Func<IProject, bool> isInstalled) {
            this.Id = id;
            this.IsInstalled = isInstalled;
        }
        public readonly string Id;
        [NotNull] public readonly Func<IProject, bool> IsInstalled;
    }
}