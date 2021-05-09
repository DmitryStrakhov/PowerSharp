using System;
using System.Diagnostics;
using JetBrains.Annotations;
using JetBrains.ProjectModel;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
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