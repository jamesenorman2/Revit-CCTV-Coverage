using Autodesk.Revit.DB;
using CctvCoverage.Core.Geometry;

namespace CctvCoverage.Revit.Adapters;

/// <summary>
/// Revit's internal length unit is DECIMAL FEET; Core works in METRES. Convert ONLY here,
/// at the boundary. Angles are radians in both.
/// </summary>
public static class Units
{
    public static double FeetToMetres(double feet) => UnitUtils.ConvertFromInternalUnits(feet, UnitTypeId.Meters);
    public static double MetresToFeet(double metres) => UnitUtils.ConvertToInternalUnits(metres, UnitTypeId.Meters);

    public static Vec2 ToPlanMetres(XYZ p) => new(FeetToMetres(p.X), FeetToMetres(p.Y));

    /// <summary>Plan point (m) → Revit XYZ (ft) at elevation <paramref name="zFeet"/>.</summary>
    public static XYZ ToXyz(Vec2 p, double zFeet) => new(MetresToFeet(p.X), MetresToFeet(p.Y), zFeet);
}
