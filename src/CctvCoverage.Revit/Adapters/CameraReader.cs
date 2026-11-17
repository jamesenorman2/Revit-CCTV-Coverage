using Autodesk.Revit.DB;
using CctvCoverage.Core.Coverage;

namespace CctvCoverage.Revit.Adapters;

/// <summary>MILESTONE M4. Reads camera family instances into Core <see cref="CameraSpec"/>s.</summary>
public static class CameraReader
{
    /// <summary>Categories searched for cameras. Security Devices is standard; some content uses Data/Communication Devices.</summary>
    public static readonly BuiltInCategory[] CameraCategories =
    {
        BuiltInCategory.OST_SecurityDevices,
        BuiltInCategory.OST_DataDevices,
        BuiltInCategory.OST_CommunicationDevices,
    };

    /// <summary>
    /// MILESTONE M4.
    ///  - FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).WherePasses(new ElementMulticategoryFilter(CameraCategories))
    ///  - Keep only instances where LookupParameter("CCTV_HorizontalResolution") exists and has a value &gt; 0
    ///    (that is what makes a device a "camera" — no name matching).
    ///  - Keep only instances whose LevelId == level.Id (for hosted families also check
    ///    instance.Host / Parameter INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM; log and skip if level unknown).
    ///  - Each parameter is looked up on the INSTANCE first, then on the TYPE (doc.GetElement(inst.GetTypeId())).
    ///    Write a small helper: double? GetDouble(FamilyInstance fi, string name).
    ///  - Position: ((LocationPoint)inst.Location).Point → Units.ToPlanMetres.
    ///  - MountHeight: CCTV_MountHeight if set, else (point.Z − level.Elevation); convert ft → m.
    ///  - Pan: atan2 of inst.FacingOrientation (X, Y). If the family is modelled facing the other way,
    ///    the user ticks CCTV_FlipDirection (yes/no) → add π. Then add CCTV_PanOffset (angle) if set.
    ///  - Tilt: CCTV_Tilt (angle, internal radians — no conversion needed). Default 15° if missing (+warning).
    ///  - HFOV: CCTV_HFOV (angle). If missing but CCTV_FocalLength and CCTV_SensorWidth exist
    ///    (both Length → convert ft→mm), use Lens.HorizontalFovFromFocalLength. Else warn + skip.
    ///  - VFOV: CCTV_VFOV if set, else derive from aspect: 2·atan(tan(HFOV/2)·V/H) using CCTV_VerticalResolution.
    ///  - RequiredLevel: DoriLevels.TryParse(CCTV_RequiredDORI).
    ///  - Id = inst.UniqueId, Mark = ALL_MODEL_MARK parameter.
    ///  - Every skipped camera adds a human-readable line to warnings, e.g. "CAM-07: no CCTV_HFOV".
    /// </summary>
    public static IReadOnlyList<CameraSpec> ReadCamerasOnLevel(Document doc, Level level, out List<string> warnings)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }
}
