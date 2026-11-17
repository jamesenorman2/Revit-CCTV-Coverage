namespace CctvCoverage.Core.Geometry;

/// <summary>
/// A simple (non-self-intersecting) closed polygon. The closing edge from the last
/// vertex back to the first is implicit: do NOT repeat the first vertex at the end.
/// </summary>
public sealed class Polygon2
{
    public IReadOnlyList<Vec2> Vertices { get; }

    public Polygon2(IReadOnlyList<Vec2> vertices)
    {
        Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
    }

    /// <summary>Signed area via the shoelace formula. Positive = anticlockwise.</summary>
    public double SignedArea
    {
        get
        {
            double sum = 0;
            for (int i = 0; i < Vertices.Count; i++)
            {
                var a = Vertices[i];
                var b = Vertices[(i + 1) % Vertices.Count];
                sum += a.X * b.Y - b.X * a.Y;
            }
            return sum / 2.0;
        }
    }

    public double Area => Math.Abs(SignedArea);

    /// <summary>
    /// MILESTONE M1. Return a cleaned copy suitable for Revit FilledRegion creation:
    ///  1. Remove consecutive vertices closer than <paramref name="minEdge"/> metres apart
    ///     (also check the implicit closing edge last→first).
    ///  2. Remove vertices that are collinear with their neighbours
    ///     (|Cross(b-a, c-b)| / (|b-a|·|c-b|) &lt; <paramref name="collinearTolerance"/>).
    ///  3. Repeat 1–2 until nothing changes.
    ///  4. If fewer than 3 vertices remain, return null (degenerate — caller skips it).
    /// Revit rejects curves shorter than ~0.8 mm (Application.ShortCurveTolerance),
    /// hence the 5 mm default.
    /// </summary>
    public Polygon2? Simplify(double minEdge = 0.005, double collinearTolerance = 1e-6)
    {
        throw new NotImplementedException("M1: see docs/PLAN.md");
    }
}
