using System;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Resources.Shell;

namespace PowerSharp {
    [ZoneMarker]
    public class ZoneMarker : IRequire<PsiFeaturesImplZone>, IRequire<DaemonZone> {
    }
}