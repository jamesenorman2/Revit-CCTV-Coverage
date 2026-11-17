# Deployment, Signing and Corporate Networks

This is a short answer to "will IT or Revit block this?". Read it before giving the add-in to anyone else.

## 1. How Revit loads add-ins

Revit reads every `*.addin` manifest in these folders at startup:

| Scope | Folder | Needs admin? |
|---|---|---|
| One user | `%AppData%\Autodesk\Revit\Addins\2026\` | No |
| All users | `%ProgramData%\Autodesk\Revit\Addins\2026\` | Yes |

The manifest points at `CctvCoverage.Revit.dll`. Each Revit year has its own folder, so a 2026 build is not loaded by 2027 (see PLAN M7).

## 2. Code signing and the "Security – Unsigned Add-in" dialog

- **Unsigned:** Revit shows a "Security – Unsigned Add-in" prompt at startup, with **Always Load / Load Once / Do Not Load**. That's acceptable for development and a small team. Many IT departments forbid it, and users learn to click through warnings.
- **Signed with a trusted Authenticode certificate:** Revit shows the publisher name instead. "Always Load" then trusts that **publisher**, so future versions load without prompting.
- **The certificate.** You need one of these:
  - An **OV code-signing certificate** from a public CA. Since 2023, the private keys have to live on a hardware token or a cloud HSM, which complicates CI.
  - **Azure Trusted Signing**, which is usually the cheapest CI-friendly option.
  - Your company's **internal enterprise CA**. This works if IT already trusts it on every machine, and it's often the easiest route inside one firm.
- **What to sign:** sign **both** DLLs (`CctvCoverage.Revit.dll` and `CctvCoverage.Core.dll`), and the installer if you build one. Use a timestamp server, so signatures stay valid after the certificate expires:
  ```bash
  signtool sign /fd SHA256 /tr http://timestamp.digicert.com /td SHA256 /a CctvCoverage.Revit.dll CctvCoverage.Core.dll
  ```
- **Autodesk App Store:** publishing there requires signing.

## 3. Corporate network and IT issues to expect

| Issue | Symptom | Fix |
|---|---|---|
| **Application control** (AppLocker, WDAC, ThreatLocker and similar) | The DLL silently fails to load, or Revit reports a load error. This is common when the DLL runs from `%AppData%`, which is user-writable. | Ask IT to allow the **publisher certificate** (needs signing) or a file hash, and to deploy to `%ProgramData%`. |
| **No local admin** | Can't write to `%ProgramData%`. | Use the per-user `%AppData%` folder, or have IT deploy it with Intune or SCCM. |
| **Mark of the Web** on a downloaded ZIP | Files are flagged as "from the internet". SmartScreen blocks an installer. | Right-click → Properties → Unblock, or run `Unblock-File`, **before** extracting. Signing reduces SmartScreen friction. |
| **Antivirus / EDR heuristics** | A new unsigned DLL is quarantined. | Signing plus an IT allow-list. |
| **Add-in loaded from a network share** (`\\server\addins`) | Slow startup. The DLL gets locked while anyone has Revit open, so updates fail. | Copy it locally at logon (an Intune or login script) instead of pointing the manifest at UNC paths. |
| **Assembly version clashes** with other add-ins | Rare for us, because we have **no third-party runtime dependencies**. | Keep it that way. Every NuGet package added to the Revit project is a new clash risk. Revit 2026 offers opt-in add-in isolation, but verify the manifest syntax before relying on it. |
| **Security review / data exfiltration questions** | IT asks what the add-in does. | The add-in makes **no network calls**, writes only to the open model (and the CSV the user asks for), and has no telemetry. Keep it that way, and say so in the IT note. |

## 4. Recommended path

1. **Now (a few users):** use an unsigned per-user install. Users click "Always Load" once.
2. **Firm-wide:**
   1. Get the DLLs signed, with an internal CA or Azure Trusted Signing.
   2. Build an MSI (WiX) or Intune package that installs to `%ProgramData%\Autodesk\Revit\Addins\<year>\`.
   3. Give IT a one-page note covering the publisher, the install paths, "no network access, no telemetry", and the SHA-256 hashes.
3. **CI:** a sign step runs only when signing secrets exist (PLAN M8), so forks and PRs still build unsigned.
