using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace PowerSharp.Services {
    /// <summary>
    ///
    /// Service which is used to resolve TypeElements.
    /// Strongly required for code-generation purposes.
    /// 
    /// </summary>
    public interface ITypeElementResolutionService {
        [CanBeNull] ITypeElement Resolve([NotNull] IProject project, [NotNull] string clrTypeName);
        bool ContainsClrType([NotNull] IProject project, [NotNull] string clrTypeName);
    }
}