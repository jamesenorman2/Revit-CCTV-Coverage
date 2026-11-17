using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CctvCoverage.Revit.Commands;

/// <summary>
/// MILESTONE M5. For each selected camera (or all cameras if none selected), create a perspective
/// View3D named "CCTV - {Mark}" looking from the lens along pan/tilt. See docs/PLAN.md M5 for the
/// FOV/crop-box procedure and the required empirical check.
/// </summary>
[Transaction(TransactionMode.Manual)]
public sealed class CreateCameraViewsCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("CCTV Coverage", "Not implemented yet — milestone M5 in docs/PLAN.md.");
        return Result.Cancelled;
    }
}
