using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Standards;

namespace CctvCoverage.Core.Coverage;

/// <summary>One coloured band for one DORI level. May be several polygons if walls split it.</summary>
public sealed record CoverageBand(DoriLevel Level, double InnerPlanRadius, double OuterPlanRadius, IReadOnlyList<Polygon2> Polygons)
{
    public double Area => Polygons.Sum(p => p.Area);
}

public sealed record CoverageResult(CameraSpec Camera, IReadOnlyList<CoverageBand> Bands)
{
    /// <summary>Total visible area (m²) achieving at least <paramref name="level"/>.</summary>
    public double AreaAtLeast(DoriLevel level) => Bands.Where(b => b.Level >= level).Sum(b => b.Area);
}
