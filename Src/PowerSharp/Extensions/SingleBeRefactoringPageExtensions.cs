using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Extensions {
    public static class SingleBeRefactoringPageExtensions {
        [Pure]
        [NotNull]
        public static IProperty<T> NewProperty<T>(this SingleBeRefactoringPage @this, [NotNull] string id, T value) {
            return new Property<T>(@this.Lifetime, id, value);
        }
    }
}