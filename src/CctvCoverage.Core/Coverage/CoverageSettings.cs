using CctvCoverage.Core.Standards;

namespace CctvCoverage.Core.Coverage;

/// <summary>Project-wide calculation settings. Defaults are sensible; expose in UI in M6.</summary>
public sealed record CoverageSettings
{
    /// <summary>Height above floor at which pixel density and occlusion are evaluated (face height ≈ 1.6 m).</summary>
    public double TargetHeight { get; init; } = 1.6;

    /// <summary>Angular step between rays. 0.5° gives smooth output; smaller = slower.</summary>
    public double RayStepRadians { get; init; } = 0.5 * Math.PI / 180.0;

    /// <summary>Hard cap on drawn range, metres (stops Monitor bands from running 100 m+ down corridors).</summary>
    public double MaxRange { get; init; } = 80.0;

    /// <summary>Levels to draw as bands. Default = the four DORI levels.</summary>
    public IReadOnlyList<DoriLevel> LevelsToDraw { get; init; } = new[]
    {
        DoriLevel.Identify, DoriLevel.Recognise, DoriLevel.Observe, DoriLevel.Detect,
    };
}
