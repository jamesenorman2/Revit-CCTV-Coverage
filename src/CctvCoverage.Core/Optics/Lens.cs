namespace CctvCoverage.Core.Optics;

public static class Lens
{
    /// <summary>
    /// Horizontal field of view (radians) from focal length and sensor width (both in mm):
    /// HFOV = 2·atan(sensorWidth / (2·focalLength)).
    /// Prefer the manufacturer's quoted HFOV where available (it includes lens distortion).
    /// </summary>
    public static double HorizontalFovFromFocalLength(double focalLengthMm, double sensorWidthMm)
    {
        if (focalLengthMm <= 0) throw new ArgumentOutOfRangeException(nameof(focalLengthMm));
        if (sensorWidthMm <= 0) throw new ArgumentOutOfRangeException(nameof(sensorWidthMm));
        return 2.0 * Math.Atan(sensorWidthMm / (2.0 * focalLengthMm));
    }

    public static double DegToRad(double degrees) => degrees * Math.PI / 180.0;
    public static double RadToDeg(double radians) => radians * 180.0 / Math.PI;
}
