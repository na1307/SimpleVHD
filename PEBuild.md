# PE 빌드하기
SimpleVHD의 핵심은 윈도우 PE입니다. 제어판은 그냥 사용을 쉽게 해주는 도구일 뿐이고 PE에서 거의 모든 작업이 이루어집니다.

이 문서에서는 SimpleVHD를 위한 PE를 만드는 과정을 설명합니다. 만약 ARM64 장치를 사용하고 계시는 분들은 AMD64 버전의 PE만 제공하기 때문에 PE를 직접 빌드하셔야 합니다. 제가 만든 PE가 걱정되거나 ARM64 장치를 사용하고 계시는 분들은 직접 만들어서 쓰시면 됩니다. 이 과정은 제가 SimpleVHD.wim 파일을 만든 것과 **100% 동일한 방법**입니다.

글로만 설명하겠습니다. 이 글은 초보를 위한 것이 아닙니다.

## 준비물
1. Windows ADK 및 PE 추가 기능<br/>어떤 버전을 쓰든 상관 없습니다. 다만 ARM64 버전의 PE는 1709 이상 버전에만 포함되어 있습니다. ADK와 PE는 버전을 맞춰 주시는 것이 좋습니다.

## ADK 설치
https://learn.microsoft.com/ko-kr/windows-hardware/get-started/adk-install

위 사이트에서 ADK와 PE 추가 기능을 다운받고 설치합니다. ADK는 **배포 도구**만 설치하면 됩니다.

설치를 완료하면 시작 메뉴의 Windows Kits 폴더에 **배포 및 이미징 도구 환경**이라는 항목이 있습니다. 관리자 권한으로 실행합니다.

## PE 복사
이제 PE를 복사하겠습니다. 명령 프롬프트 창이 하나 떠 있을텐데 아래와 같이 입력합니다.

```batch
copype amd64 D:\PE
```

amd64는 amd64 버전의 PE를 만들 것이기 때문이고 arm64라면 arm64라고 치면 됩니다. 세번째 부분은 경로입니다. 앞으로도 계속 나올텐데 다른 경로를 입력했다면 아래 명령들의 <code>D:\PE</code> 부분을 그 경로로 대체하시면 됩니다.

## 패키지 파일 복사
언어 팩 등의 패키지 파일을 복사합니다. 일반적으로는 <code>C:\Program Files (x86)\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment\amd64\WinPE_OCs</code> 경로에 있습니다. 물론 arm64라면 amd64를 arm64로 바꾸면 됩니다. 여기서 다음 파일들을 <code>D:\PE\packages</code> 폴더로 복사합니다.

1. WinPE-FontSupport-KO-KR.cab
1. WinPE-NetFx.cab
1. WinPE-WMI.cab

ko-kr 폴더에도 필요한 파일이 있습니다. 마찬가지로 <code>D:\PE\packages</code> 폴더로 복사합니다.

1. lp.cab
1. WinPE-NetFx_ko-kr.cab
1. WinPE-WMI_ko-kr.cab

## 패키지 설치 및 정리 작업
명령 프롬프트 창에 다음 명령들을 순서대로 입력합니다.

```batch
dism /mount-image /imagefile:"D:\PE\media\sources\boot.wim" /index:1 /mountdir:"D:\PE\mount"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\WinPE-FontSupport-KO-KR.cab"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\lp.cab"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\WinPE-WMI.cab"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\WinPE-WMI_ko-kr.cab"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\WinPE-NetFx.cab"
dism /image:"D:\PE\mount" /add-package /packagepath:"D:\PE\packages\WinPE-NetFx_ko-kr.cab"
dism /image:"D:\PE\mount" /set-allintl:ko-kr
dism /image:"D:\PE\mount" /set-skuintldefaults:ko-kr
dism /image:"D:\PE\mount" /remove-package /packagename:Microsoft-Windows-WinPE-LanguagePack-Package~31bf3856ad364e35~amd64~en-US~10.0.20348.1
```

PEAction.exe, SimpleVHD.dll, System.ValueTuple.dll 파일을 <code>D:\PE\mount\Windows\system32</code> 폴더에 복사합니다. 그리고 startnet.cmd 파일을 다음 내용으로 바꿉니다. 텍스트 에디터를 관리자 권한으로 열어야 합니다.

```batch
@echo off

set PVDir=null

for %%a in (C: D: E: F: G: H: I: J: K: L: M: N: O: P: Q: R: S: T: U: V: W: X: Y: Z:) do (
    if exist %%a\SimpleVHD\Config.xml set PVDir=%%a\SimpleVHD\
)

if /i "%PVDir%" == "null" exit

path %PVDir%Command;%path%

if exist "%PVDir%Bin\PEAction.exe" (
    start /wait %PVDir%Bin\PEAction.exe
) else (
    start /wait PEAction.exe
)

if %errorlevel% neq 1307 (
    wpeutil reboot
) else (
    wpeutil shutdown
)
```

그런 다음 다음 명령들을 입력합니다.

```batch
dism /unmount-image /mountdir:"D:\PE\mount" /commit
dism /export-image /sourceimagefile:"D:\PE\media\sources\boot.wim" /sourceindex:1 /destinationimagefile:"D:\PE\SimpleVHD.wim"
```

이제 SimpleVHD.wim 파일을 사용하면 됩니다.

## PE 업데이트
SimpleVHD의 새 버전이 나오게 되면 PE를 업데이트해야 합니다. 위 과정에서 mount-image 부분을 실행하고 PEAction.exe와 SimpleVHD.dll 파일을 새 버전으로 교체하면 됩니다. 필요할 경우 startnet.cmd 파일도 교체해야할 수 있습니다.
