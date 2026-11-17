# Instructions for the coding agent

You are building this add-in by following `docs/PLAN.md`. These rules matter more than speed.

## Workflow

1. Work on **one milestone at a time**, in order (M0 → M8). Read the whole milestone section, plus the doc comment of every stub you're about to implement, **before writing code**.
2. Every stub contains `throw new NotImplementedException("Mx: …")`. Its XML doc comment is the specification: follow it literally. If the spec looks wrong, **stop**, explain why in your reply, and add a note under "Findings" in PLAN.md. Don't silently deviate.
3. After each milestone:
   - Run `dotnet test tests/CctvCoverage.Core.Tests`. It must be green, with no remaining `Skip` for that milestone.
   - Run `dotnet build src/CctvCoverage.Revit`. It must compile.
   - Commit with the message `Mx: <summary>`.
4. You **can't run Revit**. For milestones M3–M6, finish when the code compiles. Then list the manual test checklist from PLAN.md for the human, and **don't claim it passed**.

## Hard rules

- **Never change a test's expected values to make it pass.** Fix the code. If you're sure a test is wrong, explain the maths in your reply and leave the test alone.
- `CctvCoverage.Core` must **never** reference `Autodesk.*`.
- Convert units (feet ↔ metres) **only** in `src/CctvCoverage.Revit/Adapters/Units.cs`.
- Every Revit model change goes inside a named `Transaction("CCTV: …")`. Use one transaction per user action where you can.
- One bad camera or polygon must never abort a whole run. Catch the error per item, add a warning, and continue.
- Don't add NuGet packages to the Revit project (see `docs/DEPLOYMENT.md`, "Assembly version clashes"). Test-only packages are fine.
- No network calls, no telemetry, no file writes except the CSV the user asks for.
- Keep public APIs as stubbed. Adding private helpers is fine.

## Commands

```bash
dotnet test tests/CctvCoverage.Core.Tests
```

```bash
dotnet build src/CctvCoverage.Revit
```

## Revit API tips

- `ReferenceIntersector` needs a **3D view**, and only hits elements visible in that view. That's why we own the "CCTV Raycast" view.
- `FindReferencesInRevitLinks = true` is required. Walls usually live in a linked architectural model.
- `ReferenceWithContext.Proximity` is in **feet**, measured from the ray origin.
- `FilledRegion.Create` needs closed `CurveLoop`s with no segment shorter than about 0.8 mm. That's why `Polygon2.Simplify()` exists.
- `FamilyInstance.FacingOrientation` is a unit `XYZ` in model coordinates.
