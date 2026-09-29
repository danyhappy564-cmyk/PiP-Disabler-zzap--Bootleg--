using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using EFT.Animations;
using EFT.CameraControl;
using UnityEngine;

namespace PiPDisabler
{
    internal static class Helpers
    {
        internal static Camera GetMainCamera()
        {
            try
            {
                if (CameraManager.Exist)
                {
                    var cam = CameraManager.Instance.Camera;
                    if (cam != null) return cam;
                }
            }
            catch { }
            return Camera.main;
        }

        /// <summary>
        /// Returns the local player via GameWorld singleton.
        /// </summary>
        internal static Player GetLocalPlayer()
        {
            try
            {
                var gw = Singleton<GameWorld>.Instance;
                return gw?.MainPlayer;
            }
            catch { return null; }
        }

        /// <summary>
        /// Returns the final backbuffer viewport in pixels.
        /// </summary>
        internal static Rect GetDisplayViewport(Camera cam)
        {
            return new Rect(
                0f,
                0f,
                Mathf.Max(1f, Screen.width),
                Mathf.Max(1f, Screen.height));
        }

        /// <summary>
        /// Check if two transforms share the same mode_XXX ancestor.
        /// </summary>
        internal static bool IsOnSameMode(Transform a, Transform b)
        {
            var mA = FindModeAncestor(a);
            var mB = FindModeAncestor(b);
            return mA == mB;
        }

        internal static float GetBaseFov(this ProceduralWeaponAnimation pwa)
        {
            return pwa != null ? pwa.HeadBobbing : Settings.BaselineFOV.Value;
        }

        private static readonly System.Reflection.FieldInfo EventsConsumersField =
            HarmonyLib.AccessTools.Field(typeof(FirearmsAnimator), "_playerEventsConsumers")
            ?? HarmonyLib.AccessTools.Field(typeof(FirearmsAnimator), "EventsConsumers")
            ?? HarmonyLib.AccessTools.Field(typeof(FirearmsAnimator), "_eventsConsumers");

        internal static System.Collections.IEnumerable GetEventsConsumers(this FirearmsAnimator fa)
        {
            if (fa == null || EventsConsumersField == null) return null;
            try { return EventsConsumersField.GetValue(fa) as System.Collections.IEnumerable; } catch { return null; }
        }

        private static readonly System.Reflection.FieldInfo ManagerRtField =
            HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "renderTexture_0")
            ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "RenderTexture_0")
            ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_renderTexture");

        private static readonly System.Reflection.PropertyInfo ManagerRtProp =
            HarmonyLib.AccessTools.Property(typeof(EFT.CameraControl.OpticCameraManager), "RenderTexture_0")
            ?? HarmonyLib.AccessTools.Property(typeof(EFT.CameraControl.OpticCameraManager), "RenderTexture");

        internal static RenderTexture GetRenderTexture(this EFT.CameraControl.OpticCameraManager manager)
        {
            if (manager == null) return null;
            try
            {
                if (ManagerRtProp != null) return ManagerRtProp.GetValue(manager) as RenderTexture;
                if (ManagerRtField != null) return ManagerRtField.GetValue(manager) as RenderTexture;
            }
            catch { }
            return manager.Camera != null ? manager.Camera.targetTexture : null;
        }

        internal static void SetRenderTexture(this EFT.CameraControl.OpticCameraManager manager, RenderTexture rt)
        {
            if (manager == null) return;
            try
            {
                if (ManagerRtProp != null && ManagerRtProp.CanWrite) ManagerRtProp.SetValue(manager, rt);
                else ManagerRtField?.SetValue(manager, rt);
            }
            catch { }
        }

        private static readonly System.Reflection.FieldInfo UpdaterField =
            HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "OpticComponentUpdater_0")
            ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_updater")
            ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_opticComponentUpdater");

        private static readonly System.Reflection.PropertyInfo UpdaterProp =
            HarmonyLib.AccessTools.Property(typeof(EFT.CameraControl.OpticCameraManager), "OpticComponentUpdater_0")
            ?? HarmonyLib.AccessTools.Property(typeof(EFT.CameraControl.OpticCameraManager), "OpticComponentUpdater");

        internal static OpticComponentUpdater GetOpticComponentUpdater(this EFT.CameraControl.OpticCameraManager manager)
        {
            if (manager == null) return null;
            try
            {
                if (UpdaterProp != null) return UpdaterProp.GetValue(manager) as OpticComponentUpdater;
                if (UpdaterField != null) return UpdaterField.GetValue(manager) as OpticComponentUpdater;
            }
            catch { }
            return manager.Camera != null ? manager.Camera.GetComponent<OpticComponentUpdater>() : null;
        }

        private static int _opticTexId = -1;
        internal static int GetOpticTexPropertyId()
        {
            if (_opticTexId == -1)
            {
                try
                {
                    var f = HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_camTexId")
                         ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "Int_0")
                         ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_renderTextureId")
                         ?? HarmonyLib.AccessTools.Field(typeof(EFT.CameraControl.OpticCameraManager), "_opticTexId");
                    if (f != null) _opticTexId = (int)f.GetValue(null);
                }
                catch { }
                if (_opticTexId == -1) _opticTexId = Shader.PropertyToID("_CamTex");
            }
            return _opticTexId;
        }

        private static Transform FindModeAncestor(Transform t)
        {
            for (var p = t; p != null; p = p.parent)
                if (p.name != null && p.name.StartsWith("mode_", StringComparison.OrdinalIgnoreCase))
                    return p;
            return null;
        }
    }
    internal static class InputProxy
    {
        private static System.Type _inputType;
        private static System.Reflection.MethodInfo _getKeyDown;
        private static System.Reflection.MethodInfo _getKey;
        private static System.Reflection.PropertyInfo _mouseScrollDelta;

        static InputProxy()
        {
            _inputType = System.Type.GetType("UnityEngine.Input, UnityEngine.InputLegacyModule")
                      ?? System.Type.GetType("UnityEngine.Input, UnityEngine");
            if (_inputType != null)
            {
                _getKeyDown = _inputType.GetMethod("GetKeyDown", new[] { typeof(KeyCode) });
                _getKey = _inputType.GetMethod("GetKey", new[] { typeof(KeyCode) });
                _mouseScrollDelta = _inputType.GetProperty("mouseScrollDelta",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            }
        }

        public static bool GetKeyDown(KeyCode key)
        {
            try
            {
                if (_getKeyDown == null) return false;
                return (bool)_getKeyDown.Invoke(null, new object[] { key });
            }
            catch { return false; }
        }

        public static bool GetKey(KeyCode key)
        {
            try
            {
                if (_getKey == null) return false;
                return (bool)_getKey.Invoke(null, new object[] { key });
            }
            catch { return false; }
        }
    }
}
