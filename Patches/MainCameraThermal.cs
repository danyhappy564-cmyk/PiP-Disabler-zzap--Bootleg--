using EFT.CameraControl;

namespace PiPDisabler.Patches
{
    /// <summary>
    /// Thermal scopes without PiP: vanilla draws a thermal scope's image with the ThermalVision
    /// component on the optic camera, which PiP-Disabler switches off. While such a scope is
    /// aimed, the scope's ThermalVisionData is put on the main camera's own ThermalVision (the
    /// component thermal goggles use) and restored on exit. The whole screen turns thermal,
    /// the scope housing included; the scope shadow/vignette (drawn after it) stays black.
    ///
    /// On is set directly, not via Switch(): Switch() also enables the goggle tube mask, the
    /// goggle-only components and the chromatic aberration shift.
    /// </summary>
    internal static class MainCameraThermal
    {
        private struct ThermalState
        {
            public bool Enabled, IsNoisy, IsFpsStuck, IsMotionBlurred, IsGlitch, IsPixelated;
            public ThermalVisionUtilities Utilities;
            public StuckFPSUtilities StuckFps;
            public MotionBlurUtilities MotionBlur;
            public GlitchUtilities Glitch;
            public PixelationUtilities Pixelation;
            public float ChromaticShift, UnsharpBias, UnsharpRadius;
        }

        private static ThermalVision _applied;
        private static ThermalState _saved;

        /// <summary>True for a scope mode with thermal and without night vision.</summary>
        public static bool IsThermalOnly(OpticSight os)
        {
            var data = os != null ? os.ScopeData : null;
            if (data == null) return false;
            bool thermal = data.ThermalVisionData != null && data.ThermalVisionData.ThermalVision;
            bool night = data.NightVisionData != null && data.NightVisionData.NightVision;
            return thermal && !night;
        }

        /// <summary>Called on (non-bypassed) scope enter and after a mode switch.</summary>
        public static void OnScopeEnter(OpticSight os)
        {
            Restore();
            if (!Settings.ThermalScopesWithoutPiP.Value || !IsThermalOnly(os)) return;
            if (!CameraManager.Exist || CameraManager.Instance == null) return;

            try
            {
                var thermalData = os.ScopeData.ThermalVisionData;
                var tv = CameraManager.Instance.ThermalVision;
                if (tv == null) return;

                // Thermal goggles already on — leave them alone.
                if (tv.On && tv.enabled) return;

                _saved = new ThermalState
                {
                    Enabled = tv.enabled, IsNoisy = tv.IsNoisy, IsFpsStuck = tv.IsFpsStuck,
                    IsMotionBlurred = tv.IsMotionBlurred, IsGlitch = tv.IsGlitch, IsPixelated = tv.IsPixelated,
                    Utilities = tv.ThermalVisionUtilities, StuckFps = tv.StuckFpsUtilities,
                    MotionBlur = tv.MotionBlurUtilities, Glitch = tv.GlitchUtilities, Pixelation = tv.PixelationUtilities,
                    ChromaticShift = tv.ChromaticAberrationThermalShift, UnsharpBias = tv.UnsharpBias, UnsharpRadius = tv.UnsharpRadiusBlur
                };

                // Same assignments vanilla makes on the optic camera (OpticComponentUpdater.CopyComponentFromOptic).
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
                _applied = tv;
                PiPDisablerPlugin.DebugLogInfo($"[MainCameraThermal] Thermal of '{os.name}' applied to the main camera.");
            }
            catch (System.Exception ex)
            {
                PiPDisablerPlugin.LogSource.LogError($"[MainCameraThermal] Applying thermal failed: {ex.Message}");
                Restore();
            }
        }

        /// <summary>Called on scope exit, bypass, mode switch and mod shutdown.</summary>
        public static void Restore()
        {
            var tv = _applied;
            _applied = null;
            if (tv == null) return;

            try
            {
                var s = _saved;
                tv.On = false;
                tv.IsNoisy = s.IsNoisy; tv.IsFpsStuck = s.IsFpsStuck; tv.IsMotionBlurred = s.IsMotionBlurred;
                tv.IsGlitch = s.IsGlitch; tv.IsPixelated = s.IsPixelated;
                tv.ThermalVisionUtilities = s.Utilities; tv.StuckFpsUtilities = s.StuckFps;
                tv.MotionBlurUtilities = s.MotionBlur; tv.GlitchUtilities = s.Glitch; tv.PixelationUtilities = s.Pixelation;
                tv.ChromaticAberrationThermalShift = s.ChromaticShift; tv.UnsharpBias = s.UnsharpBias; tv.UnsharpRadiusBlur = s.UnsharpRadius;
                tv.enabled = s.Enabled;
                PiPDisablerPlugin.DebugLogInfo("[MainCameraThermal] Main camera thermal restored.");
            }
            catch (System.Exception ex)
            {
                PiPDisablerPlugin.LogSource.LogError($"[MainCameraThermal] Restoring thermal failed: {ex.Message}");
            }
        }
    }
}
