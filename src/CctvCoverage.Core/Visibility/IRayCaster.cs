using CctvCoverage.Core.Geometry;

namespace CctvCoverage.Core.Visibility;

/// <summary>
/// The ONLY seam between the maths and the BIM model. Core never knows about Revit:
/// the Revit project implements this with ReferenceIntersector, tests implement it with
/// <see cref="SegmentRayCaster"/>.
/// </summary>
public interface IRayCaster
{
    /// <summary>
    /// Cast a horizontal ray in plan from <paramref name="origin"/> in unit direction <paramref name="direction"/>.
    /// Returns the plan distance (m) to the nearest obstacle, or null if nothing is hit within <paramref name="maxDistance"/>.
    /// </summary>
    double? Cast(Vec2 origin, Vec2 direction, double maxDistance);
}
