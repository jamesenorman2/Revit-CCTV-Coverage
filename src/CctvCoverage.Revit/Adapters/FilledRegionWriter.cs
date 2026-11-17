using Autodesk.Revit.DB;
using CctvCoverage.Core.Coverage;
using CctvCoverage.Core.Standards;

namespace CctvCoverage.Revit.Adapters;

/// <summary>
/// MILESTONE M4. Draws coverage as FilledRegions (view-specific 2D detail elements) in a plan view.
/// Must be called inside an open Transaction.
/// </summary>
public static class FilledRegionWriter
{
    /// <summary>Marker written into each region's Comments so we can find and replace our own output.</summary>
    public const string CommentPrefix = "CCTV-COVERAGE";

    /// <summary>Default band colours (R, G, B). Same scheme as most DORI charts.</summary>
    public static (byte R, byte G, byte B) ColourFor(DoriLevel level) => level switch
    {
        DoriLevel.Inspect => (128, 0, 128),
        DoriLevel.Identify => (230, 0, 0),
        DoriLevel.Recognise => (255, 140, 0),
        DoriLevel.Observe => (255, 220, 0),
        DoriLevel.Detect => (0, 170, 0),
        DoriLevel.Monitor => (0, 120, 255),
        _ => (128, 128, 128),
    };

    /// <summary>
    /// Delete every FilledRegion in <paramref name="view"/> whose ALL_MODEL_INSTANCE_COMMENTS starts with CommentPrefix.
    /// Use FilteredElementCollector(doc, view.Id).OfClass(typeof(FilledRegion)).
    /// </summary>
    public static void DeleteExisting(Document doc, View view)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }

    /// <summary>
    /// For each result, for each band, for each polygon:
    ///  1. typeId = GetOrCreateRegionType(doc, band.Level).
    ///  2. Build a CurveLoop from Line.CreateBound(a, b) for consecutive vertices (including last→first),
    ///     with Units.ToXyz(v, z). Use z = view.GenLevel.Elevation. If Revit throws "curves must be in the
    ///     plane of the view", retry with z = 0 and record which works in docs/PLAN.md "Findings".
    ///  3. var region = FilledRegion.Create(doc, typeId, view.Id, new List&lt;CurveLoop&gt; { loop });
    ///  4. region.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
    ///         .Set($"{CommentPrefix}|{result.Camera.Id}|{band.Level}");
    ///  5. Draw less demanding bands FIRST so Identify ends up on top (iterate bands in reverse).
    ///  6. Catch Autodesk.Revit.Exceptions.ArgumentException per polygon → skip it and add a warning; never abort.
    /// </summary>
    public static void Write(Document doc, ViewPlan view, IReadOnlyList<CoverageResult> results, List<string> warnings)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }

    /// <summary>
    /// Find FilledRegionType named $"CCTV - {level}"; if missing, duplicate any existing FilledRegionType:
    ///   var t = (FilledRegionType)existing.Duplicate($"CCTV - {level}");
    ///   t.ForegroundPatternId = solid fill: FilteredElementCollector(doc).OfClass(typeof(FillPatternElement))
    ///        .Cast&lt;FillPatternElement&gt;().First(f =&gt; f.GetFillPattern().IsSolidFill).Id;
    ///   t.ForegroundPatternColor = new Color(r, g, b);
    ///   t.IsMasking = false;   // so walls/underlay still show through the edges
    /// Transparency: FilledRegionType has no transparency. Users set the view's
    /// "Detail Items" category halftone, or put the plan view on an underlay — document this in the README.
    /// </summary>
    public static ElementId GetOrCreateRegionType(Document doc, DoriLevel level)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }
}
