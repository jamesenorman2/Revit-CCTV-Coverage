using Autodesk.Revit.DB;

namespace CctvCoverage.Revit.Adapters;

/// <summary>
/// MILESTONE M4. ReferenceIntersector needs a non-template 3D view; it only "sees" elements
/// visible in that view. We own a dedicated view so users' views never affect results and
/// users can deliberately hide things (e.g. glazing) in it.
/// </summary>
public static class RaycastViewProvider
{
    public const string ViewName = "CCTV Raycast";

    /// <summary>
    ///  - Look for an existing View3D named ViewName (not IsTemplate, not IsPerspective). Return it if found.
    ///  - Otherwise, in its own Transaction("CCTV: create raycast view"):
    ///      var vft = FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType)).Cast&lt;ViewFamilyType&gt;()
    ///                .First(t =&gt; t.ViewFamily == ViewFamily.ThreeDimensional);
    ///      var v = View3D.CreateIsometric(doc, vft.Id); v.Name = ViewName; v.IsSectionBoxActive = false;
    ///      v.DetailLevel = ViewDetailLevel.Fine;
    ///  - Linked models MUST be visible in this view (they are by default) — architectural walls are usually in a link.
    /// </summary>
    public static View3D GetOrCreate(Document doc)
    {
        throw new NotImplementedException("M4: see docs/PLAN.md");
    }
}
