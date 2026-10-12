# PiP-Disabler-zzap--Bootleg- (SPT 4.1 포팅)

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

## 변경 이력

- 2026-10-13 04:36 — README 맨 위 원작자 표기를 한 줄씩 나눠 보기 쉽게 정리 (내용 변화 없음)

전체 변경 이력 67개는 [`changelog/`](changelog/) 폴더에 10개씩 나눠 있습니다 (번호가 클수록 최신, 지금 마지막 파일: [`CHANGELOG-007.md`](changelog/CHANGELOG-007.md)).

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

## 스코프별로 맞추는 법 (예: 20배율에서 총이 너무 크게 보일 때)

1. 그 스코프로 **조준한 채로** F12를 엶 → `3. 스코프별 설정` 맨 위 **지금 조준 중인 스코프**에서 이름·상태 확인.
   - "원래 방식(PiP)으로 보는 중"이면 이 모드가 꺼진 스코프라 아래 값이 안 먹힘.
2. 조준한 채로 값을 움직이면 **바로 보이고 자동 저장**됨:
   - 고배율에서 스코프 몸통이 화면을 가림 → **총 크기 - 가장 높은 배율** 올리기(기본 1).
   - 렌즈 안에 몸통 안쪽(원통)이 보임 → **몸통 구멍 - 깊이** 늘리기.
   - 조준선이 크거나 작음 → **조준선 크기**.
   - 더 당기거나 덜 당기고 싶음 → **확대 배수**(1 = 원래, 2 = 두 배). 많이 당기면 몸통도 커 보이니 총 크기를 같이 올리기.
   - 더 세밀한 값(구멍 원 위치/크기 등)은 F12 오른쪽 위 "고급 설정(Advanced settings)"을 켜면 나옴.
3. 처음으로 되돌리려면 `이 스코프 설정 초기화 키`에 키를 지정하고, 조준한 채로 누르기.
4. 내가 맞춘 값은 `BepInEx\config\PiP-Disabler.scope-settings.json`에 저장됨. 모드를 업데이트해도 유지되고, 이 파일만 백업하면 됨.

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
