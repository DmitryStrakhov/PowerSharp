using JetBrains.ProjectModel;
using System.Collections.Generic;

namespace PowerSharp.Refactorings.AddFavoriteDependency {
    public sealed class AddFavoriteDependencyDataModel {
        public AddFavoriteDependencyDataModel() {
        }
        public IProject Project { get; set; }
        public IReadOnlyCollection<FavoriteDependency> DependencyList { get; set; }
        public List<FavoriteDependency> NeedToInstallList { get; } = new List<FavoriteDependency>(16);
    }
}