namespace CctvCoverage.Core.Geometry;

/// <summary>A 2D line segment (an obstacle edge in plan), in metres.</summary>
public readonly record struct Segment2(Vec2 A, Vec2 B);
