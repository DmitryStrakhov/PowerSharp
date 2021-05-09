using System;
using System.Collections.Generic;
using PowerSharp.Utils;
using JetBrains.Annotations;
using PowerSharp.Services;
using JetBrains.ProjectModel;
using PowerSharp.Refactorings.AddFavoriteDependency;

namespace PowerSharp.Components {
    /// <summary>
    /// 
    /// Default implementation of IFavoriteDependencyStorage service.
    /// At the moment, the list  of favorite dependencies is hard-coded here. Needs to be made customizable
    /// through the settings somewhere in the future.
    /// 
    /// </summary>
    [SolutionComponent]
    public class DefaultFavoriteDependencyStorage : IFavoriteDependencyStorage {
        [NotNull] readonly ISolution solution;
        [NotNull] readonly List<FavoriteDependency> dependencyList;

        public DefaultFavoriteDependencyStorage([NotNull] ISolution solution) {
            Guard.IsNotNull(solution, nameof(solution));
            this.solution = solution;
            this.dependencyList = new List<FavoriteDependency>(16);

            ITypeElementResolutionService service = solution.GetComponent<ITypeElementResolutionService>();
            new DefaultStorageInitializer(dependencyList, service).Initialize();
        }

        #region IFavoriteDependencyStorage

        IReadOnlyCollection<FavoriteDependency> IFavoriteDependencyStorage.GetDependencies() {
            return dependencyList;
        }

        #endregion

        private class DefaultStorageInitializer {
            [NotNull] readonly ITypeElementResolutionService resolutionService;
            [ItemNotNull] readonly List<FavoriteDependency> dependencyList;

            public DefaultStorageInitializer([NotNull] List<FavoriteDependency> dependencyList, [NotNull] ITypeElementResolutionService resolutionService) {
                this.dependencyList = dependencyList;
                this.resolutionService = resolutionService;
            }
            public void Initialize() {
                dependencyList.Add(new FavoriteDependency(NUnitUtil.PackageId, x => {
                    return resolutionService.ContainsClrType(x, NUnitUtil.MarkerClrName);
                }));
                dependencyList.Add(new FavoriteDependency(FluentAssertionsUtil.PackageId, x => {
                    return resolutionService.ContainsClrType(x, FluentAssertionsUtil.MarkerClrName);
                }));
            }
        }
    }
}