using BepInEx.Bootstrap;
using EFT;
using EFT.CameraControl;
using UnityEngine;

namespace PiPDisabler.Patches
{
    /// <summary>
    /// 7Bpencil's MagicOpticMount (MIT) renders a thermal/NV device mounted in front of a normal
    /// optic by switching the vanilla PiP optic camera to that device's ScopeData effects
    /// (OpticComponentUpdater.CopyComponentFromOptic postfix). PiP-Disabler replaces that camera,
    /// so the device's image was lost. When such an aligned device is on the weapon, the current
    /// optic is bypassed so vanilla PiP (and MagicOpticMount) renders it.
    ///
    /// The alignment test mirrors MagicOpticMount's own AreSightsAligned.
    ///
    /// Default (MagicOpticMountWithoutPiP): instead of bypassing, the device's thermal/NV settings
    /// are put on the main camera's own ThermalVision/NightVision components (the ones goggles
    /// use) while scoped, and restored afterwards — the device image without the second render.
    /// </summary>
    internal static class MagicOpticMountCompat
    {
        private const string MagicOpticMountGuid = "7Bpencil.MagicOpticMount";
        private const float MaxAngle = 45f;
        private const float MaxDistance = 0.006f; // 6 mm

        private static bool? _installed;

        private static bool Installed
        {
            get
            {
                if (!_installed.HasValue)
                    _installed = Chainloader.PluginInfos.ContainsKey(MagicOpticMountGuid);
                return _installed.Value;
            }
        }

        public static bool ShouldBypass(OpticSight os)
        {
            if (Settings.MagicOpticMountWithoutPiP.Value) return false;
            return FindAlignedDevice(os) != null;
        }

        /// <summary>The aligned thermal/NV device OpticSight in front of <paramref name="os"/>, or null.</summary>
        private static OpticSight FindAlignedDevice(OpticSight os)
        {
            if (os == null || !Installed) return null;

            try
            {
                var player = Helpers.GetLocalPlayer();
                var pwa = player?.ProceduralWeaponAnimation;
                if (pwa == null || pwa.ScopeAimTransforms == null || pwa.ScopeAimTransforms.Count == 0)
                    return null;

                // The device itself is thermal/NV — nothing for MagicOpticMount to add.
                if (IsSpecialOptic(os)) return null;

                Transform opticBone = FindBoneFor(pwa, os);
                if (opticBone == null) return null;

                var firearmController = player.HandsController as Player.FirearmController;
                var weaponPrefab = firearmController?.Firearms?.WeaponPrefab;
                var weaponRoot = weaponPrefab != null ? weaponPrefab.Hierarchy?.GetTransform(ECharacterWeaponBones.weapon) : null;
                if (weaponRoot == null) return null;
                Vector3 weaponForward = -weaponRoot.up;

                foreach (var sight in pwa.ScopeAimTransforms)
                {
                    if (sight == null || !sight.IsOptic || sight.Bone == null || sight.Bone == opticBone) continue;
                    var cache = sight.ScopePrefabCache;
                    if (cache == null) continue;

                    var other = cache.CurrentModOpticSight;
                    if (other == null || !IsSpecialOptic(other)) continue;

                    if (AreSightsAligned(opticBone, sight.Bone, weaponForward))
                    {
                        PiPDisablerPlugin.DebugLogInfo(
                            $"[MagicOpticMountCompat] '{os.name}' has an aligned thermal/NV device '{other.name}' in front.");
                        return other;
                    }
                }
            }
            catch
            {
                // fall through: never block the mod on a failed probe
            }

            return null;
        }

        // ── Without-PiP mode: device effect on the main camera ──────────────

        private struct ThermalState
        {
            public bool Enabled, On, IsNoisy, IsFpsStuck, IsMotionBlurred, IsGlitch, IsPixelated;
            public ThermalVisionUtilities Utilities;
            public StuckFPSUtilities StuckFps;
            public MotionBlurUtilities MotionBlur;
            public GlitchUtilities Glitch;
            public PixelationUtilities Pixelation;
            public float ChromaticShift, UnsharpBias, UnsharpRadius;
        }

        private struct NightState
        {
            public bool Enabled, On;
            public float Intensity, NoiseIntensity, NoiseScale;
            public Color Color;
        }

        private static ThermalVision _appliedThermal;
        private static BSG.CameraEffects.NightVision _appliedNight;
        private static ThermalState _savedThermal;
        private static NightState _savedNight;

        /// <summary>Called on (non-bypassed) scope enter and after a mode switch.</summary>
        public static void OnScopeEnter(OpticSight os)
        {
            Restore();
            if (!Settings.MagicOpticMountWithoutPiP.Value) return;

            OpticSight device = FindAlignedDevice(os);
            ScopeData data = device != null ? device.ScopeData : null;
            if (data == null || !CameraManager.Exist || CameraManager.Instance == null) return;

            try
            {
                var thermalData = data.ThermalVisionData;
                var tv = CameraManager.Instance.ThermalVision;
                if (thermalData != null && thermalData.ThermalVision && tv != null)
                {
                    _savedThermal = new ThermalState
                    {
                        Enabled = tv.enabled, On = tv.On, IsNoisy = tv.IsNoisy, IsFpsStuck = tv.IsFpsStuck,
                        IsMotionBlurred = tv.IsMotionBlurred, IsGlitch = tv.IsGlitch, IsPixelated = tv.IsPixelated,
                        Utilities = tv.ThermalVisionUtilities, StuckFps = tv.StuckFpsUtilities,
                        MotionBlur = tv.MotionBlurUtilities, Glitch = tv.GlitchUtilities, Pixelation = tv.PixelationUtilities,
                        ChromaticShift = tv.ChromaticAberrationThermalShift, UnsharpBias = tv.UnsharpBias, UnsharpRadius = tv.UnsharpRadiusBlur
                    };

                    // Same assignments vanilla makes on the optic camera (CopyComponentFromOptic). On is
                    // set directly, not via Switch(), so the goggle tube mask is not switched on.
                    tv.IsGlitch = thermalData.ThermalVisionIsGlitch;
                    tv.IsPixelated = thermalData.ThermalVisionIsPixelated;
                    tv.IsNoisy = thermalData.ThermalVisionIsNoisy;
                    tv.IsMotionBlurred = thermalData.ThermalVisionIsMotionBlurred;
                    tv.IsFpsStuck = thermalData.ThermalVisionIsFpsStuck;
                    tv.ThermalVisionUtilities = thermalData.ThermalVisionUtilities;
                    tv.StuckFpsUtilities = thermalData.StuckFPSUtilities;
                    tv.MotionBlurUtilities = thermalData.MotionBlurUtilities;
                    tv.GlitchUtilities = thermalData.GlitchUtilities;
                    tv.PixelationUtilities = thermalData.PixelationUtilities;
                    tv.ChromaticAberrationThermalShift = thermalData.ChromaticAberrationThermalShift;
                    tv.UnsharpBias = thermalData.UnsharpBias;
                    tv.UnsharpRadiusBlur = thermalData.UnsharpRadiusBlur;
                    tv.enabled = true;
                    tv.On = true;
                    _appliedThermal = tv;
                    PiPDisablerPlugin.DebugLogInfo($"[MagicOpticMountCompat] Thermal from '{device.name}' applied to the main camera.");
                    return;
                }

                var nightData = data.NightVisionData;
                var nv = CameraManager.Instance.NightVision;
                if (nightData != null && nightData.NightVision && nv != null)
                {
                    _savedNight = new NightState
                    {
                        Enabled = nv.enabled, On = nv.On, Intensity = nv.Intensity,
                        NoiseIntensity = nv.NoiseIntensity, NoiseScale = nv.NoiseScale, Color = nv.Color
                    };
                    nv.Intensity = nightData.Intensity;
                    nv.NoiseIntensity = nightData.NoiseIntensity;
                    nv.NoiseScale = nightData.NoiseScale;
                    nv.Color = nightData.Color;
                    nv.enabled = true;
                    nv.On = true;
                    nv.ApplySettings();
                    _appliedNight = nv;
                    PiPDisablerPlugin.DebugLogInfo($"[MagicOpticMountCompat] Night vision from '{device.name}' applied to the main camera.");
                }
            }
            catch (System.Exception ex)
            {
                PiPDisablerPlugin.LogSource.LogError($"[MagicOpticMountCompat] Applying device effect failed: {ex.Message}");
                Restore();
            }
        }

        /// <summary>Called on scope exit, bypass, mode switch and mod shutdown.</summary>
        public static void Restore()
        {
            try
            {
                if (_appliedThermal != null)
                {
                    var tv = _appliedThermal;
                    var s = _savedThermal;
                    tv.IsNoisy = s.IsNoisy; tv.IsFpsStuck = s.IsFpsStuck; tv.IsMotionBlurred = s.IsMotionBlurred;
                    tv.IsGlitch = s.IsGlitch; tv.IsPixelated = s.IsPixelated;
                    tv.ThermalVisionUtilities = s.Utilities; tv.StuckFpsUtilities = s.StuckFps;
                    tv.MotionBlurUtilities = s.MotionBlur; tv.GlitchUtilities = s.Glitch; tv.PixelationUtilities = s.Pixelation;
                    tv.ChromaticAberrationThermalShift = s.ChromaticShift; tv.UnsharpBias = s.UnsharpBias; tv.UnsharpRadiusBlur = s.UnsharpRadius;
                    // Only undo On/enabled if nobody (e.g. thermal goggles) switched them meanwhile.
                    if (tv.On)
                    {
                        tv.On = s.On;
                        tv.enabled = s.Enabled;
                    }
                }

                if (_appliedNight != null)
                {
                    var nv = _appliedNight;
                    var s = _savedNight;
                    nv.Intensity = s.Intensity; nv.NoiseIntensity = s.NoiseIntensity;
                    nv.NoiseScale = s.NoiseScale; nv.Color = s.Color;
                    if (nv.On)
                    {
                        nv.On = s.On;
                        nv.enabled = s.Enabled;
                    }
                    nv.ApplySettings();
                }
            }
            catch (System.Exception ex)
            {
                PiPDisablerPlugin.LogSource.LogError($"[MagicOpticMountCompat] Restoring camera effects failed: {ex.Message}");
            }
            finally
            {
                _appliedThermal = null;
                _appliedNight = null;
            }
        }

        private static bool IsSpecialOptic(OpticSight os)
        {
            var data = os.ScopeData;
            if (data == null) return false;
            return (data.ThermalVisionData != null && data.ThermalVisionData.ThermalVision)
                   || (data.NightVisionData != null && data.NightVisionData.NightVision);
        }

        private static Transform FindBoneFor(EFT.Animations.ProceduralWeaponAnimation pwa, OpticSight os)
        {
            foreach (var sight in pwa.ScopeAimTransforms)
            {
                var cache = sight?.ScopePrefabCache;
                if (cache == null) continue;
                for (int m = 0; m < cache.ModesCount; m++)
                {
                    if (cache.GetOpticSight(m) == os)
                        return sight.Bone;
                }
            }
            return pwa.CurrentScope?.Bone;
        }

        private static bool AreSightsAligned(Transform opticBone, Transform specialOpticBone, Vector3 weaponForward)
        {
            // device is in front of the optic
            if (Vector3.Angle(specialOpticBone.position - opticBone.position, weaponForward) > MaxAngle)
                return false;

            // and close to the optic's view axis
            return SqDistPointSegment(opticBone.position, opticBone.position + weaponForward, specialOpticBone.position)
                   <= MaxDistance * MaxDistance;
        }

        private static float SqDistPointSegment(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            float e = Vector3.Dot(ac, ab);
            if (e <= 0f) return Vector3.Dot(ac, ac);
            float f = Vector3.Dot(ab, ab);
            if (e >= f)
            {
                Vector3 bc = c - b;
                return Vector3.Dot(bc, bc);
            }
            return Vector3.Dot(ac, ac) - e * e / f;
        }
    }
}
