using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Refactorings;

namespace PowerSharp.Extensions {
    /// <summary>
    ///
    /// Extension-methods for SingleBeRefactoringPage class.
    /// 
    /// </summary>
    public static class SingleBeRefactoringPageExtensions {
        [Pure]
        [NotNull]
        public static IProperty<T> Property<T>(this SingleBeRefactoringPage @this, [NotNull] string id, T value) {
            Guard.IsNotEmpty(id, nameof(id));
            return new Property<T>(@this.Lifetime, id, value);
        }
    }
}