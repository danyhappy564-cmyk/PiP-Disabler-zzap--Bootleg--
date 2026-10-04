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
