using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;

namespace PowerSharp.Services {
    public interface ITypeElementResolutionService {
        [CanBeNull] ITypeElement Resolve([NotNull] IProject project, string clrTypeName);
        bool ContainsClrType([NotNull] IProject project, string clrTypeName);
    }
}