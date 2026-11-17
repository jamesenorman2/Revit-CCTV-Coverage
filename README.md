# Revit CCTV Coverage

A Revit add-in that draws CCTV camera coverage on floor plans. The coverage is:

- **clipped by walls, columns, doors and glazing**, including those in linked models, and
- **banded by BS EN IEC 62676-4 observation level**: Identify, Recognise, Observe and Detect, based on pixels per metre at the target.

It can also create a matched **perspective "camera view"** for each camera, and export a CSV coverage report.

> Status: **scaffold**. The build plan is in [`docs/PLAN.md`](docs/PLAN.md).

## How it works (one paragraph)

Each camera is a family instance with `CCTV_*` parameters (resolution, HFOV, tilt and so on). For each camera, the add-in fires a fan of horizontal rays at target height (default 1.6 m) through the camera's field of view, using Revit's `ReferenceIntersector`, to find how far the camera can see in each direction. It also works out the plan distance at which each DORI pixel-density threshold is reached, allowing for mounting height and the blind zone under the camera. Intersecting the two gives coloured bands, which are drawn as filled regions in the plan view.

## Layout

| Path | What |
|---|---|
| `src/CctvCoverage.Core` | Pure maths. No Revit references; unit tested on any OS. |
| `src/CctvCoverage.Revit` | The Revit 2026 add-in: ribbon, commands and Revit adapters. |
| `tests/CctvCoverage.Core.Tests` | xUnit tests. These are the specification for Core. |
| `revit/SharedParameters` | Shared parameter file for camera families. |
| `docs/PLAN.md` | Milestone-by-milestone build plan. |
| `docs/DEPLOYMENT.md` | Installing, code signing and corporate IT notes. |

## Build

```bash
dotnet test tests/CctvCoverage.Core.Tests
```

```bash
dotnet build src/CctvCoverage.Revit -c Release
```

## Install (development)

Copy `CctvCoverage.addin` to `%AppData%\Autodesk\Revit\Addins\2026\`. Then copy `CctvCoverage.Revit.dll`, `CctvCoverage.Core.dll` and `CctvSharedParameters.txt` into a `CctvCoverage\` subfolder next to it. See `docs/DEPLOYMENT.md` before rolling this out to other people.
