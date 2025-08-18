# 프로젝트 설명

<summary>프로젝트 개요</summary>

- **프로젝트명** : Unity Practice Project – 콩콩던전 Clone  
- **개발 기간** : 2025.06.19 ~ 진행 중  
- **참여 인원 / 역할** : 1인 개발 (기획, 클라이언트, UI/UX, 데이터 구조 설계, 최적화 등 전 영역 담당)  
- **레퍼런스** : [콩콩던전 (Google Play)](https://play.google.com/store/apps/details?id=com.game.kingrush&hl=ko&pli=1)  
- **프로젝트 설명** :  
  원작 *콩콩던전*은 **캐주얼 판타지 타워 디펜스 게임**으로, 플레이어가 다양한 타워를 배치하고 업그레이드하여 몰려오는 적 웨이브를 막아내는 방식의 게임입니다.  
  본 프로젝트에서는 해당 게임을 **클론 개발**하며, 실제 상용 게임 구조를 분석·재현하는 과정을 통해 아래와 같은 목표를 달성하는 것을 중점으로 합니다:  
  - **모바일 환경 최적화** : 오브젝트 풀링, Draw Call 최소화, 메모리 관리, GC 최소화를 적용하여 원활한 퍼포먼스를 유지하는 방법을 학습  
  - **데이터 관리 체계화** : 에셋 기반 런타임 데이터 로딩, BigNum 구조 적용, Addressables 활용 등 **대규모 데이터 관리 방식** 실습  
  - **실전 감각 강화** : 실제 상용 게임의 흐름(전투 루프, 업그레이드, 보상 시스템)을 구조적으로 분석하고 재구성하여 **실제 개발 파이프라인 경험 축적**  


<summary>기술 스택</summary>

- **엔진 / 언어** : Unity 2022.3.15f1, C#  

- **툴 / 에셋 활용** :  
  - **Odin Inspector (유료)** : 인스펙터 확장 및 디버깅 효율 향상  
    - GitHub 업로드 시 의존성 문제를 피하기 위해 **Stub 코드 및 Define Symbol 처리**를 적용 → 에셋 미보유 환경에서도 컴파일 오류 발생하지 않도록 관리  
  - **Gamebase Setting Tool / Game Package Manager** : 프로젝트 초기 설정 및 패키지 관리 자동화  
  - **무료 에셋 (UI/캐릭터/사운드 등)** : 학습 목적의 빠른 프로토타입 제작에 활용 (아래 🎨 사용 에셋 섹션 참고)  

- **데이터 관리 방식** :  
  - [Unity Google Sheets](https://shlifedev.gitbook.io/unitygooglesheets) 오픈소스 라이브러리 활용  
    - Google Sheets 데이터를 스크립트로 변환 → 런타임에서 활용  
    - 데이터 변경 시 자동 반영 가능, 협업 및 유지보수 효율 향상  
  - 대규모 성장 수치 처리를 위해 **BigNum 구조체** 설계 및 UI 반영  

- **아키텍처 지향점** :  
  - ScriptableObject 중심보다는 **코드 기반 데이터 관리 구조**  
  - 전투, UI, 데이터, 리소스 관리 등 **모듈 단위 설계**로 확장성과 유지보수성 강화  

- **플랫폼 빌드 경험** : Android (모바일 환경 최적화 실습 중점)  


<summary>주요 구현 기능</summary>

- **데이터 처리 및 최적화**  
  - [Unity Google Sheets Integration](https://shlifedev.gitbook.io/unitygooglesheets) 오픈소스 활용 → Google Sheets 데이터를 스크립트로 변환하여 관리  
  - **BigNum 구조체 설계**  
    - 기본 자료형(int, long, double)에서 변환 가능하도록 지원  
    - M(가수) + E(지수) 구조로 표현하여 **아주 큰 값도 안정적으로 연산/표현 가능**  
    - 게임 내 성장 곡선, 골드, 데미지 등 **대규모 수치 처리 및 UI 반영**에 활용  
  - **오브젝트 풀링** 및 GC 최소화 → 모바일 환경 최적화  

- **디버깅 및 개발 편의성**  
  - **Define Symbol 제어**를 통해 디버깅 모드 On/Off 관리  
  - Odin Inspector + 커스텀 인스펙터 활용하여 **런타임 데이터 확인 및 튜닝 편의성** 강화  

- **이벤트 시스템**  
  - `Dictionary<EEventType, Delegate>` 기반 **EventManager** 설계  
  - 이벤트 구독/해제/실행을 통합 관리하여 모듈 간 의존성 최소화  

- **서버 연동 (예정)**  
  - **PlayFab** 연동 계획 → 플레이어 데이터 저장/로드 및 서버 검증 구조 학습  

- **전투 시스템**  
  - **웨이브 기반 적 스폰** 로직 구현  
  - **데이터 기반 스탯 및 보상 관리** (Google Sheets 연동 데이터 활용)  
  - **영웅 소환 및 스킬 강화 기능** 구현  

<summary>개인 기여도</summary>

- 1인 개발 프로젝트로, 기획을 포함한 전체 클라이언트 개발을 직접 담당  
- 레퍼런스 게임을 기반으로 구현에 집중하여 학습 효율 극대화  
- 아트 리소스는 주로 무료 에셋을 활용, 일부 간단한 아이콘은 AI 제작 도구 보조 활용  

<summary>성과 및 차별화 포인트</summary>

- **BigNum 구조체 필요성**  
  - 문제 : 게임 내 성장 수치가 int/long 범위를 초과하는 구간에서 계산 오류 발생  
  - 해결 : M(가수) + E(지수) 구조를 가진 BigNum 구조체를 직접 설계하고, int/long/float/double → BigNum 변환 기능을 추가  
  - 결과 : 초대형 수치(골드, 데미지, 경험치 등)를 안정적으로 처리 가능, UI에도 적용되어 표현 오류 제거  

- **유료 에셋 의존성 문제 (Odin Inspector)**  
  - 문제 : GitHub 업로드 시 Odin Inspector 부재로 인해 컴파일 오류 발생  
  - 해결 : Stub 코드 및 Define Symbol 처리 방식 적용, Odin Inspector가 없어도 정상 빌드 가능하도록 구조 변경  
  - 결과 : 유료 에셋 미보유 환경에서도 프로젝트 공유 및 협업 가능  

- **데이터 관리 효율성**  
  - 문제 : 초기에는 Excel 데이터 관리 시 수동 반영으로 번거롭고 실수 발생 가능  
  - 해결 : [Unity Google Sheets Integration](https://shlifedev.gitbook.io/unitygooglesheets) 오픈소스 라이브러리를 도입하여 Google Sheets → Script 자동 변환 파이프라인 구축  
  - 결과 : 데이터 변경 즉시 반영 가능, 협업 및 유지보수 효율 향상  

<details>
<summary>스크린샷 / 영상</summary>
추후 추가

- **게임 실행 화면**  

- **UI 캡처**  

- **시연 영상 (YouTube 링크 예시)**  
</details>


---

### 🎨 사용 에셋 (Assets Used)

- [2D Casual UI HD](https://assetstore.unity.com/packages/2d/gui/icons/2d-casual-ui-hd-82080) – UI  
- [Cartoon FX Remaster Free](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565) – VFX / 파티클 이펙트  
- [FREE Casual Game SFX Pack](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-144412) – 사운드 효과  
- [Free Casual Music Pack](https://assetstore.unity.com/packages/audio/music/free-casual-music-pack-165757) – 배경 음악  
- [Kawaii Slimes](https://assetstore.unity.com/packages/3d/characters/creatures/kawaii-slimes-158700) – 캐릭터 & 애니메이션  
- [RPG Tiny Hero Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/rpg-tiny-hero-duo-pbr-polyart-118308) – 캐릭터 & 애니메이션  
