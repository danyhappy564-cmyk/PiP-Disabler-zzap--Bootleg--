using System.Collections.Generic;
using BepInEx.Configuration;

namespace PiPDisabler
{
    /// <summary>
    /// Korean names, categories and short descriptions for the F12 (Configuration Manager) panel.
    /// Only the displayed text changes (ConfigurationManagerAttributes.DispName/Category/Description);
    /// the cfg section/key names stay English so existing user settings are kept.
    /// </summary>
    internal static class SettingsKorean
    {
        // When a change shows up, appended to every description.
        private const string Live = "\n[적용: 바로]";
        private const string Reaim = "\n[적용: 조준 중이면 바로 다시 적용]";
        private const string Saved = "\n[적용: 조준한 채로 '이 스코프 설정 저장' 키를 누르면 바로]";
        private const string Rarely = "\n※ 보통 건드릴 필요 없음.";

        private const string CatBasic = "1. 기본";
        private const string CatEffects = "2. 화면 효과";
        private const string CatPerScope = "3. 스코프별 설정 (조준 중인 스코프)";
        private const string CatMotion = "4. 동작 보정";
        private const string CatGraphics = "5. 그래픽";
        private const string CatCompat = "6. 다른 모드 호환";
        private const string CatMesh = "7. 스코프 몸통 자르기 (전체 기본값)";
        private const string CatDebug = "8. 디버그";

        private struct Text
        {
            public string Category, Name, Desc;
            public Text(string category, string name, string desc) { Category = category; Name = name; Desc = desc; }
        }

        private static readonly Dictionary<string, Text> Map = new Dictionary<string, Text>
        {
            // ── General ──
            [K("General", "Mod Enabled")] = new Text(CatBasic, "모드 켜기", "PiP-Disabler 전체 켜기/끄기." + Live),
            [K("General", "Mod Toggle Key")] = new Text(CatBasic, "모드 켜기/끄기 키", "이 키를 누르면 모드가 켜지고 꺼짐." + Live),
            [K("General", "Auto Disable For NV/Thermals")] = new Text(CatBasic, "열상·야간투시 스코프는 원래 방식",
                "열상/NV 스코프는 이 모드를 끄고 게임 원래 방식(PiP)으로 보여줌. 끄면 열상이 안 보임 — 켜 두세요." + Reaim),
            [K("General", "Scope Blacklist Names")] = new Text(CatBasic, "제외할 스코프 목록",
                "여기 적힌 스코프는 이 모드를 안 씀(원래 방식). 세미콜론(;)으로 구분. 아래 '제외 목록 추가/삭제 키'로 넣는 게 편함." + Reaim),
            [K("General", "Scope Blacklist Toggle Entry Key")] = new Text(CatBasic, "제외 목록 추가/삭제 키",
                "조준 중에 누르면 지금 스코프를 제외 목록에 넣거나 뺌." + Live),
            [K("General", "Scope Whitelist Names")] = new Text(CatBasic, "허용할 스코프 목록",
                "비어 있으면 무시. 적혀 있으면 이 스코프들에만 모드를 씀. 세미콜론(;)으로 구분." + Reaim),
            [K("General", "Scope Whitelist Toggle Entry Key")] = new Text(CatBasic, "허용 목록 추가/삭제 키",
                "조준 중에 누르면 지금 스코프를 허용 목록에 넣거나 뺌." + Live),
            [K("General", "FOV Fix Behaviour")] = new Text(CatBasic, "1배율 시야각 = 게임 설정 시야각",
                "켜면 1배율 스코프의 시야각을 게임 설정의 FOV로 맞춤." + Reaim),
            [K("General", "Global Scope Scaling Multiplier")] = new Text(CatBasic, "스코프 크기 (전체)",
                "모든 스코프가 화면에서 보이는 크기. 크게 = 값 올림." + Reaim),
            [K("General", "Global Reticle Scaling Multiplier")] = new Text(CatBasic, "조준선 크기 (전체)",
                "모든 조준선 크기. 크게 = 값 올림." + Reaim),
            [K("General", "Baseline FOV")] = new Text(CatBasic, "배율 계산 기준 시야각",
                "배율을 시야각으로 바꿀 때 쓰는 기준(예: 35면 2배율 = 17.5°). 작게 = 전체적으로 더 확대." + Reaim + Rarely),
            [K("General", "FOV Animation Duration")] = new Text(CatBasic, "줌 전환 시간(초)",
                "조준·해제·배율 변경 때 화면이 확대/축소되는 시간. 0 = 즉시." + Live),
            [K("General", "Depth of field")] = new Text(CatEffects, "렌즈 바깥 흐림",
                "스코프 렌즈 바깥을 흐리게 함." + Live),
            [K("General", "NVG Lens Blur")] = new Text(CatEffects, "야간투시 렌즈 흐림",
                "렌즈 안을 살짝 흐리게 해서 실제 야간투시 느낌을 냄." + Live),

            // ── Scope Effects ──
            [K("Scope Effects", "Vignette")] = new Text(CatEffects, "렌즈 가장자리 어둡게(비네팅)", "렌즈 테두리를 둥글게 어둡게." + Live),
            [K("Scope Effects", "Vignette Opacity")] = new Text(CatEffects, "비네팅 진하기", "0 = 안 보임, 1 = 완전 검정." + Live),
            [K("Scope Effects", "Vignette Radius")] = new Text(CatEffects, "비네팅 시작 위치", "작을수록 가운데부터 어두워짐." + Live),
            [K("Scope Effects", "Vignette Softness")] = new Text(CatEffects, "비네팅 부드러움", "0 = 딱 끊김, 1 = 부드럽게." + Live),
            [K("Scope Effects", "Scope Shadow")] = new Text(CatEffects, "렌즈 바깥 검게(그림자)", "렌즈 바깥 화면을 검게 덮음." + Live),
            [K("Scope Effects", "ScopeShadow Persist On Unscope")] = new Text(CatEffects, "조준 해제 후에도 그림자 잠깐 유지",
                "줌 전환 시간이 0일 때 유용." + Live + Rarely),
            [K("Scope Effects", "ScopeShadow Opacity")] = new Text(CatEffects, "그림자 진하기", "렌즈 바깥 그림자 진하기." + Live),
            [K("Scope Effects", "Depth of field Blur Downsample")] = new Text(CatEffects, "바깥 흐림 - 해상도 낮추기",
                "높을수록 가볍지만 뭉개짐." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Iterations")] = new Text(CatEffects, "바깥 흐림 - 반복 횟수", "높을수록 더 흐림." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radius")] = new Text(CatEffects, "바깥 흐림 - 세기", "높을수록 더 흐림." + Live),
            [K("Scope Effects", "Depth of field Blur Opacity")] = new Text(CatEffects, "바깥 흐림 - 섞는 정도", "0 = 흐림 없음, 1 = 완전히." + Live),
            [K("Scope Effects", "Depth of field Blur Darkening")] = new Text(CatEffects, "바깥 흐림 - 어둡게", "흐린 부분을 추가로 어둡게." + Live),
            [K("Scope Effects", "Depth of field Blur Radial Gate")] = new Text(CatEffects, "바깥 흐림 - 렌즈에서 멀수록만",
                "렌즈에서 떨어진 곳부터 흐려지게." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radial Gate Start")] = new Text(CatEffects, "바깥 흐림 - 시작 거리",
                "렌즈 반지름 기준 몇 배 거리부터 흐릴지." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radial Gate Softness")] = new Text(CatEffects, "바깥 흐림 - 시작 부드러움",
                "흐림이 시작되는 경계의 부드러움." + Live + Rarely),
            [K("Scope Effects", "NVG Lens Blur Multiplier")] = new Text(CatEffects, "야간투시 렌즈 흐림 세기", "높을수록 더 흐림." + Live),

            // ── Per scope settings ──
            [K("Per scope settings", "Current scope")] = new Text(CatPerScope, "지금 조준 중인 스코프",
                "어떤 스코프인지, 이 모드가 적용 중인지, 전용 설정이 저장돼 있는지 보여줌. 조준한 채로 F12를 열어 확인하세요."),
            [K("Per scope settings", "Save custom settings key")] = new Text(CatPerScope, "이 스코프 설정 저장 키",
                "조준한 채로 누르면 아래 값들을 '지금 조준 중인 스코프' 전용으로 저장하고 바로 다시 적용(화면 알림 뜸)."),
            [K("Per scope settings", "DeleteCustomMeshSurgerySettingsKey")] = new Text(CatPerScope, "이 스코프 설정 삭제 키",
                "조준한 채로 누르면 지금 스코프의 전용 설정을 지우고 기본값으로 되돌림(화면 알림 뜸)."),
            [K("Per scope settings", "Reticle Size Multiplier")] = new Text(CatPerScope, "조준선 크기 배수",
                "이 스코프의 조준선 크기. 1 = 그대로." + Saved),
            [K("Per scope settings", "Reticle Size")] = new Text(CatPerScope, "조준선 기본 크기",
                "배율 따라 안 변하는 조준선의 크기." + Saved + Rarely),
            [K("Per scope settings", "Variable Reticle Minimum Size")] = new Text(CatPerScope, "조준선 최소 크기(가변)",
                "최소·최대 둘 다 0이 아닐 때만 사용." + Saved + Rarely),
            [K("Per scope settings", "Variable Reticle Maximum Size")] = new Text(CatPerScope, "조준선 최대 크기(가변)",
                "최소·최대 둘 다 0이 아닐 때만 사용." + Saved + Rarely),
            [K("Per scope settings", "Weapon Scale Multiplier")] = new Text(CatPerScope, "총·스코프 크기 배수",
                "아래 '총 크기' 두 값에 한꺼번에 곱함. 1 = 그대로." + Saved),
            [K("Per scope settings", "Weapon Scale Min Magnification")] = new Text(CatPerScope, "총 크기 - 최저 배율일 때",
                "이 스코프를 최저 배율로 볼 때 총 모델 크기. 기본 1, 값이 클수록 총이 작아짐(스코프 몸통이 화면을 덜 가림)." + Saved),
            [K("Per scope settings", "Weapon Scale Max Magnification")] = new Text(CatPerScope, "총 크기 - 최고 배율일 때",
                "최고 배율일 때 총 모델 크기. 고배율에서 스코프 몸통·안쪽 원통이 크게 보이면 이 값을 올려 보세요." + Saved),
            [K("Per scope settings", "Visual Recoil Compensation")] = new Text(CatPerScope, "반동 화면 보정",
                "0 = 전체 기본값 사용. 음수면 방향 반대." + Saved + Rarely),
            [K("Per scope settings", "Vignette Opacity")] = new Text(CatPerScope, "비네팅 진하기 (이 스코프)",
                "0 = '화면 효과' 기본값 사용. 바꾸면 자동 저장." + Live),
            [K("Per scope settings", "Vignette Radius")] = new Text(CatPerScope, "비네팅 시작 위치 (이 스코프)",
                "0 = '화면 효과' 기본값 사용. 바꾸면 자동 저장." + Live),
            [K("Per scope settings", "Vignette Softness")] = new Text(CatPerScope, "비네팅 부드러움 (이 스코프)",
                "0 = '화면 효과' 기본값 사용. 바꾸면 자동 저장." + Live),
            [K("Per scope settings", "CutLength")] = new Text(CatPerScope, "몸통 자르기 - 길이",
                "렌즈 안쪽에 원통(몸통 내부)이 보이면 늘려 보세요." + Saved),
            [K("Per scope settings", "CutStartOffset")] = new Text(CatPerScope, "몸통 자르기 - 시작 위치",
                "눈 쪽으로 얼마나 앞에서부터 자를지." + Saved + Rarely),
            [K("Per scope settings", "NearPreserveDepth")] = new Text(CatPerScope, "몸통 자르기 - 접안부 남기는 두께",
                "눈 앞에 남기는 테두리 두께. 0 = 다 자름." + Saved + Rarely),
            [K("Per scope settings", "Plane1Radius")] = new Text(CatPerScope, "몸통 자르기 - 눈 쪽 반지름",
                "가까운 쪽 구멍 크기." + Saved + Rarely),
            [K("Per scope settings", "Plane2Radius")] = new Text(CatPerScope, "몸통 자르기 - 2번째 반지름", "" + Saved + Rarely),
            [K("Per scope settings", "Plane3Radius")] = new Text(CatPerScope, "몸통 자르기 - 3번째 반지름", "" + Saved + Rarely),
            [K("Per scope settings", "Plane4Radius")] = new Text(CatPerScope, "몸통 자르기 - 끝 쪽 반지름", "먼 쪽 구멍 크기." + Saved + Rarely),
            [K("Per scope settings", "Plane2Position")] = new Text(CatPerScope, "몸통 자르기 - 2번째 위치", "" + Saved + Rarely),
            [K("Per scope settings", "Plane3Position")] = new Text(CatPerScope, "몸통 자르기 - 3번째 위치", "" + Saved + Rarely),
            [K("Per scope settings", "Plane4Position")] = new Text(CatPerScope, "몸통 자르기 - 끝 위치", "" + Saved + Rarely),
            [K("Per scope settings", "PlaneOffsetMeters")] = new Text(CatPerScope, "몸통 자르기 - 면 오프셋", "" + Saved + Rarely),
            [K("Per scope settings", "Plane1OffsetMeters")] = new Text(CatPerScope, "몸통 자르기 - 눈 쪽 오프셋", "" + Saved + Rarely),
            [K("Per scope settings", "ExpandSearchToWeaponRoot")] = new Text(CatPerScope, "총 전체에서 자를 부품 찾기", "" + Saved + Rarely),

            // ── Hacks ──
            [K("Hacks", "Keep scope centered with zeroing")] = new Text(CatMotion, "영점 바꿔도 스코프 가운데 유지",
                "영점(거리)을 바꿔도 스코프가 화면 아래로 밀리지 않게 함. 대신 조준선이 탄착점 쪽으로 살짝 움직임." + Reaim),
            [K("Hacks", "ADS Scope Alignment Angle Tolerance")] = new Text(CatMotion, "질주 후 조준 정렬 허용 각도", "" + Live + Rarely),
            [K("Hacks", "Post Sprint ADS Gate Duration")] = new Text(CatMotion, "질주 후 조준 대기 시간", "" + Live + Rarely),
            [K("Hacks", "Bypass During Stance Transitions")] = new Text(CatMotion, "서기↔엎드리기 중엔 원래 방식", "" + Live + Rarely),
            [K("Hacks", "Post Stance ADS Gate Duration")] = new Text(CatMotion, "자세 변경 후 조준 대기 시간", "" + Live + Rarely),
            [K("Hacks", "Bypass during reload")] = new Text(CatMotion, "재장전 중엔 원래 방식", "재장전하는 동안 모드를 잠깐 끔." + Live),
            [K("Hacks", "Reload Bypass Modifier")] = new Text(CatMotion, "재장전 중 꺼지는 시간", "높을수록 짧게 꺼짐." + Live + Rarely),
            [K("Hacks", "Scale Sway With Camera FOV")] = new Text(CatMotion, "확대할수록 총 흔들림 줄이기", "" + Live),
            [K("Hacks", "Sway Modifier")] = new Text(CatMotion, "흔들림 줄이는 정도", "" + Live),
            [K("Hacks", "Suppress Fire Mode Switch Movement")] = new Text(CatMotion, "조정간 바꿀 때 총 움직임 없애기",
                "조정간은 바뀌고 동작만 생략." + Live),
            [K("Hacks", "Suppress Magnification Switch Movement")] = new Text(CatMotion, "배율 바꿀 때 총 움직임 없애기",
                "배율은 바뀌고 동작만 생략." + Live),
            [K("Hacks", "Force Recoil Return To Zero")] = new Text(CatMotion, "반동 후 총을 정확히 원위치", "" + Live + Rarely),
            [K("Hacks", "Manual Weapon Scale")] = new Text(CatMotion, "총 크기 (수동, 구버전)", "" + Reaim + Rarely),
            [K("Hacks", "Visual Recoil Compensation")] = new Text(CatMotion, "반동 화면 보정 (전체)", "0 = 끔." + Reaim + Rarely),

            // ── Graphics ──
            [K("Graphics", "Keep scoped LOD Bias until inventory is opened")] = new Text(CatGraphics, "조준 해제 후에도 원거리 디테일 유지",
                "인벤토리를 열 때까지 고배율용 디테일을 유지." + Live),
            [K("Graphics", "Manual LOD Bias")] = new Text(CatGraphics, "원거리 디테일 (수동)",
                "조준 중 멀리 있는 물체의 디테일. 0 = 자동. 높으면 FPS 하락." + Reaim),
            [K("Graphics", "Auto LOD bias multiplier")] = new Text(CatGraphics, "원거리 디테일 (자동 배수)",
                "배율 × 이 값. 높으면 FPS 하락." + Reaim),

            // ── Compatibility ──
            [K("Compatibility", "COTI thermal only inside lens")] = new Text(CatCompat, "COTI 열상은 렌즈 안에만",
                "COTI 클립온 열상을 렌즈 안에만 보이게." + Live),
            [K("Compatibility", "COTI thermal centred in scope")] = new Text(CatCompat, "COTI 열상 원을 스코프 가운데로",
                "위 설정이 켜져 있어야 함." + Live),
            [K("Compatibility", "COTI lens-only flip Y")] = new Text(CatCompat, "COTI 렌즈 바깥 상하 뒤집기",
                "렌즈 바깥이 위아래로 뒤집혀 보일 때만 켜세요." + Live + Rarely),

            // ── Global Mesh Surgery settings ──
            [K("Global Mesh Surgery settings", "PlaneOffsetMeters")] = new Text(CatMesh, "면 오프셋", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane1Radius")] = new Text(CatMesh, "눈 쪽 반지름", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane1OffsetMeters")] = new Text(CatMesh, "눈 쪽 오프셋", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane2Position")] = new Text(CatMesh, "2번째 위치", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane2Radius")] = new Text(CatMesh, "2번째 반지름", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane3Position")] = new Text(CatMesh, "3번째 위치", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane3Radius")] = new Text(CatMesh, "3번째 반지름", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane4Position")] = new Text(CatMesh, "끝 위치", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Plane4Radius")] = new Text(CatMesh, "끝 쪽 반지름", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "CutStartOffset")] = new Text(CatMesh, "자르기 시작 위치", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "CutLength")] = new Text(CatMesh, "자르기 길이", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "NearPreserveDepth")] = new Text(CatMesh, "접안부 남기는 두께", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "ExpandSearchToWeaponRoot")] = new Text(CatMesh, "총 전체에서 자를 부품 찾기", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Reticle Base Size")] = new Text(CatMesh, "조준선 기본 크기", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Mesh Reticle Min Scale")] = new Text(CatMesh, "입체 조준선 최소 크기", "0 = 끔." + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Mesh Reticle Max Scale")] = new Text(CatMesh, "입체 조준선 최대 크기", "0 = 끔." + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Mesh Reticle Normalized Scale")] = new Text(CatMesh, "입체 조준선 크기 배수", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Mesh Reticle Minimum Stroke Enabled")] = new Text(CatMesh, "입체 조준선 최소 두께 보장", "" + Reaim + Rarely),
            [K("Global Mesh Surgery settings", "Mesh Reticle Minimum Stroke Pixels")] = new Text(CatMesh, "입체 조준선 최소 두께(픽셀)", "" + Reaim + Rarely),

            // ── Debug ──
            [K("Debug", "Debug logging")] = new Text(CatDebug, "자세한 로그 남기기",
                "문제 제보할 때 켜고, LogOutput.log를 보내 주세요." + Live),
            [K("Debug", "DebugShowHousingMask")] = new Text(CatDebug, "렌즈 영역 빨갛게 표시", "" + Live + Rarely),
            [K("Debug", "DebugShowScopeShadowMask")] = new Text(CatDebug, "그림자 영역 초록색 표시", "" + Live + Rarely),
            [K("Debug", "Draw reticle after everything")] = new Text(CatDebug, "(구버전 설정, 효과 없음)", "" + Rarely),
        };

        // Advanced (hidden) in the original, shown here because they are the ones people tune per scope.
        private static readonly HashSet<string> Visible = new HashSet<string>
        {
            K("Per scope settings", "DeleteCustomMeshSurgerySettingsKey"),
            K("Per scope settings", "Weapon Scale Min Magnification"),
            K("Per scope settings", "Weapon Scale Max Magnification"),
            K("Per scope settings", "CutLength"),
            K("General", "Auto Disable For NV/Thermals"),
        };

        private static string K(string section, string key) => section + "\u0001" + key;

        public static void Apply(IEnumerable<ConfigEntryBase> entries)
        {
            foreach (var entry in entries)
            {
                if (entry?.Definition == null || entry.Description == null) continue;
                if (!Map.TryGetValue(K(entry.Definition.Section, entry.Definition.Key), out var text)) continue;

                var tags = entry.Description.Tags;
                ConfigurationManagerAttributes attributes = null;
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        attributes = tag as ConfigurationManagerAttributes;
                        if (attributes != null) break;
                    }
                }
                if (attributes == null) continue;

                if (Visible.Contains(K(entry.Definition.Section, entry.Definition.Key)))
                    attributes.IsAdvanced = false;
                attributes.DispName = text.Name;
                attributes.Category = text.Category;
                attributes.Description = text.Desc.TrimStart('\n');
            }
        }
    }
}
