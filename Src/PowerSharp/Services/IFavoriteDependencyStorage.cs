using JetBrains.Annotations;
using System.Collections.Generic;
using PowerSharp.Refactorings.AddFavoriteDependency;

namespace PowerSharp.Services {
    /// <summary>
    ///
    /// Service which is used to work with favorite dependency
    /// storage.
    /// 
    /// </summary>
    public interface IFavoriteDependencyStorage {
        [NotNull, ItemNotNull]
        IReadOnlyCollection<FavoriteDependency> GetDependencies();
    }
}