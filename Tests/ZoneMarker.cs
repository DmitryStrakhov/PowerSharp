using NUnit.Framework;
using System.Threading;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.TestFramework.Application.Zones;
using JetBrains.Application.BuildScript.Application.Zones;

[assembly: Apartment(ApartmentState.STA)]

namespace PowerSharp.Tests {
    [ZoneDefinition]
    public class PowerSharpTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone> {
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<PowerSharpTestEnvironmentZone> {
    }
}