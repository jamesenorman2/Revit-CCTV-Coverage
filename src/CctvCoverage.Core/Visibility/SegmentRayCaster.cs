using CctvCoverage.Core.Geometry;

namespace CctvCoverage.Core.Visibility;

/// <summary>
/// Pure-2D ray caster against a list of obstacle segments. Used by unit tests and
/// usable as a Revit-free fallback (e.g. walls extracted as plan lines).
/// </summary>
public sealed class SegmentRayCaster : IRayCaster
{
    private readonly IReadOnlyList<Segment2> _segments;

    public SegmentRayCaster(IReadOnlyList<Segment2> segments) => _segments = segments;

    /// <summary>
    /// MILESTONE M1. Ray/segment intersection, return the smallest t in (1e-9, maxDistance].
    ///
    /// For ray P + t·D and segment A + u·(B − A):
    ///   E = B − A, denom = Cross(D, E)
    ///   if |denom| &lt; 1e-12: parallel → skip segment
    ///   t = Cross(A − P, E) / denom
    ///   u = Cross(A − P, D) / denom
    ///   hit if t &gt; 1e-9 AND 0 &lt;= u &lt;= 1 AND t &lt;= maxDistance
    /// Return min t over all hits, else null. Assumes |D| = 1 so t is a distance.
    /// </summary>
    public double? Cast(Vec2 origin, Vec2 direction, double maxDistance)
    {
        throw new NotImplementedException("M1: see docs/PLAN.md");
    }
}
