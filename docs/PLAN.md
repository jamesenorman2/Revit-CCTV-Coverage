# Build Plan

This is the plan for finishing the add-in. Work through it **in order, one milestone at a time**. Each milestone lists its files, its steps, and a checklist for when it's **done**. Don't start a milestone until the previous one's checklist passes.

Read `CLAUDE.md` first. It has the working rules.

---

## 0. What we are building

These are the user-visible features, in priority order:

1. **Generate Coverage** (the core feature). In a floor plan view, every camera on that level gets coloured bands:
   - red for Identify
   - orange for Recognise
   - yellow for Observe
   - green for Detect

   The bands stop at walls, columns, closed doors and glazing, including those in linked models. Running it again replaces the previous output.
2. **Setup Parameters**. Adds the `CCTV_*` parameters to the project, so any camera family can be used.
3. **Camera Views**. Creates a perspective 3D view from each camera that roughly matches its lens.
4. **Coverage Report**. A CSV of area per camera per DORI level, and whether the camera meets its required level.

### Non-goals (do not build)

- Fisheye or panoramic cameras (HFOV of 180° or more).
- PTZ patrol paths.
- Lighting or IR range.
- Motion blur.
- True 3D partial occlusion. A low wall either blocks completely at target height or doesn't block at all.
- Sloped floors.
- Any cloud or network feature. The add-in must never make network calls (see `DEPLOYMENT.md`).

---

## 1. Architecture

```
┌──────────────────────── CctvCoverage.Revit (net8.0-windows) ────────────────────────┐
│ App.cs  ── ribbon "CCTV" tab                                                           │
│ Commands/*Command.cs  ── orchestration only, no maths                                  │
│ Adapters/                                                                              │
│   CameraReader        FamilyInstance  ──►  CameraSpec         (ft → m here)            │
│   RaycastViewProvider owns the "CCTV Raycast" 3D view                                  │
│   RevitRayCaster      IRayCaster via ReferenceIntersector     (ft ↔ m here)            │
│   FilledRegionWriter  Polygon2  ──►  FilledRegion             (m → ft here)            │
│   Units               the ONLY place units are converted                               │
└───────────────────────────────────────────┬──────────────────────────────────────────┘
                                            │ project reference
┌───────────────────────── CctvCoverage.Core (net8.0, no Revit) ───────────────────────┐
│ Standards/DoriLevel      EN 62676-4 px/m table                                         │
│ Optics/PixelDensity      PPM ↔ distance                                                │
│ Optics/VerticalFootprint blind zone + far edge from tilt/VFOV                          │
│ Visibility/IRayCaster    the seam;  SegmentRayCaster = test/2D implementation          │
│ Coverage/CoverageBuilder camera + ray caster ──► DORI band polygons                    │
└──────────────────────────────────────────────────────────────────────────────────────┘
```

### Rules

- **Core never references Revit.** All the maths is tested without Revit, because Revit can't run in CI.
- Core uses **metres and radians**. Revit uses **decimal feet and radians**. Convert only in `Adapters/Units.cs`.
- Plan angles are measured **anticlockwise from +X** (east in internal coordinates).
- Commands only orchestrate. If a command has an `if` that does geometry, move that code into Core.

---

## 2. The algorithm (reference)

For a camera with lens height `H` above its level, target height `T` (default 1.6 m), drop `h = H − T`, horizontal resolution `px`, horizontal FOV `hfov`, vertical FOV `vfov`, pan `p` and tilt `t`:

1. **Pixel density.** At slant distance `s`, `PPM = px / (2·s·tan(hfov/2))`. The maximum slant distance for level L is therefore `s_L = px / (2·tan(hfov/2)·PPM_L)`. Convert it to a plan distance with `r_L = sqrt(s_L² − h²)`, or 0 if `s_L ≤ h`.

   These are the EN 62676-4 values, in pixels per metre:

   | Level | px/m |
   |---|---|
   | Monitor | 12.5 |
   | Detect | 25 |
   | Observe | 62.5 |
   | Recognise | 125 |
   | Identify | 250 |
   | Inspect | 1000 |

2. **Vertical footprint.** The bottom image edge meets the target plane at `Near = h / tan(t + vfov/2)`. Inside that radius is the blind zone. The top edge meets it at `Far = h / tan(t − vfov/2)`, or never if `t ≤ vfov/2`.
3. **Bands.** Bands are nested annuli, most demanding first:
   - Identify runs from `Near` to `r_Identify`.
   - Recognise runs from `r_Identify` to `r_Recognise`.
   - Observe and Detect continue the same way.

   Every outer radius is capped at `Far` and at `settings.MaxRange`.
4. **Occlusion.** Cast about `hfov / 0.5°` horizontal rays from the camera at target height. The ray caster returns `vis(θ)`, the distance to the first obstacle.
5. **Polygons.** For each band and azimuth, the lit interval is `[inner, min(outer, vis(θ))]`. Each consecutive run of lit azimuths becomes one polygon: the outer arc forwards, then the inner arc backwards.

`CoverageBuilder.cs` has the exact step-by-step version in its doc comment.

---

## 3. Milestones

### M0: Verify the scaffold builds

1. Run `dotnet test tests/CctvCoverage.Core.Tests`. Every test that isn't skipped must pass.
2. Run `dotnet build src/CctvCoverage.Revit`. It must compile. On Linux this works because of `EnableWindowsTargeting`; if it doesn't, rely on the Windows CI job.
3. Fix any compile errors in the scaffold. **Don't change test expectations.**

**Done when:** CI is green on both jobs.

### M1: Core geometry primitives

| File | Implement |
|---|---|
| `Optics/VerticalFootprint.cs` | `Compute` |
| `Visibility/SegmentRayCaster.cs` | `Cast` |
| `Geometry/Polygon2.cs` | `Simplify` |

The algorithms are in each method's doc comment. Remove `Skip = M1` from `M1_GeometryTests.cs`.

**Done when:**
- [ ] All M1 tests pass, unmodified apart from removing `Skip`.
- [ ] No new public API beyond what's stubbed.

### M2: CoverageBuilder

| File | Implement |
|---|---|
| `Coverage/CoverageBuilder.cs` | `Build`, following the numbered steps in its doc comment. Private helper methods are fine. |

Remove `Skip = M2` in `M2_CoverageBuilderTests.cs`.

**Done when:**
- [ ] All M2 tests pass.
- [ ] One extra test of your own exists, for a camera with `RequiredLevel` set and `HFOV = 60°` and `pan = 225°`. Assert that every vertex lies in the south-west quadrant relative to the camera.

### M3: Setup Parameters command

| File | Implement |
|---|---|
| `Commands/SetupParametersCommand.cs` | Follow its doc comment. |

**Manual test (the human does this in Revit 2026):**
- [ ] In a new project, click **CCTV → Setup Parameters**.
- [ ] Place any Security Devices family. Its Properties palette shows a "Data" group containing all 11 `CCTV_*` parameters.
- [ ] Run the command again. No error, and no duplicate parameters.
- [ ] The user's original shared parameter file path is unchanged (check in Manage → Shared Parameters).

### M4: Generate Coverage (the main feature)

Build this in four sub-steps and compile after each one.

**M4a.** Implement `RaycastViewProvider.GetOrCreate`, then `CameraReader.ReadCamerasOnLevel`. Use the per-parameter rules in its doc comment exactly.

**M4b.** Implement `RevitRayCaster.Cast`, including the "camera mounted on a wall" gotcha. Use `Find` and skip hits closer than 0.05 m.

**M4c.** Implement `FilledRegionWriter`:
- `GetOrCreateRegionType`
- `DeleteExisting`
- `Write`

**M4d.** Wire it all together in `GenerateCoverageCommand`, following its doc comment.

Before the manual test, build a test model named `CCTV-Test.rvt` (the human builds it, or you script it):
- Level 1 contains a 20 × 10 m room of 3 m high walls.
- One interior wall at x = 10 has a 1 m door that has been **deleted**, leaving a doorway.
- One 400 mm column.
- A second building, in a **linked** RVT, with one wall placed 5 m outside the room.
- Camera A sits in a corner at 3 m, 1920 × 1080, HFOV 90°, tilt 15°, facing into the room.
- Camera B is on the opposite wall, facing the doorway.

**Manual test:**
- [ ] Bands are coloured red, orange, yellow and green from the camera outwards, with a gap under the camera.
- [ ] Nothing is drawn through solid walls. Coverage passes through the doorway.
- [ ] The column casts a visible "shadow" with no coverage.
- [ ] The wall in the linked model blocks coverage.
- [ ] Running the command again replaces the regions. There are no duplicates (count them in Select All Instances).
- [ ] A camera with a missing `CCTV_HFOV` produces a warning line, and the other cameras still draw.
- [ ] Hand check: with Camera A at h = 1.4 m, `r_Identify` is about 3.58 m. Compute it with the formulas in section 2, then use Measure to check it in Revit.
- [ ] Runtime for 50 cameras is under 30 s. Record the actual number in **Findings**.

### M5: Camera Views

| File | Implement |
|---|---|
| `Commands/CreateCameraViewsCommand.cs` | See the steps below. |

1. Get a perspective `ViewFamilyType` (`ViewFamily.ThreeDimensional`). Create the view with `View3D.CreatePerspective(doc, vftId)`.
2. Set the eye to the lens position, in feet, and the direction from pan `p` and tilt `t`:
   - `forward = (cos p · cos t, sin p · cos t, −sin t)`
   - `up = (cos p · sin t, sin p · sin t, cos t)`
3. Apply them with `view.SetOrientation(new ViewOrientation3D(eye, up, forward))`.
4. Name the view `CCTV - {Mark}`. If a view with that name exists, update its orientation instead of creating a new one.
5. **Field of view.** Revit sets a perspective's field of view from the crop region size relative to the eye-to-target distance. The API controls for this aren't well documented. Do this:
   1. Set `CropBoxActive = true`.
   2. Set the crop box aspect ratio to `px : py`.
   3. **Calibrate empirically.** In the test model, put a 10 m wide wall exactly `5 / tan(hfov/2)` m in front of a 90° camera, so it should exactly fill the frame width. Adjust the crop box until it does.
   4. Write the formula you found under **Findings**.

**Done when:**
- [ ] The calibration wall fills the frame width to within ±5%, for both a 90° and a 50° HFOV.

### M6: Settings and Coverage Report

1. Add a `CoverageSettings` editor: a simple WinForms or TaskDialog chain, or `UseWPF`. It covers:
   - target height
   - max range
   - which levels to draw
   - whether doors and curtain panels block view
2. Store the settings in the project with Extensible Storage on `ProjectInformation`, so every user of the model gets the same results. Wrap storage in a single `SettingsStore` class.
3. Implement `ExportReportCommand` as its doc comment describes.

**Done when:**
- [ ] Changing the target height and re-running Generate visibly changes the bands.
- [ ] Settings survive a save and reopen.
- [ ] The CSV opens in Excel with correct numbers for Camera A (cross-check one value by hand).

### M7: Revit 2027 and other versions

Revit 2027 is on **.NET 10**. Revit 2025 and 2026 are on **.NET 8**.

1. Multi-target the Revit project:
   - `TargetFrameworks` becomes `net8.0-windows;net10.0-windows`.
   - Add a `RevitVersion` property set per framework.
   - Use conditional `PackageReference` versions: `2026.*` for net8 and `2027.*` for net10.
2. Core can stay on `net8.0` (net10 can reference it), or multi-target as well.
3. Produce one output folder per Revit year.
4. Revit 2024 and earlier use .NET Framework 4.8. **Only support them if the user asks.** It needs `net48` plus C# language-level care.

### M8: Packaging and signing

Follow `docs/DEPLOYMENT.md`:
1. Write an install script or MSI.
2. Add a signing step to CI using a secret. It is skipped when the secret is absent.
3. Write the IT deployment notes.

---

## 4. Findings

Append to this section as you go. Record what you discovered about Revit behaviour, plus timings and calibrations, so the next person doesn't have to rediscover them.

- *(empty)*
