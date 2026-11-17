using System.Reflection;
using Autodesk.Revit.UI;

namespace CctvCoverage.Revit;

/// <summary>Creates the "CCTV" ribbon tab. Each button maps to one command class.</summary>
public sealed class App : IExternalApplication
{
    private const string TabName = "CCTV";

    public Result OnStartup(UIControlledApplication application)
    {
        application.CreateRibbonTab(TabName);
        var panel = application.CreateRibbonPanel(TabName, "Coverage");
        var dll = Assembly.GetExecutingAssembly().Location;

        AddButton(panel, dll, "SetupParameters", "Setup\nParameters", typeof(Commands.SetupParametersCommand),
            "Bind the CCTV_* shared parameters to camera categories in this project.");
        AddButton(panel, dll, "GenerateCoverage", "Generate\nCoverage", typeof(Commands.GenerateCoverageCommand),
            "Draw DORI coverage bands (EN 62676-4), clipped by walls, for cameras on the active plan view's level.");
        AddButton(panel, dll, "CameraViews", "Camera\nViews", typeof(Commands.CreateCameraViewsCommand),
            "Create a perspective 3D view from each selected camera, matched to its lens.");
        AddButton(panel, dll, "Report", "Coverage\nReport", typeof(Commands.ExportReportCommand),
            "Export a CSV of coverage area per camera and DORI level.");

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application) => Result.Succeeded;

    private static void AddButton(RibbonPanel panel, string dll, string name, string text, Type command, string tooltip)
    {
        var data = new PushButtonData(name, text, dll, command.FullName) { ToolTip = tooltip };
        panel.AddItem(data);
    }
}
