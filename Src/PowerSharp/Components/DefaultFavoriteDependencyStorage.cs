using System;
using System.Collections.Generic;
using PowerSharp.Utils;
using JetBrains.Annotations;
using PowerSharp.Services;
using JetBrains.ProjectModel;
using PowerSharp.Refactorings.AddFavoriteDependency;

namespace PowerSharp.Components {
    [SolutionComponent]
    public class DefaultFavoriteDependencyStorage : IFavoriteDependencyStorage {
        [NotNull] readonly ISolution solution;
        [NotNull] readonly List<FavoriteDependency> dependencyList;

        public DefaultFavoriteDependencyStorage([NotNull] ISolution solution) {
            this.solution = solution;
            this.dependencyList = new List<FavoriteDependency>(16);

            ITypeElementResolutionService service = solution.GetComponent<ITypeElementResolutionService>();
            new StorageInitializer(dependencyList, service).Initialize();
        }

        #region IFavoriteDependencyStorage

        IReadOnlyCollection<FavoriteDependency> IFavoriteDependencyStorage.GetDependencies() {
            return dependencyList;
        }

        #endregion

        class StorageInitializer {
            readonly List<FavoriteDependency> dependencyList;
            readonly ITypeElementResolutionService service;

            public StorageInitializer(List<FavoriteDependency> dependencyList, ITypeElementResolutionService service) {
                this.dependencyList = dependencyList;
                this.service = service;
            }
            public void Initialize() {
                dependencyList.Add(new FavoriteDependency(NUnitUtil.PackageId, x => service.ContainsClrType(x, NUnitUtil.MarkerClrName)));
                dependencyList.Add(new FavoriteDependency(FluentAssertionsUtil.PackageId, x => service.ContainsClrType(x, FluentAssertionsUtil.MarkerClrName)));
            }
        }
    }
}