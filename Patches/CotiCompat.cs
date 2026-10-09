using System;
using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;

namespace PiPDisabler.Patches
{
    /// <summary>
    /// COTI (clip-on thermal) compatibility: while scoped, centre COTI's NVG tube circle on the
    /// scope lens so its heat fills the lens instead of a monocular tube's off-centre circle.
    ///
    /// COTI resolves its host config (circle centre/radius) and tube mask every frame in
    /// CotiState.Update, and everything after it (thermal crop, overlay mask) reads those two
    /// statics. Swapping them right after that call means COTI never regenerates its own mask
    /// (a full-resolution C# loop), so there is no hitch on ADS. The centred mask is built once
    /// with COTI's own generator and cached.
    /// </summary>
    internal static class CotiCompat
    {
        private const string CotiGuid = "com.lennoxp90.coti";
        private const string HarmonyId = "com.fiodor.pipdisabler.coticompat";

        // Circle radius (fraction of screen height) used while scoped; large enough to cover the
        // lens, which PiP-Disabler keeps centred. The lens-only restore trims everything outside.
        private const float CenteredMinRadius = 0.45f;
        private const int ResyncIntervalFrames = 60;

        private static FieldInfo _hostField;
        private static FieldInfo _maskField;
        private static MethodInfo _buildMethod;
        private static MethodInfo _memberwiseClone;
        private static PropertyInfo _centerX, _centerY, _radius, _feather;

        private static object _sourceHost;
        private static object _centeredHost;
        private static Texture2D _sourceMask;
        private static Texture2D _centeredMask;
        private static int _maskWidth, _maskHeight;
        private static float _maskRadius = -1f, _maskFeather = -1f;
        private static int _nextResyncFrame;
        private static bool _failed;
        private static bool _loggedBuilt;

        public static void Enable()
        {
            if (!Chainloader.PluginInfos.ContainsKey(CotiGuid))
                return;

            try
            {
                Type stateType = AccessTools.TypeByName("Coti.Client.CotiState");
                Type generatorType = AccessTools.TypeByName("Coti.Client.MaskGenerator");
                Type hostType = AccessTools.TypeByName("Coti.Client.CotiNvgHostConfig");
                if (stateType == null || generatorType == null || hostType == null)
                {
                    PiPDisablerPlugin.LogSource.LogWarning("[CotiCompat] COTI types not found (unsupported COTI version) — scope centring disabled.");
                    return;
                }

                _hostField = AccessTools.Field(stateType, "Host");
                _maskField = AccessTools.Field(stateType, "Mask");
                _buildMethod = AccessTools.Method(generatorType, "Build", new[] { hostType, typeof(int), typeof(int) });
                _centerX = AccessTools.Property(hostType, "MaskCenterX");
                _centerY = AccessTools.Property(hostType, "MaskCenterY");
                _radius = AccessTools.Property(hostType, "MaskRadius");
                _feather = AccessTools.Property(hostType, "MaskFeather");
                _memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                MethodInfo update = AccessTools.Method(stateType, "Update", new[] { typeof(string), typeof(bool), typeof(bool) });

                if (_hostField == null || _maskField == null || _buildMethod == null || update == null
                    || _centerX == null || _centerY == null || _radius == null || _feather == null || _memberwiseClone == null)
                {
                    PiPDisablerPlugin.LogSource.LogWarning("[CotiCompat] COTI members not found (unsupported COTI version) — scope centring disabled.");
                    return;
                }

                new Harmony(HarmonyId).Patch(update,
                    postfix: new HarmonyMethod(typeof(CotiCompat).GetMethod(nameof(UpdatePostfix), BindingFlags.NonPublic | BindingFlags.Static)));
                PiPDisablerPlugin.LogSource.LogInfo("[CotiCompat] COTI detected — tube circle is centred on the scope while aiming.");
            }
            catch (Exception ex)
            {
                PiPDisablerPlugin.LogSource.LogError($"[CotiCompat] Enable failed: {ex.Message}");
            }
        }

        private static void UpdatePostfix()
        {
            if (_failed) return;

            try
            {
                object host = _hostField.GetValue(null);
                Texture2D mask = _maskField.GetValue(null) as Texture2D;
                if (host == null || mask == null)
                    return;

                bool enabled = Settings.ModEnabled.Value
                               && Settings.CotiThermalLensOnly.Value
                               && Settings.CotiCenterInScope.Value;
                if (!enabled)
                {
                    RestoreIfSwapped(host);
                    return;
                }

                // Prepared even when not aiming, so the one-time build happens when COTI starts
                // (it builds its own mask at that moment too) rather than on the first ADS.
                if (!ReferenceEquals(mask, _centeredMask))
                    _sourceMask = mask;
                PrepareCentered(host, mask.width, mask.height);

                bool scoped = ScopeLifecycle.IsScoped && !ScopeLifecycle.IsModBypassedForCurrentScope;
                if (!scoped || _centeredHost == null || _centeredMask == null)
                {
                    RestoreIfSwapped(host);
                    return;
                }

                _hostField.SetValue(null, _centeredHost);
                _maskField.SetValue(null, _centeredMask);
            }
            catch (Exception ex)
            {
                _failed = true;
                PiPDisablerPlugin.LogSource.LogError($"[CotiCompat] Disabled after error: {ex}");
            }
        }

        // If COTI kept our swapped values (an early-out path that doesn't reassign them), put its
        // own back when we are no longer aiming.
        private static void RestoreIfSwapped(object host)
        {
            if (!ReferenceEquals(host, _centeredHost) || _sourceHost == null)
                return;
            _hostField.SetValue(null, _sourceHost);
            if (_sourceMask != null)
                _maskField.SetValue(null, _sourceMask);
        }

        // 2.8.3 diagnosis ("circle off-centre after switching PiP off and on"): one line whenever
        // COTI hands us a host with different circle values, with the aim state at that moment.
        private static string _lastSourceLog;
        private static void LogSourceChange(object source)
        {
            try
            {
                string line = $"cx={(float)_centerX.GetValue(source):F3} cy={(float)_centerY.GetValue(source):F3} r={(float)_radius.GetValue(source):F3}";
                if (line == _lastSourceLog) return;
                _lastSourceLog = line;
                PiPDisablerPlugin.LogSource.LogInfo(
                    $"[CotiCompat] COTI circle source {line} ({source.GetType().Name}), mod {(Settings.ModEnabled.Value ? "on" : "off")}, " +
                    $"aiming={ScopeLifecycle.IsScoped}, PiP for this scope={ScopeLifecycle.IsModBypassedForCurrentScope}");
            }
            catch { }
        }

        private static void PrepareCentered(object host, int width, int height)
        {
            object source = ReferenceEquals(host, _centeredHost) ? _sourceHost : host;
            if (source == null) return;

            bool newSource = !ReferenceEquals(source, _sourceHost) || _centeredHost == null;
            if (!newSource && Time.frameCount < _nextResyncFrame
                && _centeredMask != null && _maskWidth == width && _maskHeight == height)
                return;
            _nextResyncFrame = Time.frameCount + ResyncIntervalFrames;

            if (newSource)
            {
                _sourceHost = source;
                _centeredHost = _memberwiseClone.Invoke(source, null);
                LogSourceChange(source);
            }

            // Re-read periodically: COTI's mask tuner can change the source while in raid.
            float radius = Mathf.Max((float)_radius.GetValue(source), CenteredMinRadius);
            float feather = (float)_feather.GetValue(source);
            _centerX.SetValue(_centeredHost, 0.5f);
            _centerY.SetValue(_centeredHost, 0.5f);
            _radius.SetValue(_centeredHost, radius);
            _feather.SetValue(_centeredHost, feather);

            if (_centeredMask != null && _maskWidth == width && _maskHeight == height
                && Mathf.Approximately(_maskRadius, radius) && Mathf.Approximately(_maskFeather, feather))
                return;

            if (_centeredMask != null)
                UnityEngine.Object.Destroy(_centeredMask);
            _centeredMask = _buildMethod.Invoke(null, new object[] { _centeredHost, width, height }) as Texture2D;
            _maskWidth = width;
            _maskHeight = height;
            _maskRadius = radius;
            _maskFeather = feather;

            if (!_loggedBuilt)
            {
                _loggedBuilt = true;
                PiPDisablerPlugin.LogSource.LogInfo(
                    $"[CotiCompat] Built centred COTI mask {width}x{height} r={radius:F3} f={feather:F3}");
            }
        }
    }
}
