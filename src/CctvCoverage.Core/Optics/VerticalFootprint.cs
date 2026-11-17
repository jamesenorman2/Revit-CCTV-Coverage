namespace CctvCoverage.Core.Optics;

/// <summary>
/// Where the camera's vertical field of view meets the horizontal target plane,
/// measured as PLAN distance from the point directly below the camera.
/// </summary>
/// <param name="Near">Plan distance where the bottom edge of the image hits the target plane (blind zone inside this).</param>
/// <param name="Far">Plan distance where the top edge hits the plane, or +∞ if the top edge is at/above horizontal.</param>
public readonly record struct VerticalFootprint(double Near, double Far)
{
    /// <summary>
    /// MILESTONE M1.
    /// Inputs: h = cameraHeight − targetHeight (m), tilt = downward tilt from horizontal (rad, 0 = level, π/2 = straight down),
    /// vfov = vertical field of view (rad).
    ///
    /// Algorithm:
    ///   if h &lt;= 0: return (0, +∞)   // camera at/below target plane: treat as no vertical limit
    ///   lower = tilt + vfov/2         // angle of bottom image edge below horizontal
    ///   upper = tilt − vfov/2         // angle of top image edge below horizontal
    ///   Near = lower &gt;= π/2 ? 0 : h / tan(lower)
    ///   Far  = upper &lt;= 0   ? +∞ : h / tan(upper)
    ///
    /// Worked examples (h = 2):
    ///   tilt 30°, vfov 60° → Near = 2/tan60° = 1.1547, Far = +∞
    ///   tilt 45°, vfov 30° → Near = 1.1547,            Far = 2/tan30° = 3.4641
    /// </summary>
    public static VerticalFootprint Compute(double verticalDrop, double tiltRadians, double vfovRadians)
    {
        throw new NotImplementedException("M1: see docs/PLAN.md");
    }
}
