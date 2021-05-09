using JetBrains.Annotations;
using System.Collections.Generic;
using PowerSharp.Refactorings.AddFavoriteDependency;

namespace PowerSharp.Services {
    public interface IFavoriteDependencyStorage {
        [NotNull, ItemNotNull]
        IReadOnlyCollection<FavoriteDependency> GetDependencies();
    }
}