using CctvCoverage.Core.Geometry;
using CctvCoverage.Core.Standards;

namespace CctvCoverage.Core.Coverage;

/// <summary>
/// Everything Core needs to know about one camera. All lengths in METRES, angles in RADIANS.
/// The Revit layer builds this from a FamilyInstance (see Adapters/CameraReader.cs).
/// </summary>
public sealed record CameraSpec
{
    /// <summary>Revit UniqueId of the camera instance (used to tag/replace its output).</summary>
    public required string Id { get; init; }
    public string Mark { get; init; } = "";

    /// <summary>Plan position of the lens.</summary>
    public required Vec2 Position { get; init; }

    /// <summary>Lens height above the level the coverage is drawn for.</summary>
    public required double MountHeight { get; init; }

    /// <summary>Viewing direction in plan, anticlockwise from +X (east).</summary>
    public required double PanRadians { get; init; }

    /// <summary>Downward tilt from horizontal. 0 = level, π/2 = looking straight down.</summary>
    public required double TiltRadians { get; init; }

    public required double HfovRadians { get; init; }
    public required double VfovRadians { get; init; }
    public required int HorizontalPixels { get; init; }

    /// <summary>Operational-requirement level this camera is specified to achieve (optional).</summary>
    public DoriLevel? RequiredLevel { get; init; }
}
