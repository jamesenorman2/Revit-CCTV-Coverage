using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Optics;
using CctvCoverage.Core.Visibility;
using Xunit;

namespace CctvCoverage.Core.Tests;

// MILESTONE M1: remove every `Skip = M1` when you implement the M1 stubs. All must pass.
public class M1_GeometryTests
{
    private const string M1 = "M1 not implemented yet";

    [Fact(Skip = M1)]
    public void Footprint_top_edge_above_horizontal_is_unbounded()
    {
        var fp = VerticalFootprint.Compute(2.0, Lens.DegToRad(30), Lens.DegToRad(60));
        Assert.Equal(1.1547005, fp.Near, 6);
        Assert.True(double.IsPositiveInfinity(fp.Far));
    }

    [Fact(Skip = M1)]
    public void Footprint_steep_tilt_is_bounded()
    {
        var fp = VerticalFootprint.Compute(2.0, Lens.DegToRad(45), Lens.DegToRad(30));
        Assert.Equal(1.1547005, fp.Near, 6);
        Assert.Equal(3.4641016, fp.Far, 6);
    }

    [Fact(Skip = M1)]
    public void Footprint_straight_down_has_no_blind_zone()
    {
        var fp = VerticalFootprint.Compute(2.0, Lens.DegToRad(90), Lens.DegToRad(60));
        Assert.Equal(0.0, fp.Near, 9);
    }

    [Fact(Skip = M1)]
    public void Footprint_camera_below_target_is_unlimited()
    {
        var fp = VerticalFootprint.Compute(-0.5, Lens.DegToRad(10), Lens.DegToRad(40));
        Assert.Equal(0.0, fp.Near, 9);
        Assert.True(double.IsPositiveInfinity(fp.Far));
    }

    private static readonly SegmentRayCaster WallAtX5 =
        new(new[] { new Segment2(new Vec2(5, -10), new Vec2(5, 10)) });

    [Fact(Skip = M1)]
    public void Ray_hits_wall_straight_ahead() =>
        Assert.Equal(5.0, WallAtX5.Cast(new Vec2(0, 0), new Vec2(1, 0), 100)!.Value, 9);

    [Fact(Skip = M1)]
    public void Ray_hits_wall_at_45_degrees() =>
        Assert.Equal(5 * Math.Sqrt(2), WallAtX5.Cast(new Vec2(0, 0), Vec2.FromAngle(Math.PI / 4), 100)!.Value, 9);

    [Fact(Skip = M1)]
    public void Ray_pointing_away_misses() =>
        Assert.Null(WallAtX5.Cast(new Vec2(0, 0), new Vec2(-1, 0), 100));

    [Fact(Skip = M1)]
    public void Ray_beyond_max_distance_misses() =>
        Assert.Null(WallAtX5.Cast(new Vec2(0, 0), new Vec2(1, 0), 4.9));

    [Fact(Skip = M1)]
    public void Nearest_of_several_walls_wins()
    {
        var rc = new SegmentRayCaster(new[]
        {
            new Segment2(new Vec2(5, -10), new Vec2(5, 10)),
            new Segment2(new Vec2(3, -10), new Vec2(3, 10)),
        });
        Assert.Equal(3.0, rc.Cast(new Vec2(0, 0), new Vec2(1, 0), 100)!.Value, 9);
    }

    [Fact(Skip = M1)]
    public void Simplify_removes_collinear_and_duplicate_points()
    {
        var square = new Polygon2(new[]
        {
            new Vec2(0, 0), new Vec2(0.5, 0), new Vec2(1, 0), new Vec2(1, 0.0001),
            new Vec2(1, 1), new Vec2(0, 1),
        });
        var s = square.Simplify()!;
        Assert.Equal(4, s.Vertices.Count);
        Assert.Equal(1.0, s.Area, 6);
    }

    [Fact(Skip = M1)]
    public void Simplify_returns_null_for_degenerate()
    {
        var sliver = new Polygon2(new[] { new Vec2(0, 0), new Vec2(1, 0), new Vec2(2, 0) });
        Assert.Null(sliver.Simplify());
    }

    [Fact]
    public void Polygon_area_unit_square() // already implemented — keep passing
    {
        var sq = new Polygon2(new[] { new Vec2(0, 0), new Vec2(1, 0), new Vec2(1, 1), new Vec2(0, 1) });
        Assert.Equal(1.0, sq.SignedArea, 9);
    }
}
