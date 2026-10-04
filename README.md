# PiP-Disabler-zzap--Bootleg- (SPT 4.1 포팅)

---

### ⚠️ IMPORTANT NOTICE / DISCLAIMER

**Original Author:** Fiodorwellfme (Fiodor)
**Original Repository:** PiP-Disabler
**Original Link:** https://github.com/Fiodorwellfme/PiP-Disabler
**License notation:** MIT License — Copyright (c) 2026 Fiodorwellfme (see `LICENSE`)

1. **Reflection & Take-Downs:** I deeply reflect on the ECOT incident. As an AI-assisted "vibe coder," I will immediately delete files if the original authors ask.
2. **No Re-Distribution:** These ported builds are unverified, temporary fixes. Please do NOT re-upload or share them anywhere else.
3. **Do Not Pester Original Authors:** Never report bugs or pester original modders regarding issues from my unofficial ports.
4. **Full Credit & Respect:** I will always credit original creators on GitHub and prioritize their decisions above all else.
5. **Support Original Creators:** Instead of using my ports, please visit the original authors' Forge pages to leave kind words or tips.

---

## 변경 이력

- 2026-10-05 05:09 — **2.0.9**: 사용자 스크린샷·제보 기반 수정 3건.
  - **COTI 켜고 줌할 때 스코프 바깥 시야가 위아래로 뒤집혀 보이던 문제 수정**: 그래픽 방식(DirectX 등)에 맞춰 방향을 자동으로 맞춤. `COTI lens-only flip Y`는 이제 "자동값을 반대로"라는 뜻(평소엔 끄면 됨).
  - **영점(사거리)을 올리거나 내리면 스코프가 화면 중앙에서 위아래로 밀리던 문제 수정**: 게임은 영점을 바꿀 때 스코프 속 카메라를 기울이는데, 이 모드가 화면 전체를 그 기울기에 맞춰 돌려서 스코프 몸통이 밀렸음. 이제 화면은 스코프의 실제 축에 맞추고, 조준선을 영점 위치(탄이 맞는 곳)로 옮겨 그림. 그래서 영점을 크게 바꾸면 조준선이 렌즈 안에서 조금 움직임. 설정: F12 → `Hacks` → `Keep scope centered with zeroing`(끄면 예전 방식).
  - **Epic's AiO 가변 배율 스코프 조준선이 가로로 늘어나 보이던 문제 수정**(원작자도 원인을 몰라 남겨 둔 비호환): 에픽 조준선은 회전값이 (-90, 90, 90)인데, 화면 비율 보정을 회전 전 엉뚱한 축에 걸어서 16:9 화면에서 약 1.78배 넓게 그려졌음. 보정을 회전 후 화면 가로에 걸도록 바꿈. 기존 스코프 조준선은 계산 결과가 똑같아 변화 없음.
- 2026-10-05 03:43 — **2.0.8**: 원작 최신 main(2026-10-04, 원작자의 4.1 재정리판)을 검토해서 쓸 만한 부분만 반영.
  - 반영: 무기 크기가 줌 애니메이션을 한 박자 늦게 따라가던 것 → FOV가 바뀔 때마다 바로 갱신(원작 "Update scaling sooner").
  - 반영: `FOV Fix Behaviour`를 켰을 때 원작의 새 동작(배율 계산 기준은 50°로 고정, 1배율로 전환하면 게임 설정 FOV로 복귀, 그에 맞춘 무기 크기 보정).
  - 반영 안 함(원작 쪽 4.1 버그로 판단): 1배율·조준 해제 FOV 계산에 블라인드파이어 값(0/1)을 FOV로 쓰는 부분, 조준경 교체 시 몸통 절단 캐시 갱신이 끊기는 이벤트 처리, 조준 모드 변경 패치 대상이 모호한 부분, 모드 끄기 단축키 정리 코드 비활성화.
- 2026-10-05 01:01 — **2.0.7**: COTI를 켜고 줌하면 렌즈 바깥이 단색 회색으로 칠해지던 문제 수정.
  - 원인: 2.0.5의 "열상 렌즈 안에만" 기능이 렌즈 바깥을 되돌릴 때 쓰는 화면 복사본을 임시 텍스처로 만들었는데, 게임에서 이 텍스처가 연결되지 않아 Unity 기본값인 회색이 그려졌음.
  - 수정: 복사본을 모드가 직접 들고 있는 고정 텍스처로 바꾸고 셰이더에 직접 연결함. 처음 작동할 때 LogOutput.log에 `COTI lens-only engaged` 진단 줄을 한 번 남김.
  - 그래도 이상하면 F12 → `Compatibility` → `COTI thermal only inside lens`를 끄면 됨(2.0.4 방식으로 동작).
- 2026-10-04 13:28 — **2.0.6**: COTI 열상에도 렌즈 가장자리 비네팅 적용.
  - 그동안 렌즈 안쪽 가장자리를 어둡게 하는 비네팅이 COTI 열상보다 먼저 그려져서, 가장자리에서도 열상만 밝게 남았음.
  - COTI 열상이 있을 때는 비네팅을 열상 **다음에** 그려서 열상도 똑같이 가장자리가 어두워짐. 조준선은 여전히 맨 위에 그려짐.
  - COTI가 없으면 예전과 똑같이 동작함.
- 2026-10-04 13:25 — **2.0.5**: COTI 열상을 스코프 렌즈 안에만 보이게 하는 옵션 추가 (기본 켜짐).
  - COTI가 원래 스코프 화면(PiP)과 함께 쓸 때처럼, 배율 스코프로 조준하면 열상이 **렌즈 안에만** 보임. 렌즈 바깥(스코프 몸통·주변 시야)에는 열상이 나오지 않음.
  - 방식: COTI가 열상을 더하기 직전의 화면을 복사해 두었다가, COTI가 그린 뒤 렌즈 바깥만 그 복사본으로 되돌림. 조준선은 계속 맨 위에 그려짐.
  - 설정: F12 → `PiP-Disabler` → `Compatibility` → `COTI thermal only inside lens`. 끄면 2.0.4 방식(렌즈 바깥 열상을 그림자로만 어둡게)으로 동작함.
  - 렌즈 바깥 화면이 위아래로 뒤집혀 보이면 같은 항목의 고급 설정 `COTI lens-only flip Y`를 켜 주세요(게임 안에서 확인하지 못한 부분이라 대비용으로 넣음).
- 2026-10-04 13:20 — **2.0.4**: COTI 열상이 스코프 바깥 검은 그림자 위로 비치던 문제 수정.
  - 원인: 이 모드의 "렌즈 바깥 그림자"는 화면을 그리는 중간 단계에서 칠하는데, COTI 열상은 맨 마지막에 그 위로 더해져서 어두운 테두리 위에 열원이 밝게 보였음.
  - 수정: COTI 열상이 화면에 있을 때만 그림자를 COTI 열상 **다음에** 칠함. 그래서 렌즈 바깥 열상도 장면과 똑같이 어두워짐(그림자 투명도 설정 그대로 적용). COTI가 없으면 예전과 똑같이 동작함.
  - 비네팅(렌즈 안쪽 가장자리)과 렌즈 바깥 흐림은 그대로 둠. 조준선 위를 덮지 않게 하기 위해서임.
- 2026-10-04 13:15 — **2.0.3**: COTI 3.2.0 대응 점검.
  - COTI 3.2.0은 열상 카메라가 NVG 원 부분만 잘라서 렌더하도록(전용 투영값) 바뀜. 2.0.2에서 넣은 "줌 전환 중 열상 카메라 FOV 맞추기"가 이 잘라내기를 건드릴 수 있어서 뺌. 열상 방향을 맞추는 수정(2.0.2의 핵심)은 그대로 유지.
- 2026-10-02 01:43 — **2.0.2**: COTI(Clip-On Thermal Imager) 호환 수정.
  - 스코프로 조준할 때 COTI 열상 윤곽이 실제 적 위치와 어긋나고 흔들리던 문제를 고침. 이 모드는 조준 중 렌더 직전에 화면을 스코프 축에 맞춰 돌리는데, COTI 열상 카메라는 그보다 먼저 그려져서 돌리기 전 방향을 찍고 있었음. 이제 메인 카메라에 붙은 카메라(COTI 열상)가 그려지기 전에 정렬을 먼저 적용함.
  - 줌이 들어가고 빠지는 동안 열상 크기가 한 프레임 늦게 따라오던 것도 맞춤.
- 2026-10-02 01:33 — **2.0.1**: SPT 4.1 포팅 + 스코프 버그 수정.
  - 원작자가 WIP 브랜치에 올린 공식 4.1 코드(v2.0.0)를 기반으로 삼음. 먼저 4.1로 포팅해 주신 Sierra 님 DLL을 디컴파일해 비교했고, 그쪽에서 맞게 고친 부분을 모두 반영함.
  - **조준 중 조준선·비네팅이 갑자기 사라져 다시 조준해야 돌아오던 문제 수정**: 지금 보고 있는 스코프가 아닌 다른 스코프가 꺼질 때도 조준선을 지우던 코드를 고침. 혹시 다른 이유로 사라져도 몇 프레임(약 0.25초) 안에 다시 켜지는 안전장치를 넣음.
  - **다른 스코프(봇·바닥에 떨어진 총 등)가 켜질 때 내 스코프 대신 그 스코프로 바뀌던 경로 차단**: 내 총에 달린 스코프의 신호만 받음.
  - **일부 일반 스코프가 야간투시/열상으로 잘못 판정돼 모드가 꺼지던 경로 수정**: 게임과 같은 기준(부품이 있고 실제로 켜져 있을 때만)으로 판정함.
  - 조준을 풀 때 총 크기가 줌 상태의 FOV 기준으로 계산되던 문제를 고침. 이제 게임 설정 FOV로 계산함.
  - 조준 중 LOD(먼 물체의 디테일) 하한값이 계속 올라가기만 하던 문제를 고침. 이제 게임 그래픽 설정값을 기준으로 함.
  - 원작 4.1 코드에서 잘못된 클래스를 가리켜 적용되지 않던 패치(`method_10`, 바닐라 스코프 화면을 켜는 부분)를 바로잡음.
  - 4.1에서 새로 생긴 조준 모드 변경 방식(`ChangeAimingMode(int)`)에도 대응함.
  - 조준 진입 중 오류가 나면 반쯤 적용된 상태로 멈추지 않고 원래대로 되돌린 뒤 1초 뒤에 다시 시도함.
  - 패치 적용에 실패하면 디버그 설정과 상관없이 LogOutput.log에 항상 기록함.

## 이 모드가 하는 일

배율 스코프로 조준하면 원래 게임은 화면을 **두 번** 그립니다. 하나는 평소 화면이고, 하나는 렌즈 속 확대 화면(PiP)입니다. 이 모드는 렌즈 속 화면을 끄고 다음과 같이 대신 처리해서 조준할 때 떨어지는 FPS를 줄입니다.

- 메인 카메라 자체를 배율만큼 확대(FOV 축소)
- 시야를 가리는 스코프 몸통 모델을 실시간으로 잘라냄
- 조준선(레티클)을 화면 중앙에 따로 그림

자동으로 제외되는 스코프는 다음과 같습니다(원래 게임 방식으로 표시).

- 야간투시·열상 스코프 (`Auto Disable For NV/Thermals`)
- 거리 측정기가 달린 스코프
- `Auto Bypass Name Contains`에 이름이 들어간 스코프 (기본값: `d-evo; scope_ags_npz_pag17_2,7x`)
- 블랙리스트에 넣은 스코프, 또는 화이트리스트를 쓸 때 목록에 없는 스코프

## 설치

1. `PiP-Disabler` 폴더를 `E:\SPT 4.1\BepInEx\plugins\`에 넣습니다. 들어갈 파일은 다음과 같습니다.
   - `PiP-Disabler.dll`
   - `custom_mesh_surgery_settings.json` (스코프별 설정)
   - `pipdisabler_reticle_shaders.bundle`, `pipdisabler_effect_shaders.bundle`
2. Sierra 님 포팅판이나 예전 버전이 있으면 그 DLL은 지웁니다(같은 모드가 두 번 로드되면 안 됩니다).
3. 게임에서 F12 → `PiP-Disabler`에서 설정합니다.

**Fontaine's FOV Fix와는 같이 쓰지 않는 것을 권장합니다**(원작 안내). 대신 `General → FOV Fix Behaviour`를 켜면 1배율 FOV를 게임 설정 FOV로 맞춥니다.

## 직접 빌드 (사용자 PC)

```
dotnet build -c Release
```

- 기본 SPT 경로는 `E:\SPT 4.1`입니다. 다르면 `dotnet build -c Release -p:SPTPath="D:\내 SPT"`로 지정합니다.
- 빌드하면 `E:\SPT 4.1\BepInEx\plugins\PiP-Disabler\`에 바로 복사되고, `Releases\PiP-Disabler.zip`이 만들어집니다.
- `custom_mesh_surgery_settings.json`은 대상 폴더에 **없을 때만** 복사합니다. 게임에서 저장한 스코프별 설정을 덮어쓰지 않기 위해서입니다.

## 버그 제보 방법

1. F12 → `PiP-Disabler` → `Debug` → `Debug logging` 켜기
2. 문제가 나는 스코프로 레이드에서 재현
3. `E:\SPT 4.1\BepInEx\LogOutput.log` 파일과 **스코프 이름**을 보내 주세요.
   - 로그에 `Bypassing mod for current scope (...)`가 있으면 모드가 일부러 끈 스코프입니다. 괄호 안에 이유가 나옵니다.
   - `Thermal/NV auto-bypass match`, `AutoBypassNameContains match`, `Whitelist bypass`, `Rangefinder bypass`도 같은 뜻입니다.
   - `[Patcher] Failed to enable ...`, `Scope enter failed`는 오류이니 꼭 같이 보내 주세요.
4. 제보가 끝나면 `Debug logging`을 다시 끄세요. 켜 두면 로그가 많이 쌓입니다.

> 원작자분께는 이 포팅판 관련 문의를 하지 말아 주세요(위 Disclaimer 3번).

## 알려진 한계 (원작과 같음)

- 가변 배율 스코프는 지원 범위가 제한적입니다.
- 배율 값은 아이템 데이터(`Zooms`)에서 읽어서, 원래 게임 렌즈 화면과 약간 다를 수 있습니다.
- 스코프마다 몸통 절단·조준선 크기 설정이 필요할 수 있습니다(`custom_mesh_surgery_settings.json`, F12의 Mesh Surgery 항목에서 조절 후 저장 키).

## 원작 설명 (영문, 원문 유지)

<details>
<summary>펼치기</summary>

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

## FPS 비교 (원작 스크린샷)

![PiP](https://github.com/Fiodorwellfme/PiP-Disabler/blob/main/images/Screenshot%202026-03-07%20173054.png "Picture in Picture Scope")
![PiP-Disabler](https://github.com/Fiodorwellfme/PiP-Disabler/blob/main/images/Screenshot%202026-03-07%20173102.png "PiP-Disabler")
