using System;
using JetBrains.ProjectModel.NuGet;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;

namespace PowerSharp {
    [ZoneMarker]
    public class ZoneMarker : IRequire<PsiFeaturesImplZone>, IRequire<DaemonZone>, IRequire<ILanguageCSharpZone>, IRequire<INuGetZone>, IRequire<ICodeEditingZone> {
    }
}