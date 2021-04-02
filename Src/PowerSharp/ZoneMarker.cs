using System;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Application.BuildScript.Application.Zones;

namespace PowerSharp {
    [ZoneMarker]
    public class ZoneMarker : IRequire<PsiFeaturesImplZone>, IRequire<DaemonZone>, IRequire<ILanguageCSharpZone> {
    }
}