# Installation

Before you install the package, make sure you meet the [prerequisites](#prerequisites).

## Install the KTX package using the Unity Package Manager

The package is currently experimental, you can't install it through the Unity Package Manager (UPM) user interface.

To install the KTX package using the UPM, see the following steps:
1. Open your `<your-unity-project-path>/Packages/manifest.json` file.
2. Add the package to the dependencies section, with a single new line in your `manifest.json` file (see the snippet below). Make sure you use the latest version of the package. You can keep all the other dependencies in the file.
3. Open your project in the Unity Editor, the package and its dependencies will be resolved automatically.
4. In your Unity project, go to **Window > Package Manager** and verify that the package and all its dependencies are resolved.

Below is a snippet showing the line that you should add in the `manifest.json` file. Make sure you don't add the lines starting with `//`. They were added in the snippet to provide some explanation. Adding these lines will generate an error within Unity.
```json
  {
    "dependencies": {
      // Add this line:
      "com.unity.cloud.ktx": "2.2.3",
      // Other dependencies...
    }
  }
```

## Prerequisites

To use the package, you require the following:

* A Unity version of 2018.4 or later. In the Unity Hub, go to **Installs** > **Official releases** to find the latest Long Term Support version.
