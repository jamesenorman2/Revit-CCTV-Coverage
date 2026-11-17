using CctvCoverage.Core.Coverage;
using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Optics;
using CctvCoverage.Core.Standards;
using CctvCoverage.Core.Visibility;
using Xunit;

namespace CctvCoverage.Core.Tests;

// MILESTONE M2: remove every `Skip = M2` when CoverageBuilder.Build is implemented.
public class M2_CoverageBuilderTests
{
    private const string M2 = "M2 not implemented yet";

    // 1920 px, 90° HFOV, mounted 3.6 m, target 1.6 m → h = 2.0
    // Tilt 30°, VFOV 60° → blind zone 1.1547 m, no far limit.
    private static CameraSpec Camera() => new()
    {
        Id = "cam-1",
        Position = new Vec2(0, 0),
        MountHeight = 3.6,
        PanRadians = 0, // looking east (+X)
        TiltRadians = Lens.DegToRad(30),
        HfovRadians = Lens.DegToRad(90),
        VfovRadians = Lens.DegToRad(60),
        HorizontalPixels = 1920,
    };

    private static readonly CoverageSettings Settings = new() { RayStepRadians = Lens.DegToRad(0.1) };
    private static readonly IRayCaster NoWalls = new SegmentRayCaster(Array.Empty<Segment2>());

    [Fact(Skip = M2)]
    public void Band_radii_are_correct()
    {
        var r = new CoverageBuilder(Settings).Build(Camera(), NoWalls);
        var identify = r.Bands.Single(b => b.Level == DoriLevel.Identify);
        var recognise = r.Bands.Single(b => b.Level == DoriLevel.Recognise);
        Assert.Equal(1.1547005, identify.InnerPlanRadius, 5);
        Assert.Equal(3.2780482, identify.OuterPlanRadius, 5);
        Assert.Equal(3.2780482, recognise.InnerPlanRadius, 5);
        Assert.Equal(7.4150118, recognise.OuterPlanRadius, 5);
    }

    [Fact(Skip = M2)]
    public void Bands_are_most_demanding_first()
    {
        var r = new CoverageBuilder(Settings).Build(Camera(), NoWalls);
        Assert.Equal(new[] { DoriLevel.Identify, DoriLevel.Recognise, DoriLevel.Observe, DoriLevel.Detect },
            r.Bands.Select(b => b.Level));
    }

    [Fact(Skip = M2)]
    public void Unobstructed_identify_area_matches_annular_sector()
    {
        // ½ · θ · (r2² − r1²) = (π/4)(3.27805² − 1.15470²) = 7.3924 m²
        var r = new CoverageBuilder(Settings).Build(Camera(), NoWalls);
        var band = r.Bands.Single(b => b.Level == DoriLevel.Identify);
        Assert.Single(band.Polygons);
        Assert.InRange(band.Area, 7.3924 * 0.99, 7.3924 * 1.001);
    }

    [Fact(Skip = M2)]
    public void Wall_clips_coverage()
    {
        var wall = new SegmentRayCaster(new[] { new Segment2(new Vec2(2, -20), new Vec2(2, 20)) });
        var r = new CoverageBuilder(Settings).Build(Camera(), wall);
        foreach (var band in r.Bands)
            foreach (var poly in band.Polygons)
                Assert.All(poly.Vertices, v => Assert.True(v.X <= 2.0 + 1e-6, $"{band.Level} vertex {v} is behind the wall"));
        // Nothing beyond the wall: Observe/Detect start at 7.4 m+ so must be empty.
        Assert.Empty(r.Bands.Single(b => b.Level == DoriLevel.Observe).Polygons);
    }

    [Fact(Skip = M2)]
    public void Wall_with_doorway_splits_band_into_two_polygons_beyond_it()
    {
        // Wall at x = 4 with a 1 m doorway at y = -0.5..0.5.
        // Identify (1.15–3.28 m) is entirely in front of the wall → 1 polygon.
        // Observe (7.42–15.23 m) is entirely behind the wall, visible only through the doorway
        // (azimuths within ±7.1°) → exactly 1 narrow polygon.
        var rc = new SegmentRayCaster(new[]
        {
            new Segment2(new Vec2(4, -30), new Vec2(4, -0.5)),
            new Segment2(new Vec2(4, 0.5), new Vec2(4, 30)),
        });
        var r = new CoverageBuilder(Settings).Build(Camera(), rc);
        Assert.Single(r.Bands.Single(b => b.Level == DoriLevel.Identify).Polygons);
        var observe = r.Bands.Single(b => b.Level == DoriLevel.Observe);
        Assert.Single(observe.Polygons);
        Assert.All(observe.Polygons[0].Vertices, v => Assert.InRange(Math.Abs(v.Y), 0, 5));
    }

    [Fact(Skip = M2)]
    public void Area_at_least_is_cumulative()
    {
        var r = new CoverageBuilder(Settings).Build(Camera(), NoWalls);
        Assert.True(r.AreaAtLeast(DoriLevel.Recognise) > r.AreaAtLeast(DoriLevel.Identify));
        Assert.Equal(r.Bands.Single(b => b.Level == DoriLevel.Identify).Area, r.AreaAtLeast(DoriLevel.Identify), 9);
    }

    [Fact(Skip = M2)]
    public void Pan_rotates_coverage()
    {
        var north = Camera() with { PanRadians = Math.PI / 2 };
        var r = new CoverageBuilder(Settings).Build(north, NoWalls);
        foreach (var v in r.Bands[0].Polygons[0].Vertices)
            Assert.True(v.Y >= -1e-6, $"vertex {v} should be north of the camera");
    }
}
