using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Builders;

public interface IMemberToInstantiate {
    string Name();
    ITypeUsage TypeUsage();
}
