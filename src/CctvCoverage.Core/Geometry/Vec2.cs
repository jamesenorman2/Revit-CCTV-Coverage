namespace CctvCoverage.Core.Geometry;

/// <summary>
/// Immutable 2D vector / point in plan, in METRES. X = project east, Y = project north
/// (Revit internal X/Y after conversion from feet).
/// </summary>
public readonly record struct Vec2(double X, double Y)
{
    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator *(Vec2 a, double s) => new(a.X * s, a.Y * s);
    public static Vec2 operator *(double s, Vec2 a) => new(a.X * s, a.Y * s);

    public double Length => Math.Sqrt(X * X + Y * Y);

    public static double Dot(Vec2 a, Vec2 b) => a.X * b.X + a.Y * b.Y;

    /// <summary>2D cross product (z component of a × b).</summary>
    public static double Cross(Vec2 a, Vec2 b) => a.X * b.Y - a.Y * b.X;

    public Vec2 Normalized()
    {
        var len = Length;
        if (len < 1e-12) throw new InvalidOperationException("Cannot normalise a zero-length vector.");
        return new Vec2(X / len, Y / len);
    }

    /// <summary>Unit vector at <paramref name="radians"/> measured anticlockwise from +X.</summary>
    public static Vec2 FromAngle(double radians) => new(Math.Cos(radians), Math.Sin(radians));

    /// <summary>Angle of this vector anticlockwise from +X, in radians, range (-π, π].</summary>
    public double Angle => Math.Atan2(Y, X);
}
