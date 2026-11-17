using Autodesk.Revit.DB;
using CctvCoverage.Core.Coverage;
using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Visibility;

namespace CctvCoverage.Revit.Adapters;

/// <summary>
/// MILESTONE M4. Implements Core's IRayCaster with Revit's ReferenceIntersector.
/// Rays are HORIZONTAL at the target plane (level elevation + target height), so anything
/// that crosses that height (walls, columns, closed doors, furniture taller than 1.6 m) blocks view.
/// </summary>
public sealed class RevitRayCaster : IRayCaster
{
    /// <summary>Categories that block view. Doors/windows/curtain panels are toggled in M6 settings.</summary>
    public static readonly BuiltInCategory[] ObstacleCategories =
    {
        BuiltInCategory.OST_Walls,
        BuiltInCategory.OST_StructuralColumns,
        BuiltInCategory.OST_Columns,
        BuiltInCategory.OST_Doors,
        BuiltInCategory.OST_CurtainWallPanels,
        BuiltInCategory.OST_CurtainWallMullions,
        BuiltInCategory.OST_Stairs,
    };

    private readonly double _zFeet;
    private readonly ReferenceIntersector _intersector;

    /// <param name="view3d">From RaycastViewProvider.GetOrCreate.</param>
    /// <param name="targetPlaneElevationFeet">level.Elevation + Units.MetresToFeet(settings.TargetHeight).</param>
    public RevitRayCaster(View3D view3d, double targetPlaneElevationFeet, CoverageSettings settings)
    {
        _zFeet = targetPlaneElevationFeet;
        _intersector = new ReferenceIntersector(new ElementMulticategoryFilter(ObstacleCategories), FindReferenceTarget.Face, view3d)
        {
            FindReferencesInRevitLinks = true,
        };
    }

    /// <summary>
    /// MILESTONE M4.
    ///   var originFt = Units.ToXyz(origin, _zFeet);
    ///   var dir = new XYZ(direction.X, direction.Y, 0);   // unit vector — no unit conversion for directions
    ///   var hit = _intersector.FindNearest(originFt, dir);
    ///   if (hit == null) return null;
    ///   var d = Units.FeetToMetres(hit.Proximity);
    ///   return d &lt;= maxDistance ? d : null;
    ///
    /// GOTCHA: if the camera is mounted ON a wall, the ray starts inside/at that wall's face and FindNearest
    /// returns ~0. Fix: ignore hits with Proximity &lt; 0.05 m by using _intersector.Find(originFt, dir)
    /// (all hits), ordering by Proximity and taking the first with d &gt;= 0.05 m.
    /// Implement it that way from the start.
    /// </summary>
    public double? Cast(Vec2 origin, Vec2 direction, double maxDistance)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }
}
