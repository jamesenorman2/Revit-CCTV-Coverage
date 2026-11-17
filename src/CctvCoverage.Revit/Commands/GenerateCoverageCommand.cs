using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CctvCoverage.Revit.Commands;

/// <summary>
/// MILESTONE M4. The main command. Runs in the ACTIVE FLOOR PLAN view.
///
///  1. Guard: doc.ActiveView must be a ViewPlan with ViewType.FloorPlan and a non-null GenLevel,
///     else TaskDialog "Open a floor plan first" and return Result.Cancelled.
///  2. cameras = CameraReader.ReadCamerasOnLevel(doc, view.GenLevel, out var warnings).
///     If none: TaskDialog explaining which categories/parameters are looked for. Return Cancelled.
///  3. var view3d = RaycastViewProvider.GetOrCreate(doc)   (needs its own transaction — do it FIRST).
///  4. settings = new CoverageSettings() (M6 replaces this with saved settings).
///  5. For each camera: var caster = new RevitRayCaster(view3d, levelElevationFt + targetHeightFt, settings);
///     results.Add(new CoverageBuilder(settings).Build(camera, caster)).
///     Wrap each camera in try/catch: one bad camera adds a warning, it must NOT abort the run.
///  6. One Transaction("CCTV: generate coverage"):
///        FilledRegionWriter.DeleteExisting(doc, view)   // removes previous output in THIS view only
///        FilledRegionWriter.Write(doc, view, results, warnings)
///  7. TaskDialog summary: N cameras, per camera "Mark: Identify x m², Recognise y m² …",
///     plus warnings and any camera whose RequiredLevel area is 0 (flag as "does not meet OR").
/// </summary>
[Transaction(TransactionMode.Manual)]
public sealed class GenerateCoverageCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("CCTV Coverage", "Not implemented yet — milestone M4 in docs/PLAN.md.");
        return Result.Cancelled;
    }
}
