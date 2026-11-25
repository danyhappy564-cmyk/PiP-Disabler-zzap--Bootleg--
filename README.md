<p align="center">
  <a href="README.md"><img alt="한국어 (현재 문서)" src="https://img.shields.io/badge/%ED%95%9C%EA%B5%AD%EC%96%B4-%ED%98%84%EC%9E%AC%20%EB%AC%B8%EC%84%9C-1f6feb?style=for-the-badge"></a>
  <a href="README.en.md"><img alt="English" src="https://img.shields.io/badge/English-Read%20in%20English-6e7681?style=for-the-badge"></a>
</p>

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

전체 변경 이력 81개는 [`changelog/`](changelog/) 폴더에 10개씩 나눠 있습니다 (번호가 클수록 최신, 최신 파일: [`CHANGELOG-009.md`](changelog/CHANGELOG-009.md)). 릴리즈별 요약은 [Releases](../../releases).

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

줌 정보가 아직 준비되지 않은 스코프는 준비될 때까지 업데이트를 기다립니다. 총·칼 교체 중 발생할 수 있는 `ScopeZoomHandler.method_2` 반복 오류를 막으며, 준비된 스코프의 배율 계산은 그대로 사용합니다. 이 보호는 F12에서 PiP 대체 기능을 꺼도 유지됩니다.

3.8.1에서 바뀐 동작입니다.

- **손 숨기기가 실제로 작동하도록 수정**: `픽셀 구멍 손 숨기기`를 켜도 조준할 때 앞손·장갑이 렌즈 안에 그대로 보였습니다(원작자 제보). USEC/BEAR 팔 모델에 붙은 시계 유리 때문에 팔 전체가 숨김 대상에서 빠지고 있었습니다. 이제 팔은 렌즈 안에서만 사라지고, 렌즈 밖 팔은 원래 밝기 그대로 보입니다(레이드·은신처 사격장에서 확인). 조준하는 동안 시계 유리만 그려지지 않습니다.

3.8에서 바뀐 동작입니다.

- **Vudu 비네팅 크기 조절**: Vudu 기본 스코프 설정에 들어 있던 비네팅 값(반경 0.27·진하기 1.0)을 지웠습니다. 이제 Vudu도 `화면 효과`의 전체 비네팅 값(진하기·반경·부드러움)을 따르므로 크기를 조절할 수 있습니다. 직접 저장한 스코프별 값(`user-data`)은 그대로 유지되고 그 값이 먼저 적용됩니다.

3.7에서 바뀐 동작입니다.

- **모드가 만드는 파일을 모드 폴더로**: 스코프별 설정·측정한 눈 거리·카메라 거리·사용한 배율 파일이 `BepInEx\config`가 아니라 `BepInEx\plugins\PiP-Disabler\user-data\`에 저장됩니다. 배포 zip에는 이 폴더가 없어서 덮어쓰기 설치해도 지워지지 않습니다. 예전 `BepInEx\config\PiP-Disabler.*` 파일은 게임을 켤 때 새 폴더로 자동으로 옮겨집니다. F12 설정 파일(`.cfg`)은 그대로 `BepInEx\config`에 있습니다(원작자 리뷰 반영).
- **처음 조준할 때 총이 확 당겨지던 것 완화**: 새 총·스코프·장착 위치로 처음 조준하면 잠깐 뒤 눈 거리를 재서 총 크기를 한 번에 바꾸느라 총이 툭 당겨졌습니다. 이제 그 보정을 0.35초에 걸쳐 부드럽게 넣습니다(원작자 리뷰 반영).
- **비네팅 설정 안내**: Vudu 등 기본 스코프 40개는 스코프별 비네팅 값이 들어 있어 `화면 효과`의 전체 비네팅 값이 적용되지 않습니다. F12 설명에 "그 스코프로 조준한 채 `3. 스코프별 설정`에서 바꾸라"는 안내를 넣었습니다.

3.6부터 추가된 동작입니다.

- **픽셀 구멍: 손도 지우기**(기본 켬, 원작자 제안): 픽셀 구멍 방식일 때 내 팔·장갑·시계도 렌즈 원 안에서는 지웁니다. 손이나 소매가 스코프 시야를 가리지 않고, 렌즈 밖에서는 평소대로 보입니다. 1인칭 팔 그림자는 원래처럼 꺼진 채로 둡니다.
- **번역을 파일로 분리**: F12 설정·안내 메시지의 한국어가 코드에서 빠져 `locales\kr.json`에 들어갔습니다. 영어는 모드에 내장되어 있고, 다른 언어는 `locales\<게임 언어 코드>.json` 파일을 만들면 누구나 추가할 수 있습니다.
- **설정 언어 값 변경**: `Game`(기본, 게임 언어의 번역 파일이 있으면 그 언어, 없으면 영어) / `English` / 파일 이름(`kr` 등). 이전 빌드의 `Korean` 값은 없어졌고, 설정 파일에 남아 있으면 `Game`으로 돌아갑니다(게임이 한국어면 그대로 한국어).
- 원작자에게 같은 코드로 PR을 보냈습니다(원작 2.1.0 기준).
- **같은 스코프는 어느 총에서나 같은 크기**(기본 켬): 총마다 레일 위치·마운트가 달라 눈~접안렌즈 거리가 달라서, 같은 스코프도 총에 따라 크게 또는 작게 보였습니다. 조준하고 잠깐 뒤 그 거리를 재서 어느 총이든 `기준 눈 거리 (mm)`(기본 170)에 오도록 총 크기를 자동 보정합니다. 총+스코프+장착 위치마다 한 번 재고 `user-data\eye-distance.txt`에 기억합니다. 스코프별 `총 크기` 설정은 그 위에 그대로 적용됩니다.
- **DLSS·FSR에서 레티클 떨림 제거**: 업스케일러가 매 프레임 화면을 미세하게 흔드는(지터) 값을 레티클 위치 계산에서 뺍니다. DLSS 성능 우선에서 1~3px 떨리던 레티클이 멈춥니다.

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
4. 내가 맞춘 값은 `BepInEx\plugins\PiP-Disabler\user-data\scope-settings.json`에 저장됨. 모드를 업데이트해도 유지되고, 이 파일만 백업하면 됨.

## 설치

1. `PiP-Disabler` 폴더를 `E:\SPT 4.1\BepInEx\plugins\`에 넣습니다. 들어갈 파일은 다음과 같습니다.
   - `PiP-Disabler.dll`
   - `custom_mesh_surgery_settings.json` (스코프별 설정)
   - `pipdisabler_reticle_shaders.bundle`, `pipdisabler_effect_shaders.bundle`
   - `locales\kr.json` (한국어 번역)
2. Sierra 님 포팅판이나 예전 버전이 있으면 그 DLL은 지웁니다(같은 모드가 두 번 로드되면 안 됩니다).
3. 게임에서 F12 → `PiP-Disabler`에서 설정합니다.

**Fontaine's FOV Fix와는 같이 쓰지 않는 것을 권장합니다**(원작 안내). 대신 `General → FOV Fix Behaviour`를 켜면 1배율 FOV를 게임 설정 FOV로 맞춥니다.

## 직접 빌드 (사용자 PC)

```
dotnet build -c Release
```

- 기본 SPT 경로는 `E:\SPT 4.1`입니다. 다르면 `dotnet build -c Release -p:SPTPath="D:\내 SPT"`로 지정합니다.
- Release 빌드는 `bin\Release\net472\PiP-Disabler.dll`과 `Releases\PiP-Disabler.zip`을 만듭니다. 게임 폴더에 자동 복사하지 않습니다.
- 설치할 때는 게임·서버를 종료하고 기존 파일을 백업한 뒤 필요한 파일만 수동 복사하십시오. 사용자 스코프 설정은 `BepInEx\plugins\PiP-Disabler\user-data\scope-settings.json`에 유지됩니다.
- 파일 속성 버전은 3.8.1.0, F12 플러그인 버전은 3.8.1입니다.

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
