using System.Collections.Generic;
using System.Text;
using EFT.CameraControl;
using UnityEngine;

namespace PiPDisabler
{
    /// <summary>
    /// Debug-only "what does the camera actually see through the lens" report, so a scope that
    /// looks wrong can be diagnosed from LogOutput.log without screenshots.
    ///
    /// Once the view has settled after aiming (Debug logging on), rays are cast from the main
    /// camera through points of the eyepiece lens (centre, 50 % and 90 % of its radius) and just
    /// outside it (115 %, 140 %), and tested against every mesh of the weapon as it is now (cut
    /// meshes included). For each ray the first hit is logged: object, mesh, material/shader,
    /// whether that face is a front face to the camera (a back face is only visible when the
    /// material draws both sides), and where it is relative to the lens plane. It also logs the
    /// camera (near clip, FOV, distance to the lens) and how big the lens is on screen.
    /// </summary>
    internal static class LensProbe
    {
        private struct MeshData
        {
            public string Go, Mesh, Material;
            public Vector3[] V;   // world space
            public int[] T;
            public bool Flip;
        }

        private struct Hit
        {
            public bool Any;
            public float Dist;
            public int MeshIndex;
            public bool Front;
            public float Axial;
        }

        public static string DescribeMaterial(Renderer r)
        {
            if (r == null) return "no renderer";
            try
            {
                var m = r.sharedMaterial;
                if (m == null) return "no material";
                string cull = m.HasProperty("_Cull") ? CullName(m.GetFloat("_Cull"))
                    : m.HasProperty("_CullMode") ? CullName(m.GetFloat("_CullMode")) : "cull=?";
                return $"mat='{m.name}' shader='{(m.shader != null ? m.shader.name : "?")}' {cull}";
            }
            catch { return "material ?"; }
        }

        private static string CullName(float v)
        {
            switch ((int)v)
            {
                case 0: return "cull=Off(two-sided)";
                case 1: return "cull=Front";
                case 2: return "cull=Back";
                default: return $"cull={v}";
            }
        }

        public static void Run(OpticSight os, string reason)
        {
            if (!Settings.DebugLogging.Value || os == null) return;
            var log = PiPDisablerPlugin.LogSource;

            try
            {
                var cam = Helpers.GetMainCamera();
                if (cam == null) { log.LogInfo("[Probe] no main camera"); return; }
                if (!MeshSurgeryManager.TryGetAutoPlaneWorld(out var scopeRoot, out var lensP, out var lensN))
                {
                    log.LogInfo("[Probe] skipped: no automatic-hole plane (manual hole or not cut yet)");
                    return;
                }
                float lensR = LensTransparency.GetEyepieceLensRadius(scopeRoot);
                var weaponRoot = MeshSurgeryManager.FindWeaponTransform(scopeRoot);
                if (weaponRoot == null || lensR <= 0f) { log.LogInfo($"[Probe] skipped: weaponRoot={(weaponRoot != null)} lensR={lensR}"); return; }

                Vector3 camPos = cam.transform.position;
                float camToLens = Vector3.Dot(lensP - camPos, lensN);
                Vector3 vp0 = cam.WorldToViewportPoint(lensP);
                Vector3 u = Vector3.Cross(lensN, Mathf.Abs(Vector3.Dot(lensN, Vector3.up)) > 0.9f ? Vector3.right : Vector3.up).normalized;
                Vector3 v = Vector3.Cross(lensN, u).normalized;
                Vector3 vpR = cam.WorldToViewportPoint(lensP + u * lensR);
                float lensPx = new Vector2((vpR.x - vp0.x) * cam.pixelWidth, (vpR.y - vp0.y) * cam.pixelHeight).magnitude;
                var player = Helpers.GetLocalPlayer();

                log.LogInfo($"[Probe] ===== {reason}: scope '{os.name}' root='{scopeRoot.name}' =====");
                log.LogInfo($"[Probe] camera: near={cam.nearClipPlane * 1000f:F1}mm far={cam.farClipPlane:F0}m fov={cam.fieldOfView:F2}° " +
                            $"camera→lens={camToLens * 1000f:F1}mm (off-axis {Vector3.ProjectOnPlane(lensP - camPos, lensN).magnitude * 1000f:F1}mm) " +
                            $"lens r={lensR * 1000f:F1}mm = {lensPx:F0}px on screen at ({vp0.x:F3},{vp0.y:F3}) " +
                            $"ribcage={(player != null ? player.RibcageScaleCurrent : 0f):F3} zoomMult={PerScopeMeshSurgerySettings.GetZoomMultiplier():F2} width={PerScopeMeshSurgerySettings.GetCutWidthMultiplierRaw():F2}");

                log.LogInfo($"[Probe] hidden lens surfaces (position from the eyepiece lens plane):{LensTransparency.DescribeHidden(lensP, lensN)}");

                // Gather meshes as they are now (cut meshes are readable; originals get a temp copy).
                var meshes = new List<MeshData>(64);
                var temps = new List<Mesh>();
                foreach (var mf in weaponRoot.GetComponentsInChildren<MeshFilter>(false))
                {
                    var r = mf.GetComponent<Renderer>();
                    if (mf.sharedMesh == null || r == null || !r.enabled || r.forceRenderingOff) continue;
                    if (mf.sharedMesh.vertexCount == 0) continue;
                    Mesh m = mf.sharedMesh;
                    if (!m.isReadable)
                    {
                        m = MeshPlaneCutter.MakeReadableMeshCopy(m);
                        if (m == null) continue;
                        temps.Add(m);
                    }
                    var local = m.vertices;
                    var mtx = mf.transform.localToWorldMatrix;
                    var world = new Vector3[local.Length];
                    for (int i = 0; i < local.Length; i++) world[i] = mtx.MultiplyPoint3x4(local[i]);
                    meshes.Add(new MeshData
                    {
                        Go = mf.name, Mesh = mf.sharedMesh.name, Material = DescribeMaterial(r),
                        V = world, T = m.triangles, Flip = mtx.determinant < 0f
                    });
                }
                foreach (var t in temps) Object.Destroy(t);

                // Rays through the lens and just outside it.
                float[] rings = { 0f, 0.5f, 0.9f, 1.15f, 1.4f };
                int front = 0, back = 0, none = 0;
                var summary = new Dictionary<string, int>();
                foreach (float f in rings)
                {
                    int steps = f == 0f ? 1 : 8;
                    for (int k = 0; k < steps; k++)
                    {
                        float a = k * Mathf.PI * 2f / steps;
                        Vector3 target = lensP + (u * Mathf.Cos(a) + v * Mathf.Sin(a)) * (f * lensR);
                        Vector3 dir = (target - camPos).normalized;
                        Hit h = CastRay(meshes, camPos + dir * cam.nearClipPlane, dir, lensP, lensN);
                        string where = f == 0f ? "centre" : $"{f * 100f:F0}%@{k * 45}°";
                        if (!h.Any)
                        {
                            if (f <= 1f) none++;
                            log.LogInfo($"[Probe] {where,-9}: clear (nothing hit)");
                            continue;
                        }
                        var md = meshes[h.MeshIndex];
                        if (f <= 1f) { if (h.Front) front++; else back++; }
                        if (f <= 1f)
                        {
                            string key = $"{md.Go}/{md.Mesh}";
                            summary[key] = (summary.TryGetValue(key, out int c) ? c : 0) + 1;
                        }
                        log.LogInfo($"[Probe] {where,-9}: hit '{md.Go}' mesh='{md.Mesh}' {(h.Front ? "FRONT face" : "back face")} " +
                                    $"at {h.Dist * 1000f:F0}mm from camera, {h.Axial * 1000f:+0;-0}mm from lens plane, {md.Material}");
                    }
                }
                var sb = new StringBuilder();
                foreach (var kv in summary) sb.Append($" '{kv.Key}'×{kv.Value}");
                log.LogInfo($"[Probe] inside the lens (17 rays): clear={none}, blocked by front faces={front}, by back faces={back} (back faces only show if two-sided).{(sb.Length > 0 ? " Blockers:" + sb : "")}");
            }
            catch (System.Exception ex)
            {
                log.LogInfo($"[Probe] failed: {ex.Message}");
            }
        }

        private static Hit CastRay(List<MeshData> meshes, Vector3 o, Vector3 d, Vector3 lensP, Vector3 lensN)
        {
            var best = new Hit { Dist = float.MaxValue };
            for (int mi = 0; mi < meshes.Count; mi++)
            {
                var md = meshes[mi];
                var V = md.V; var T = md.T;
                for (int i = 0; i + 2 < T.Length; i += 3)
                {
                    Vector3 a = V[T[i]], b = V[T[i + 1]], c = V[T[i + 2]];
                    Vector3 e1 = b - a, e2 = c - a;
                    Vector3 p = Vector3.Cross(d, e2);
                    float det = Vector3.Dot(e1, p);
                    if (det > -1e-9f && det < 1e-9f) continue;
                    float inv = 1f / det;
                    Vector3 s = o - a;
                    float uu = Vector3.Dot(s, p) * inv;
                    if (uu < 0f || uu > 1f) continue;
                    Vector3 q = Vector3.Cross(s, e1);
                    float vv = Vector3.Dot(d, q) * inv;
                    if (vv < 0f || uu + vv > 1f) continue;
                    float t = Vector3.Dot(e2, q) * inv;
                    if (t <= 0f || t >= best.Dist) continue;
                    Vector3 n = Vector3.Cross(e1, e2);
                    if (md.Flip) n = -n;
                    best = new Hit
                    {
                        Any = true, Dist = t, MeshIndex = mi,
                        Front = Vector3.Dot(n, -d) > 0f,
                        Axial = Vector3.Dot(o + d * t - lensP, lensN)
                    };
                }
            }
            return best;
        }
    }
}
