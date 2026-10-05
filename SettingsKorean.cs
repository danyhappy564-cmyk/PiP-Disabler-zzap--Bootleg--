using System.Collections.Generic;
using BepInEx.Configuration;

namespace PiPDisabler
{
    /// <summary>
    /// Korean names, categories and short descriptions for the F12 (Configuration Manager) panel.
    /// Only the displayed text changes (ConfigurationManagerAttributes.DispName/Category/Description);
    /// the cfg section/key names stay English so existing user settings are kept.
    /// Written for players, not modders: what it does and which way to move it.
    /// </summary>
    internal static class SettingsKorean
    {
        private const string Live = "\n[바로 적용]";
        private const string PerScope = "\n[바로 보임 · F12 창을 닫으면 지금(또는 마지막으로) 조준한 스코프에 저장]";
        private const string PerScopeCut = "\n[F12 창을 닫으면 적용 · 그 스코프에 저장]";
        private const string Rarely = "\n※ 보통은 안 건드려도 됩니다.";

        private const string CatBasic = "1. 기본";
        private const string CatEffects = "2. 화면 효과";
        private const string CatPerScope = "3. 스코프별 설정 (조준 중인 스코프)";
        private const string CatMotion = "4. 총 움직임";
        private const string CatGraphics = "5. 그래픽";
        private const string CatCompat = "6. 다른 모드 호환";
        private const string CatMesh = "7. 스코프 몸통 구멍 (기본값)";
        private const string CatDebug = "8. 문제 확인용";

        private struct Text
        {
            public string Category, Name, Desc;
            public Text(string category, string name, string desc) { Category = category; Name = name; Desc = desc; }
        }

        private static readonly Dictionary<string, Text> Map = new Dictionary<string, Text>
        {
            // ── 1. 기본 ──
            [K("General", "Mod Enabled")] = new Text(CatBasic, "모드 켜기",
                "PiP-Disabler를 켜고 끕니다." + Live),
            [K("General", "Mod Toggle Key")] = new Text(CatBasic, "모드 켜기/끄기 단축키",
                "게임 중에 이 키로 모드를 켜고 끕니다." + Live),
            [K("General", "Auto Disable For NV/Thermals")] = new Text(CatBasic, "열상·야간투시 스코프는 게임 기본 방식으로",
                "열상/야간투시 스코프에서는 이 모드를 끄고 게임 원래 화면으로 보여줍니다. 끄면 열상·야간투시가 안 보이니 켜 두세요." + Live),
            [K("General", "Scope Blacklist Names")] = new Text(CatBasic, "이 모드를 안 쓸 스코프",
                "여기 적힌 스코프는 게임 원래 방식으로 보입니다. 직접 쓰기보다 아래 '빼기/다시 넣기 키'를 쓰는 게 편합니다." + Live),
            [K("General", "Scope Blacklist Toggle Entry Key")] = new Text(CatBasic, "이 스코프 빼기/다시 넣기 키",
                "조준한 채로 누르면 지금 스코프를 위 목록에 넣거나 뺍니다." + Live),
            [K("General", "Scope Whitelist Names")] = new Text(CatBasic, "이 모드를 쓸 스코프만 고르기",
                "비어 있으면 모든 스코프에 씁니다. 뭔가 적혀 있으면 그 스코프에만 씁니다." + Live + Rarely),
            [K("General", "Scope Whitelist Toggle Entry Key")] = new Text(CatBasic, "이 스코프만 쓰기 목록 넣기/빼기 키",
                "조준한 채로 누르면 지금 스코프를 위 목록에 넣거나 뺍니다." + Live + Rarely),
            [K("General", "FOV Fix Behaviour")] = new Text(CatBasic, "1배율은 게임 설정 시야 그대로",
                "켜면 1배율 스코프(도트 등)는 확대 없이 게임 설정의 시야(FOV) 그대로 보입니다." + Live),
            [K("General", "Global Scope Scaling Multiplier")] = new Text(CatBasic, "총 크기 (기본값)",
                "스코프 볼 때 총이 화면에 보이는 크기. 클수록 총이 작아짐. 스코프별 '총 크기'를 정해둔 스코프에는 적용 안 됨." + Live),
            [K("General", "Global Reticle Scaling Multiplier")] = new Text(CatBasic, "조준선 크기 (기본값)",
                "조준선 크기. 클수록 커짐. 스코프별 '조준선 크기'를 정해둔 스코프에는 적용 안 됨." + Live),
            [K("General", "Baseline FOV")] = new Text(CatBasic, "확대 정도 (모든 스코프)",
                "작을수록 모든 스코프가 더 당겨집니다(20 = 가장 많이, 35 = 기본). 스코프 하나만 바꾸려면 '3. 스코프별 설정 → 확대 배수'." + Live),
            [K("General", "FOV Animation Duration")] = new Text(CatBasic, "줌 전환 시간(초)",
                "조준할 때·풀 때·배율 바꿀 때 화면이 당겨지는 데 걸리는 시간. 0 = 즉시." + Live),
            [K("General", "Depth of field")] = new Text(CatEffects, "렌즈 바깥 흐리게",
                "스코프 렌즈 바깥 화면을 흐리게 합니다." + Live),
            [K("General", "NVG Lens Blur")] = new Text(CatEffects, "야간투시 렌즈 살짝 흐리게",
                "야간투시 고글을 쓰고 스코프를 볼 때 렌즈 안을 살짝 흐리게 해 실제 느낌을 냅니다." + Live),

            // ── 2. 화면 효과 ──
            [K("Scope Effects", "Vignette")] = new Text(CatEffects, "렌즈 테두리 어둡게",
                "렌즈 가장자리를 둥글게 어둡게 합니다." + Live),
            [K("Scope Effects", "Vignette Opacity")] = new Text(CatEffects, "렌즈 테두리 - 진하기",
                "0 = 안 보임, 1 = 완전 검정." + Live),
            [K("Scope Effects", "Vignette Radius")] = new Text(CatEffects, "렌즈 테두리 - 두께",
                "작을수록 가운데 가까이까지 어두워집니다." + Live),
            [K("Scope Effects", "Vignette Softness")] = new Text(CatEffects, "렌즈 테두리 - 번짐",
                "0 = 경계가 딱 끊김, 1 = 부드럽게 번짐." + Live),
            [K("Scope Effects", "Scope Shadow")] = new Text(CatEffects, "렌즈 바깥 검게",
                "렌즈 바깥 화면을 검게 덮습니다." + Live),
            [K("Scope Effects", "ScopeShadow Opacity")] = new Text(CatEffects, "렌즈 바깥 검게 - 진하기",
                "0 = 안 보임, 1 = 완전 검정." + Live),
            [K("Scope Effects", "ScopeShadow Persist On Unscope")] = new Text(CatEffects, "조준 풀 때 검은 화면 잠깐 유지",
                "줌 전환 시간이 0일 때 화면이 번쩍이면 켜 보세요." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radius")] = new Text(CatEffects, "바깥 흐림 - 세기",
                "클수록 더 흐립니다." + Live),
            [K("Scope Effects", "Depth of field Blur Darkening")] = new Text(CatEffects, "바깥 흐림 - 어둡게",
                "흐린 부분을 얼마나 더 어둡게 할지. 0 = 안 어둡게." + Live),
            [K("Scope Effects", "Depth of field Blur Opacity")] = new Text(CatEffects, "바깥 흐림 - 진하기",
                "0 = 흐림 없음, 1 = 완전히 흐림." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Iterations")] = new Text(CatEffects, "바깥 흐림 - 품질",
                "클수록 더 고르게 흐리지만 조금 무거워집니다." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Downsample")] = new Text(CatEffects, "바깥 흐림 - 가볍게",
                "클수록 가벼워지지만 흐림이 뭉개져 보입니다." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radial Gate")] = new Text(CatEffects, "바깥 흐림 - 렌즈 근처는 선명하게",
                "켜면 렌즈에서 조금 떨어진 곳부터 흐려집니다." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radial Gate Start")] = new Text(CatEffects, "바깥 흐림 - 흐려지기 시작하는 거리",
                "렌즈 크기의 몇 배 떨어진 곳부터 흐릴지." + Live + Rarely),
            [K("Scope Effects", "Depth of field Blur Radial Gate Softness")] = new Text(CatEffects, "바깥 흐림 - 시작 경계 번짐",
                "선명→흐림으로 바뀌는 경계를 얼마나 부드럽게 할지." + Live + Rarely),
            [K("Scope Effects", "NVG Lens Blur Multiplier")] = new Text(CatEffects, "야간투시 렌즈 흐림 - 세기",
                "클수록 더 흐립니다." + Live),

            // ── 3. 스코프별 설정 ──
            [K("Per scope settings", "Current scope")] = new Text(CatPerScope, "지금 조준 중인 스코프",
                "어떤 스코프인지, 이 모드가 적용 중인지, 전용 설정이 있는지 보여줍니다. 조준한 채로 F12를 열어 보세요."),
            [K("Per scope settings", "Zoom Multiplier")] = new Text(CatPerScope, "확대 배수",
                "이 스코프가 얼마나 당겨질지. 1 = 원래 배율, 2 = 두 배 더 당김, 0.5 = 절반만. 배율 조절 스코프는 전 구간에 같이 적용. 많이 당기면 스코프 몸통도 커 보이니 '총 크기'를 같이 올려 보세요." + PerScope),
            [K("Per scope settings", "Save custom settings key")] = new Text(CatPerScope, "(자동 저장이라 필요 없음)", ""),
            [K("Per scope settings", "DeleteCustomMeshSurgerySettingsKey")] = new Text(CatPerScope, "이 스코프 설정 초기화 키",
                "조준한 채로 누르면 이 스코프에 저장한 값을 지우고 기본값으로 돌아갑니다(화면에 알림)."),
            [K("Per scope settings", "Weapon Scale Min Magnification")] = new Text(CatPerScope, "총 크기 - 가장 낮은 배율",
                "가장 낮은 배율로 볼 때 총 크기. 클수록 총이 작아져서 스코프 몸통이 화면을 덜 가립니다. 기본 1." + PerScope),
            [K("Per scope settings", "Weapon Scale Max Magnification")] = new Text(CatPerScope, "총 크기 - 가장 높은 배율",
                "가장 높은 배율로 볼 때 총 크기. 고배율에서 스코프 몸통이 크게 보이면 올리세요." + PerScope),
            [K("Per scope settings", "Weapon Scale Multiplier")] = new Text(CatPerScope, "총 크기 - 한꺼번에",
                "위 두 값을 한꺼번에 키우거나 줄입니다. 1 = 그대로." + PerScope),
            [K("Per scope settings", "Reticle Size Multiplier")] = new Text(CatPerScope, "조준선 크기",
                "1 = 그대로. 크게 하려면 올리세요." + PerScope),
            [K("Per scope settings", "Reticle Size")] = new Text(CatPerScope, "조준선 크기 (기준값)",
                "보통은 위 '조준선 크기'로 맞추세요." + PerScope + Rarely),
            [K("Per scope settings", "Variable Reticle Minimum Size")] = new Text(CatPerScope, "조준선 - 낮은 배율일 때 크기",
                "배율 따라 조준선 크기가 바뀌는 일부 스코프만 해당. 아래 값과 둘 다 0이 아니어야 작동." + PerScope + Rarely),
            [K("Per scope settings", "Variable Reticle Maximum Size")] = new Text(CatPerScope, "조준선 - 높은 배율일 때 크기",
                "배율 따라 조준선 크기가 바뀌는 일부 스코프만 해당. 위 값과 둘 다 0이 아니어야 작동." + PerScope + Rarely),
            [K("Per scope settings", "Vignette Opacity")] = new Text(CatPerScope, "렌즈 테두리 - 진하기",
                "0 = '2. 화면 효과' 값 그대로." + PerScope),
            [K("Per scope settings", "Vignette Radius")] = new Text(CatPerScope, "렌즈 테두리 - 두께",
                "0 = '2. 화면 효과' 값 그대로. 작을수록 가운데까지 어두워짐." + PerScope),
            [K("Per scope settings", "Vignette Softness")] = new Text(CatPerScope, "렌즈 테두리 - 번짐",
                "0 = '2. 화면 효과' 값 그대로." + PerScope),
            [K("Per scope settings", "Visual Recoil Compensation")] = new Text(CatPerScope, "쏠 때 화면 흔들림 보정",
                "0 = 끔. 쏠 때 조준선과 렌즈가 어긋나 보이면 조금씩 바꿔 보세요(음수 = 반대 방향)." + PerScope + Rarely),

            // The "hole": the mod cuts a tunnel through the scope body so you can see through it.
            [K("General", "Automatic Cut Shape")] = new Text(CatPerScope, "몸통 구멍 자동 계산 (모든 스코프)",
                "켜면 렌즈 크기를 재서 구멍을 자동으로 뚫습니다: 렌즈 크기만큼의 통로가 앞쪽으로 조금씩만 넓어지는 모양이라 스코프 안쪽은 지워지고 겉모양은 남습니다. 끄면 모든 스코프가 아래 수동 값을 씁니다." + Live),
            [K("Per scope settings", "Use Manual Cut Shape")] = new Text(CatPerScope, "이 스코프는 수동 구멍 사용",
                "켜면 이 스코프만 자동 계산 대신 아래 '몸통 구멍 - …' 수동 값을 씁니다." + PerScopeCut),
            [K("Per scope settings", "Cut Width Multiplier")] = new Text(CatPerScope, "몸통 구멍 - 넓이",
                "자동 구멍: 앞쪽으로 갈수록 넓어지는 정도(1 = 기본, 0.2 = 거의 곧은 통로, 2 = 두 배로 넓게). 수동 구멍: 스코프 앞쪽 구멍 크기 배수. 겉모양이 잘려 보이면 줄이고, 렌즈 너머에 총 앞부분이 걸려 보이면 늘리세요." + PerScopeCut),
            [K("Per scope settings", "CutLength")] = new Text(CatPerScope, "몸통 구멍 - 깊이",
                "이 모드는 스코프 몸통에 구멍을 뚫어 그 사이로 봅니다. 렌즈 안에 몸통 안쪽(검은 원통)이 보이면 늘리세요." + PerScopeCut),
            [K("Per scope settings", "Plane1Radius")] = new Text(CatPerScope, "몸통 구멍 - 눈 쪽 크기",
                "구멍은 눈 쪽에서 앞쪽 끝까지 점점 넓어지는 깔때기 모양입니다. 이건 눈 쪽 입구 크기. 클수록 넓게 뚫림." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane2Position")] = new Text(CatPerScope, "몸통 구멍 - 중간1 위치",
                "깔때기 중간 지점1이 어디쯤인지. 0 = 눈 쪽, 1 = 앞쪽 끝." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane2Radius")] = new Text(CatPerScope, "몸통 구멍 - 중간1 크기",
                "그 지점의 구멍 크기. 클수록 넓게 뚫림." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane3Position")] = new Text(CatPerScope, "몸통 구멍 - 중간2 위치",
                "0 = 눈 쪽, 1 = 앞쪽 끝." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane3Radius")] = new Text(CatPerScope, "몸통 구멍 - 중간2 크기",
                "클수록 넓게 뚫림." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane4Position")] = new Text(CatPerScope, "몸통 구멍 - 앞쪽 끝 위치",
                "보통 1." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane4Radius")] = new Text(CatPerScope, "몸통 구멍 - 앞쪽 끝 크기",
                "구멍 맨 앞쪽 크기. 클수록 넓게 뚫림." + PerScopeCut + Rarely),
            [K("Per scope settings", "CutStartOffset")] = new Text(CatPerScope, "몸통 구멍 - 뚫기 시작점",
                "렌즈보다 얼마나 눈 쪽에서부터 뚫을지. 눈앞에 몸통이 걸리면 늘리세요." + PerScopeCut + Rarely),
            [K("Per scope settings", "NearPreserveDepth")] = new Text(CatPerScope, "몸통 구멍 - 눈앞 테두리 남기기",
                "눈 바로 앞의 스코프 테두리를 얼마나 남길지. 0 = 안 남김." + PerScopeCut + Rarely),
            [K("Per scope settings", "PlaneOffsetMeters")] = new Text(CatPerScope, "몸통 구멍 - 미세 조정1",
                "아주 작은 위치 조정. 거의 안 씀." + PerScopeCut + Rarely),
            [K("Per scope settings", "Plane1OffsetMeters")] = new Text(CatPerScope, "몸통 구멍 - 미세 조정2",
                "눈 쪽 입구를 앞뒤로 아주 조금 옮김. 거의 안 씀." + PerScopeCut + Rarely),
            [K("Per scope settings", "ExpandSearchToWeaponRoot")] = new Text(CatPerScope, "몸통 구멍 - 마운트 등도 같이 뚫기",
                "켜면 구멍에 걸리는 마운트·총 부품도 같이 뚫습니다." + PerScopeCut + Rarely),

            // ── 4. 총 움직임 ──
            [K("Hacks", "Keep scope centered with zeroing")] = new Text(CatMotion, "영점 바꿔도 스코프는 화면 가운데",
                "영점(거리)을 바꿔도 스코프가 화면 아래로 밀리지 않습니다. 대신 조준선이 맞는 위치로 살짝 움직입니다." + Live),
            [K("Hacks", "Bypass during reload")] = new Text(CatMotion, "재장전할 땐 잠깐 끄기",
                "재장전하는 동안 이 모드를 잠깐 꺼서 이상한 모습이 안 보이게 합니다." + Live),
            [K("Hacks", "Reload Bypass Modifier")] = new Text(CatMotion, "재장전할 때 꺼지는 시간",
                "클수록 짧게 꺼집니다." + Live + Rarely),
            [K("Hacks", "Scale Sway With Camera FOV")] = new Text(CatMotion, "많이 당길수록 총 흔들림 줄이기",
                "고배율에서 화면이 너무 흔들리지 않게 합니다." + Live),
            [K("Hacks", "Sway Modifier")] = new Text(CatMotion, "흔들림 줄이는 정도",
                "클수록 더 많이 줄입니다." + Live),
            [K("Hacks", "Suppress Fire Mode Switch Movement")] = new Text(CatMotion, "조정간 바꿀 때 총 안 움직이기",
                "조정간은 바뀌고, 손으로 만지는 동작만 생략합니다." + Live),
            [K("Hacks", "Suppress Magnification Switch Movement")] = new Text(CatMotion, "배율 바꿀 때 총 안 움직이기",
                "배율은 바뀌고, 손으로 돌리는 동작만 생략합니다." + Live),
            [K("Hacks", "Force Recoil Return To Zero")] = new Text(CatMotion, "반동 후 총이 정확히 제자리로",
                "쏜 뒤 총이 원래 위치로 정확히 돌아오게 합니다." + Live + Rarely),
            [K("Hacks", "Bypass During Stance Transitions")] = new Text(CatMotion, "앉기·엎드리기 중엔 잠깐 끄기",
                "자세를 바꾸는 동안 이 모드를 잠깐 끕니다." + Live + Rarely),
            [K("Hacks", "ADS Scope Alignment Angle Tolerance")] = new Text(CatMotion, "달리기 후 조준 - 정렬 기다리는 정도",
                "달리다 조준하면 스코프가 눈앞에 거의 맞춰질 때까지 기다렸다가 켜집니다. 클수록 덜 기다림." + Live + Rarely),
            [K("Hacks", "Post Sprint ADS Gate Duration")] = new Text(CatMotion, "달리기 후 조준 - 기다리는 시간",
                "달린 뒤 몇 초 동안 위 기다림을 적용할지." + Live + Rarely),
            [K("Hacks", "Post Stance ADS Gate Duration")] = new Text(CatMotion, "자세 바꾼 후 조준 - 기다리는 시간",
                "자세를 바꾼 뒤 몇 초 동안 위 기다림을 적용할지." + Live + Rarely),
            [K("Hacks", "Manual Weapon Scale")] = new Text(CatMotion, "(옛 설정) 총 크기", "" + Rarely),
            [K("Hacks", "Visual Recoil Compensation")] = new Text(CatMotion, "(옛 설정) 쏠 때 화면 흔들림 보정", "" + Rarely),

            // ── 5. 그래픽 ──
            [K("Graphics", "Manual LOD Bias")] = new Text(CatGraphics, "멀리 있는 물체 디테일 (직접 지정)",
                "스코프로 볼 때 멀리 있는 물체를 얼마나 자세히 그릴지. 0 = 배율에 맞춰 자동. 높이면 FPS가 떨어집니다." + Live),
            [K("Graphics", "Auto LOD bias multiplier")] = new Text(CatGraphics, "멀리 있는 물체 디테일 (자동일 때 세기)",
                "자동일 때 '배율 × 이 값'만큼 자세히 그립니다. 높이면 FPS가 떨어집니다." + Live),
            [K("Graphics", "Keep scoped LOD Bias until inventory is opened")] = new Text(CatGraphics, "조준 풀어도 멀리 디테일 유지",
                "조준을 풀어도 인벤토리를 열 때까지 멀리 있는 물체를 자세히 그립니다. 조준할 때마다 멀리 물체가 바뀌어 보이면 켜 보세요." + Live),

            // ── 6. 다른 모드 호환 ──
            [K("Compatibility", "COTI thermal only inside lens")] = new Text(CatCompat, "COTI 열상은 렌즈 안에만",
                "COTI(클립온 열상 모드)를 쓸 때 열상이 스코프 렌즈 안에만 보이게 합니다." + Live),
            [K("Compatibility", "COTI thermal centred in scope")] = new Text(CatCompat, "COTI 열상 원을 스코프 가운데로",
                "COTI 열상의 동그란 화면을 스코프 가운데에 맞춥니다. 위 설정이 켜져 있어야 합니다." + Live),
            [K("Compatibility", "COTI lens-only flip Y")] = new Text(CatCompat, "COTI - 렌즈 바깥 위아래 뒤집기",
                "COTI를 켰을 때 렌즈 바깥 화면이 위아래로 뒤집혀 보이면 켜세요." + Live + Rarely),

            // ── 7. (hidden) global hole defaults ──
            [K("Global Mesh Surgery settings", "PlaneOffsetMeters")] = new Text(CatMesh, "미세 조정1", ""),
            [K("Global Mesh Surgery settings", "Plane1Radius")] = new Text(CatMesh, "눈 쪽 크기", ""),
            [K("Global Mesh Surgery settings", "Plane1OffsetMeters")] = new Text(CatMesh, "미세 조정2", ""),
            [K("Global Mesh Surgery settings", "Plane2Position")] = new Text(CatMesh, "중간1 위치", ""),
            [K("Global Mesh Surgery settings", "Plane2Radius")] = new Text(CatMesh, "중간1 크기", ""),
            [K("Global Mesh Surgery settings", "Plane3Position")] = new Text(CatMesh, "중간2 위치", ""),
            [K("Global Mesh Surgery settings", "Plane3Radius")] = new Text(CatMesh, "중간2 크기", ""),
            [K("Global Mesh Surgery settings", "Plane4Position")] = new Text(CatMesh, "앞쪽 끝 위치", ""),
            [K("Global Mesh Surgery settings", "Plane4Radius")] = new Text(CatMesh, "앞쪽 끝 크기", ""),
            [K("Global Mesh Surgery settings", "CutStartOffset")] = new Text(CatMesh, "뚫기 시작점", ""),
            [K("Global Mesh Surgery settings", "CutLength")] = new Text(CatMesh, "깊이", ""),
            [K("Global Mesh Surgery settings", "NearPreserveDepth")] = new Text(CatMesh, "눈앞 테두리 남기기", ""),
            [K("Global Mesh Surgery settings", "ExpandSearchToWeaponRoot")] = new Text(CatMesh, "마운트 등도 같이 뚫기", ""),
            [K("Global Mesh Surgery settings", "Reticle Base Size")] = new Text(CatMesh, "조준선 크기 (기준값)", ""),
            [K("Global Mesh Surgery settings", "Mesh Reticle Min Scale")] = new Text(CatMesh, "입체 조준선 최소 크기", ""),
            [K("Global Mesh Surgery settings", "Mesh Reticle Max Scale")] = new Text(CatMesh, "입체 조준선 최대 크기", ""),
            [K("Global Mesh Surgery settings", "Mesh Reticle Normalized Scale")] = new Text(CatMesh, "입체 조준선 크기", ""),
            [K("Global Mesh Surgery settings", "Mesh Reticle Minimum Stroke Enabled")] = new Text(CatMesh, "입체 조준선 최소 두께 보장", ""),
            [K("Global Mesh Surgery settings", "Mesh Reticle Minimum Stroke Pixels")] = new Text(CatMesh, "입체 조준선 최소 두께", ""),

            // ── 8. 문제 확인용 ──
            [K("Debug", "Debug logging")] = new Text(CatDebug, "자세한 기록 남기기",
                "문제를 알려줄 때 켜고, 게임 후 BepInEx\\LogOutput.log 파일을 보내 주세요." + Live),
            [K("Debug", "DebugShowHousingMask")] = new Text(CatDebug, "렌즈 영역을 빨갛게 표시",
                "모드가 렌즈로 인식한 곳을 빨간색으로 보여줍니다." + Live + Rarely),
            [K("Debug", "DebugShowScopeShadowMask")] = new Text(CatDebug, "검게 덮는 영역을 초록색으로 표시",
                "'렌즈 바깥 검게'가 덮는 곳을 초록색으로 보여줍니다." + Live + Rarely),
            [K("Debug", "Draw reticle after everything")] = new Text(CatDebug, "(옛 설정, 효과 없음)", ""),
        };

        // Advanced (hidden) in the original, shown here because they are the ones people tune.
        private static readonly HashSet<string> Visible = new HashSet<string>
        {
            K("Per scope settings", "DeleteCustomMeshSurgerySettingsKey"),
            K("Per scope settings", "Zoom Multiplier"),
            K("Per scope settings", "Weapon Scale Min Magnification"),
            K("Per scope settings", "Weapon Scale Max Magnification"),
            K("Per scope settings", "Weapon Scale Multiplier"),
            K("Per scope settings", "CutLength"),
            K("Per scope settings", "Cut Width Multiplier"),
            K("Per scope settings", "Use Manual Cut Shape"),
            K("General", "Auto Disable For NV/Thermals"),
            K("General", "Baseline FOV"),
        };

        // Hidden so per-scope values have one place to edit: the global cut/scale fallbacks
        // (still used by scopes without saved settings) and the save key (saving is automatic).
        private static readonly HashSet<string> Hidden = new HashSet<string>
        {
            K("Per scope settings", "Save custom settings key"),
            K("Hacks", "Manual Weapon Scale"),
            K("Hacks", "Visual Recoil Compensation"),
            K("Debug", "Draw reticle after everything"),
        };

        private static string K(string section, string key) => section + "\u0001" + key;

        public static void Apply(IEnumerable<ConfigEntryBase> entries)
        {
            foreach (var entry in entries)
            {
                if (entry?.Definition == null || entry.Description == null) continue;
                string key = K(entry.Definition.Section, entry.Definition.Key);
                if (!Map.TryGetValue(key, out var text)) continue;

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

                if (Visible.Contains(key))
                    attributes.IsAdvanced = false;
                if (Hidden.Contains(key) || entry.Definition.Section == "Global Mesh Surgery settings")
                    attributes.Browsable = false;
                attributes.DispName = text.Name;
                attributes.Category = text.Category;
                attributes.Description = text.Desc.TrimStart('\n');
            }
        }
    }
}
