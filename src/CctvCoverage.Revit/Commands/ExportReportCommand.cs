using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CctvCoverage.Revit.Commands;

/// <summary>
/// MILESTONE M6. Recompute coverage for ALL cameras on ALL levels (no drawing) and write a CSV:
/// Level, Mark, Required, Identify m², Recognise m², Observe m², Detect m², MeetsRequirement (Y/N).
/// Ask for the path with a standard Windows SaveFileDialog (Microsoft.Win32.SaveFileDialog needs UseWPF;
/// simplest alternative: write next to the .rvt as "{ProjectName}_CCTV_Coverage.csv" and tell the user).
/// </summary>
[Transaction(TransactionMode.ReadOnly)]
public sealed class ExportReportCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("CCTV Coverage", "Not implemented yet — milestone M6 in docs/PLAN.md.");
        return Result.Cancelled;
    }
}
