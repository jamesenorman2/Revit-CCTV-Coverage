using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CctvCoverage.Revit.Commands;

/// <summary>
/// MILESTONE M3. Bind every parameter in CctvSharedParameters.txt (group "CCTV") as an INSTANCE
/// parameter to the categories in <see cref="Adapters.CameraReader.CameraCategories"/>.
///
/// Steps:
///  1. var app = commandData.Application.Application;
///     remember app.SharedParametersFilename, then set it to the .txt next to this DLL
///     (Path.Combine(Path.GetDirectoryName(typeof(SetupParametersCommand).Assembly.Location)!, "CctvSharedParameters.txt")).
///  2. var file = app.OpenSharedParameterFile(); var group = file.Groups.get_Item("CCTV");
///  3. Build a CategorySet from CameraReader.CameraCategories via doc.Settings.Categories.get_Item(bic).
///  4. In ONE Transaction("CCTV: setup parameters"): for each ExternalDefinition in group.Definitions:
///       var binding = app.Create.NewInstanceBinding(categorySet);
///       if (!doc.ParameterBindings.Insert(def, binding, GroupTypeId.Data))
///           doc.ParameterBindings.ReInsert(def, binding, GroupTypeId.Data);
///  5. ALWAYS restore app.SharedParametersFilename in a finally block (don't trash the user's setting).
///  6. TaskDialog summarising what was bound.
/// </summary>
[Transaction(TransactionMode.Manual)]
public sealed class SetupParametersCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("CCTV Coverage", "Not implemented yet — milestone M3 in docs/PLAN.md.");
        return Result.Cancelled;
    }
}
