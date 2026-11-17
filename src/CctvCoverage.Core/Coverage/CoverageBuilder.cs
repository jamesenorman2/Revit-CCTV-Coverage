using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Optics;
using CctvCoverage.Core.Standards;
using CctvCoverage.Core.Visibility;

namespace CctvCoverage.Core.Coverage;

/// <summary>
/// Turns a camera + obstacles into DORI band polygons. This is the heart of the tool.
/// Read docs/PLAN.md §"Algorithm" before touching it.
/// </summary>
public sealed class CoverageBuilder
{
    private readonly CoverageSettings _settings;

    public CoverageBuilder(CoverageSettings settings) => _settings = settings;

    /// <summary>
    /// MILESTONE M2. Steps:
    ///
    /// 1. h = camera.MountHeight − settings.TargetHeight.
    ///    fp = VerticalFootprint.Compute(h, camera.TiltRadians, camera.VfovRadians).
    ///
    /// 2. Band radii (PLAN distances). Sort settings.LevelsToDraw most-demanding first.
    ///    For each level L: outer(L) = min( PixelDensity.PlanDistanceFromSlant(
    ///          PixelDensity.MaxSlantDistance(px, hfov, DoriLevels.RequiredPixelsPerMetre(L)), h),
    ///        fp.Far, settings.MaxRange ).
    ///    inner(first level) = fp.Near; inner(next level) = outer(previous level).
    ///    Clamp: inner = max(inner, fp.Near). If outer &lt;= inner the band is empty (no polygons, still return it).
    ///
    /// 3. Azimuth samples: n = max(2, ceil(hfov / settings.RayStepRadians)) + 1 samples evenly from
    ///    pan − hfov/2 to pan + hfov/2 INCLUSIVE.
    ///    For each sample i: dir_i = Vec2.FromAngle(θ_i);
    ///    vis_i = rayCaster.Cast(camera.Position, dir_i, maxOuter) ?? maxOuter
    ///    where maxOuter = largest outer radius over all bands. Cast ONCE per azimuth, reuse for every band.
    ///
    /// 4. For each band, per sample: o_i = min(outer, vis_i), n_i = inner.
    ///    Sample i is "lit" if o_i &gt; n_i + 1e-6.
    ///    Each maximal run of consecutive lit samples [s..e] (needs e − s &gt;= 1) becomes one polygon:
    ///      vertices = [P(θ_s, o_s) .. P(θ_e, o_e)]  then  [P(θ_e, n_e) .. P(θ_s, n_s)]  (reverse)
    ///    where P(θ, r) = camera.Position + r·Vec2.FromAngle(θ).
    ///    If inner == 0 the inner arc collapses to the camera point: emit camera.Position once instead.
    ///    Run each polygon through Polygon2.Simplify(); drop nulls.
    ///
    /// 5. Return new CoverageResult(camera, bands) with bands in most-demanding-first order.
    ///
    /// Note: an obstacle closer than `inner` blocks the whole band along that azimuth (o_i &lt;= n_i) — correct.
    /// </summary>
    public CoverageResult Build(CameraSpec camera, IRayCaster rayCaster)
    {
        throw new NotImplementedException("M2: see docs/PLAN.md");
    }
}
