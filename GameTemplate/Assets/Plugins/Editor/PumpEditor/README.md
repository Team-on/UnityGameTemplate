[![license](https://img.shields.io/github/license/rfadeev/pump-editor.svg)](https://github.com/rfadeev/pump-editor/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/com.rfadeev.pumpeditor?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.rfadeev.pumpeditor/)

# Pump Editor
Collection of Unity editor helpers to boost productivity.

## Installation

### Install via Git

Project supports Unity Package Manager. To install project as Git package do following:
1. Close Unity project and open the `Packages/manifest.json` file.
2. Update `dependencies` to have `com.rfadeev.pumpeditor` package:
```json
{
  "dependencies": {
    "com.rfadeev.pumpeditor": "https://github.com/rfadeev/pump-editor.git"
  }
}
```
3. Open Unity project.

Alternatively, add this repository as submodule under `Assets` folder or download it and put to `Assets` folder of your Unity project. 

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.rfadeev.pumpeditor
```

### How to use
Access Pump Editor editor windows via Unity toolbar: **Window -> Pump Editor**.

### Features

Editor windows:
* [Built-in Define Directives Editor Window](https://github.com/rfadeev/pump-editor/wiki/Built-in-define-directives-editor-window) - view Unity built-in defines values at editor time.
* [Compilation Editor Window](https://github.com/rfadeev/pump-editor/wiki/Compilation-Editor-Window) - force Unity to recompile scripts.
* [Force Reserialize Assets Editor Window](https://github.com/rfadeev/pump-editor/wiki/Force-Reserialize-Assets-Editor-Window) - force reserialize project assets explicitely.
* [Prefab Variants Editor Window](https://github.com/rfadeev/pump-editor/wiki/Prefab-Variants-Editor-Window) - preview prefab variants inheritance chains as tree view.
* [Project Settings Select Editor Window](https://github.com/rfadeev/pump-editor/wiki/Project-Settings-Select-Editor-Window) - select project settings to edit in one click.
* [Save Project As Template Editor Window](https://github.com/rfadeev/pump-editor/wiki/Project-templates-editor-windows) - save current project as template to be used in Unity Hub for new project creation.
* [Scene Open Editor Window](https://github.com/rfadeev/pump-editor/wiki/Scene-Open-Editor-Window) - open scene from project or build settings.

Shortcuts:
* [Lock Toggle Shortcuts](https://github.com/rfadeev/pump-editor/wiki/Lock-Toggle-Shortcuts) - switch editor window locks easily.
