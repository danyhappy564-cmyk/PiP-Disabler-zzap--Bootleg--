<p align="center">
  <a href="README.md"><img alt="한국어" src="https://img.shields.io/badge/%ED%95%9C%EA%B5%AD%EC%96%B4-%ED%95%9C%EA%B5%AD%EC%96%B4%EB%A1%9C%20%EB%B3%B4%EA%B8%B0-6e7681?style=for-the-badge"></a>
  <a href="README.en.md"><img alt="English (this page)" src="https://img.shields.io/badge/English-This%20page-1f6feb?style=for-the-badge"></a>
</p>

# PiP-Disabler-zzap--Bootleg- (SPT 4.1 port)

---

### ⚠️ IMPORTANT NOTICE / DISCLAIMER

- **Original Author:** Fiodorwellfme (Fiodor)
- **Original Repository:** PiP-Disabler
- **Original Link:** https://github.com/Fiodorwellfme/PiP-Disabler
- **License:** MIT License — Copyright (c) 2026 Fiodorwellfme (see `LICENSE`)

1. **Reflection & Take-Downs:** I deeply reflect on the ECOT incident. As an AI-assisted "vibe coder," I will immediately delete files if the original authors ask.
2. **No Re-Distribution:** These ported builds are unverified, temporary fixes. Please do NOT re-upload or share them anywhere else.
3. **Do Not Pester Original Authors:** Never report bugs or pester original modders regarding issues from my unofficial ports.
4. **Full Credit & Respect:** I will always credit original creators on GitHub and prioritize their decisions above all else.
5. **Support Original Creators:** Instead of using my ports, please visit the original authors' Forge pages to leave kind words or tips.

---

## Changelog

The full changelog (72 entries) is in the [`changelog/`](changelog/) folder, 10 per file (higher number = newer, latest: [`CHANGELOG-008.md`](changelog/CHANGELOG-008.md)). Per-release summaries: [Releases](../../releases).

## What this mod does

When aiming through a magnified scope, the original game renders the scene **twice**: the normal view and a magnified view inside the lens (PiP). This mod disables the lens view and uses the following approach to reduce the FPS drop while aiming.

- Magnifies the main camera itself (reduces FOV)
- Cuts away parts of the scope body model that obstruct your view in real time
- Draws the reticle separately in the center of the screen

The following scopes are automatically excluded (they use the game's original rendering).

- Night vision and thermal scopes (`Auto Disable For NV/Thermals`)
- Scopes with a rangefinder
- Scopes with names listed in `Auto Bypass Name Contains` (default: `d-evo; scope_ags_npz_pag17_2,7x`)
- Blacklisted scopes, or scopes absent from the whitelist when whitelist mode is enabled

## Adjusting individual scopes (for example, when the gun looks too large at 20×)

1. Open F12 (BepInEx Configuration Manager) **while aiming through that scope** → check its name/status under **지금 조준 중인 스코프 (Currently aimed scope)** at the top of `3. 스코프별 설정` (Per-scope settings).
   - If it says "원래 방식(PiP)으로 보는 중" (Using original PiP rendering), this mod is bypassed for that scope and the settings below have no effect.
2. Adjust values while aiming to see changes **immediately, with automatic saving**:
   - The scope body blocks the screen at high magnification → increase **총 크기 - 가장 높은 배율 (Gun size - highest magnification)** (default 1).
   - The inside of the scope body (cylinder) is visible in the lens → increase **몸통 구멍 - 깊이 (Body cutout - depth)**.
   - The reticle is too large or small → adjust **조준선 크기 (Reticle size)**.
   - You want more or less zoom → adjust **확대 배수 (Zoom multiplier)** (1 = original, 2 = double). More zoom makes the body look larger too, so increase gun size as well.
   - For finer controls (cutout circle position/size, etc.), enable "고급 설정(Advanced settings)" at the top right of F12.
3. To reset, bind `이 스코프 설정 초기화 키` (Reset this scope's settings key), then press it while aiming.
4. Your values are saved in `BepInEx\config\PiP-Disabler.scope-settings.json`. They survive mod updates; backing up this file is enough.

## Installation

1. Place the `PiP-Disabler` folder in `E:\SPT 4.1\BepInEx\plugins\`. It should contain:
   - `PiP-Disabler.dll`
   - `custom_mesh_surgery_settings.json` (per-scope settings)
   - `pipdisabler_reticle_shaders.bundle`, `pipdisabler_effect_shaders.bundle`
2. If you have Sierra's port or an older version, remove its DLL (the same mod must not load twice).
3. In-game, configure it under F12 → `PiP-Disabler`.

**Using it together with Fontaine's FOV Fix is not recommended** (the original author's guidance). Instead, enable `General → FOV Fix Behaviour` to match 1× FOV to the game's configured FOV.

## Build it yourself (on your PC)

```
dotnet build -c Release
```

- The default SPT path is `E:\SPT 4.1`. If yours differs, specify it with `dotnet build -c Release -p:SPTPath="D:\내 SPT"`.
- Building copies files directly to `E:\SPT 4.1\BepInEx\plugins\PiP-Disabler\` and creates `Releases\PiP-Disabler.zip`.
- `custom_mesh_surgery_settings.json` is copied **only if it does not already exist** at the destination, to avoid overwriting per-scope settings saved in-game.

## How to report bugs

1. F12 → `PiP-Disabler` → `Debug` → enable `Debug logging`
2. Reproduce the issue in a raid with the affected scope
3. Send `E:\SPT 4.1\BepInEx\LogOutput.log` and the **scope name**.
   - `Bypassing mod for current scope (...)` in the log means the mod intentionally bypassed that scope. The reason is in parentheses.
   - `Thermal/NV auto-bypass match`, `AutoBypassNameContains match`, `Whitelist bypass`, and `Rangefinder bypass` mean the same thing.
   - `[Patcher] Failed to enable ...` and `Scope enter failed` indicate errors; be sure to include them.
4. Turn `Debug logging` off after reporting. Leaving it on produces a large volume of logs.

> Please do not contact the original author about this port (Disclaimer item 3 above).

## Known limitations (same as the original)

- Variable-magnification scopes have limited support.
- Magnification is read from item data (`Zooms`), so it may differ slightly from the original game's lens view.
- Each scope may require body cutting/reticle size adjustments (`custom_mesh_surgery_settings.json`, or adjust the Mesh Surgery settings in F12 and use the save key).

## Original description (English, preserved verbatim)

<details>
<summary>Expand</summary>

Runtime meshcutting combined with FOV zoom (incompatible with Fontaine's FOV fix) to reduce the FPS cost of ADSing with optic scopes.

When ADS, identify if scope is optic or not. If Optic then check if thermal or NVG or variable scope or part of blacklisted scopes. If any of those is true then bypass mod.
Extract reticle, flip the texture horizontally and display it center screen in a command buffer at AfterForwardAlpha to allow compatibility with shaders such as in Borkel's NVG mod.

Set a 4 diameter cutting plane along the scope axis using the lens closest to the camera as the origin (this has to be configured manually for each scope).
The meshcutter cuts through all the meshes that are found between hands and the scope and the resulting meshes are then cached for reusal until end of raid or change of attachment on the weapon.

Once the mesh has been cut, use it to create a mask that hides the reticle when it intersects with scope housing/Weapon mesh.
Set main camera rotation axis aligned with weapon axis to keep aiming consistent and reduce sway.

Magnification is searched for in Template.Zooms to be consistent across scopes, sometimes it may not be the same as the PiP scopes because of BSG Jank.
Calculate FOV by dividing reference FOV (50°) by magnification and then set main cam FOV to that value.

Scaling is done using the ribcage scaling method with some dark magic multipliers and offsets.
When exiting ADS, the lens is made solid black to prevent reticle from flashing.

</details>

## FPS comparison (original screenshots)

![PiP](https://github.com/Fiodorwellfme/PiP-Disabler/blob/main/images/Screenshot%202026-03-07%20173054.png "Picture in Picture Scope")
![PiP-Disabler](https://github.com/Fiodorwellfme/PiP-Disabler/blob/main/images/Screenshot%202026-03-07%20173102.png "PiP-Disabler")
