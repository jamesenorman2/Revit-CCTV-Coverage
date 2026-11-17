namespace CctvCoverage.Core.Optics;

/// <summary>
/// Pixel-density maths. "Distance" here is the SLANT distance from the lens to the target
/// point (3D), not the plan distance. Convert with <see cref="PlanDistanceFromSlant"/>.
/// The width of scene covered at distance d is 2·d·tan(HFOV/2); spreading H pixels
/// across it gives PPM = H / (2·d·tan(HFOV/2)).
/// </summary>
public static class PixelDensity
{
    /// <summary>Pixels per metre at slant distance <paramref name="slantDistance"/> (m).</summary>
    public static double AtDistance(int horizontalPixels, double hfovRadians, double slantDistance)
    {
        Validate(horizontalPixels, hfovRadians);
        if (slantDistance <= 0) throw new ArgumentOutOfRangeException(nameof(slantDistance));
        return horizontalPixels / (2.0 * slantDistance * Math.Tan(hfovRadians / 2.0));
    }

    /// <summary>Furthest slant distance (m) at which <paramref name="requiredPpm"/> is still achieved.</summary>
    public static double MaxSlantDistance(int horizontalPixels, double hfovRadians, double requiredPpm)
    {
        Validate(horizontalPixels, hfovRadians);
        if (requiredPpm <= 0) throw new ArgumentOutOfRangeException(nameof(requiredPpm));
        return horizontalPixels / (2.0 * Math.Tan(hfovRadians / 2.0) * requiredPpm);
    }

    /// <summary>
    /// Plan (horizontal) distance corresponding to a slant distance, given the vertical drop
    /// h = cameraHeight − targetHeight. Returns 0 if slant &lt;= |h| (the band never reaches the target plane).
    /// </summary>
    public static double PlanDistanceFromSlant(double slantDistance, double verticalDrop)
    {
        var sq = slantDistance * slantDistance - verticalDrop * verticalDrop;
        return sq <= 0 ? 0 : Math.Sqrt(sq);
    }

    private static void Validate(int horizontalPixels, double hfovRadians)
    {
        if (horizontalPixels <= 0) throw new ArgumentOutOfRangeException(nameof(horizontalPixels));
        // tan(HFOV/2) blows up at 180°. Panoramic/fisheye (>= 180°) needs dewarped maths — out of scope (see PLAN "Non-goals").
        if (hfovRadians <= 0 || hfovRadians >= Math.PI)
            throw new ArgumentOutOfRangeException(nameof(hfovRadians), "HFOV must be in (0°, 180°).");
    }
}
