# Installation

> [!NOTE]
> Before you install the package, make sure you meet the [system requirement](#system-requirement) and [prerequisites](#prerequisites).

## Install the *KTX for Unity* package using the Unity Package Manager

> [!TIP]
> Click the following link to fast-track the installation and skip to step 5: [com.unity.cloud.ktx](com.unity3d.kharma:upmpackage/com.unity.cloud.ktx)

To install *KTX for Unity* package, follow these steps:

1. In your Unity project, go to Windows > Package Manager.
2. On the status bar, select the Add (+) button.
3. From the Add menu, select Add + package by name. Name and Version fields appear.
4. In the Name field, enter `com.unity.cloud.ktx`.
5. Click *Add*.
6. The Editor installs the latest available version of the package and any dependent packages.

## System Requirement

*KTX for Unity* supports the following platforms:

- Web
- iOS
- tvOS
- visionOS
- Android
  - Chrome OS (via Android)
  - Magic Leap 2 (via Android)
- Windows (see [Visual C++ runtime](#visual-c-runtime-on-windows))
- Universal Windows Platform
- macOS
- Linux (x86_64)
- Embedded Linux

## Prerequisites

To use the package, you require the following:

- A Unity version of 6000.0 or newer. In the Unity Hub, go to **Installs** > **Official releases** to find the latest Long Term Support version.

## Visual C++ runtime on Windows

On Windows (x64 and ARM64), the package's native library (`ktx_unity.dll`) links the Microsoft Visual C++ runtime dynamically. It requires these DLLs at runtime:

- `MSVCP140.dll`
- `VCRUNTIME140.dll`
- `VCRUNTIME140_1.dll` (x64 only)

If they're missing, the native library fails to load (usually a `DllNotFoundException` in the log) and no KTX or Basis Universal textures load.

The DLLs are part of the [Microsoft Visual C++ Redistributable][vcredist] (Visual Studio 2015–2022, v14). Many machines already have it installed (for example by Visual Studio, Steam or other applications), but you can't rely on it on end users' machines.

### Unity 6.7 and newer

Windows players built with Unity 6.7 or newer include the Visual C++ runtime DLLs. No action is required.

### Earlier Unity versions

Make sure the Visual C++ Redistributable is installed on the target machine. Common options:

- Run the redistributable installer (`vc_redist.x64.exe` or `vc_redist.arm64.exe`, available from the [Microsoft download page][vcredist]) from your application's installer, for example silently with `vc_redist.x64.exe /install /quiet /norestart`.
- If you distribute via a store or launcher, declare the Visual C++ Redistributable as an install prerequisite where supported (for example, Steam's redistributables setting).
- Copy the DLLs next to your application's executable (app-local deployment). Microsoft supports this but discourages it, because app-local copies don't receive security updates via Windows Update. See [Redistributing Visual C++ files][vcredist-deploy].

## Trademarks

*Unity* is a registered trademark of [Unity Technologies][unity].

Khronos&reg; and the Khronos Group logo are registered trademarks of the [The Khronos Group Inc][khronos].

KTX&trade; and the KTX logo are trademarks of the [The Khronos Group Inc][khronos].

[khronos]: https://www.khronos.org
[unity]: https://unity.com
[vcredist]: https://learn.microsoft.com/cpp/windows/latest-supported-vc-redist
[vcredist-deploy]: https://learn.microsoft.com/cpp/windows/redistributing-visual-cpp-files
