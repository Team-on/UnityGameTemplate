[![license](https://img.shields.io/github/license/rfadeev/presets-browser.svg)](https://github.com/rfadeev/presets-browser/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/com.rfadeev.pumpeditor.presetsbrowser?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.rfadeev.pumpeditor.presetsbrowser/)

# Presets browser
Unity presets browser editor window.

![presets-browser-demo](https://user-images.githubusercontent.com/5451929/47091018-a5792300-d256-11e8-8ee2-3a5d3f40ee70.gif)

## Summary
Unity 2018.1 introduced [Presets](https://docs.unity3d.com/2018.1/Documentation/ScriptReference/Presets.Preset.html) feature
to improve editor workflow. While presets can be found via project window search, it's not possible to filter presets by type or
determine preset asset validity in project window. Presets browser editor window addresses this issues and serves as central
place to access all project presets.

## Installation

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.rfadeev.pumpeditor.presetsbrowser
```

### Install via Git URL

Project supports Unity Package Manager. To install project as Git package do following:
1. Close Unity project and open the `Packages/manifest.json` file.
2. Update `dependencies` to have `com.rfadeev.pumpeditor.presetsbrowser` package:
```json
{
  "dependencies": {
    "com.rfadeev.pumpeditor.presetsbrowser": "https://github.com/rfadeev/presets-browser.git"
  }
}
```
3. Open Unity project.

### Install via Git Submodule

Alternatively, add this repository as submodule under `Assets` folder or download it and put to `Assets` folder of your Unity project. 


## How to use
Access presets browser via Unity toolbar: **Window -> Pump Editor -> Presets Browser**.

Preset can be invalid if you delete the class it was referencing. To filter presets by validity use validity toolbar.
Following options are supported:
* `All` - show all preset assets regardless of whether they are valid or not.
* `Only Valid` - show only valid preset assets
* `Only Invalid` - show only not valid preset assets

Use `Filter by preset type` toggle to control filtering of listed preset assets:
* Toggle on - filter listed preset assets by target preset type selected via popup. For `Only Invalid` validity toolbar option
listed preset are not filtered and type selection popup is not shown.
* Toggle off - no filtering of listed preset assets by preset type

Click preset asset object field to ping object in project window. Click `Select` button to select preset asset in project window.
