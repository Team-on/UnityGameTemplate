# Rect Extensions

Read about `RextEx` on [habr](https://habr.com/post/340858/) (ru).
Extended docs at [Documentation](./Documentation/README.md) folder (according to [Unity Package Layout](https://docs.unity3d.com/Manual/cus-layout.html)). 

RectEx is a tool that simplify drawing EditorGUI (not *Layout*) elements with [Unity3d](https://unity3d.com). 

RectEx consists of a bunch of `Rect` extensions, which provides an interface to work with rects easier.

![GitHub release](https://img.shields.io/github/release/slavniyteo/rect-ex.svg)
![GitHub Release Date](https://img.shields.io/github/release-date/slavniyteo/rect-ex.svg)
![Github commits (since latest release)](https://img.shields.io/github/commits-since/slavniyteo/rect-ex/latest.svg)
![GitHub last commit](https://img.shields.io/github/last-commit/slavniyteo/rect-ex.svg)

Stop doing 

```csharp
rect.height = 18;
GUI.LabelField(rect, "First line");
rect.y += rect.height;
GUI.LabelField(rect, "Second line");
rect.y += rect.height;
rect.width = (rect.width - 5) / 2;
GUI.LabelField(rect, "Third line: left part");
rect.x += rect.width + 5;
GUI.LabelField(rect, "Third line: right part");
```

Use **Column** and **Row** instead:

```csharp
using RectEx;
```
```csharp
var rects = rect.Column(3);
GUI.LabelField(rects[0], "First line");
GUI.LabelField(rects[1], "Second line");

rects = rects[2].Row(2);
GUI.LabelField(rects[0], "Third line: left part");
GUI.LabelField(rects[1], "Third line: right part");
```

# Why do you need it?

Because you don't want to get lost among all these `rect.y += rect.height` and `rect.width = (rect.width - 5) / 2`.

Because you want to draw your GUI elements easily and feel yourself comfortable.

Because you want to make your code readable.

Because you aren't afraid of trying new awesome things, are you?

# Contributing

If you want to add a bug report or feature request, just add an issue or PR. 

If you create a Pull Request with new feature or bugfix, please write tests.

If you find any errors in this text, feel free to fix it.

# Installation

The preferrable way to install RectEx is [Unity Package Manager](https://docs.unity3d.com/Manual/Packages.html).

To add RectEx to your Unity project, add following dependency to your `manifest.json` as described [here](https://docs.unity3d.com/Manual/upm-dependencies.html) and [here](https://docs.unity3d.com/Manual/upm-git.html). Use **master** or any version above **v0.1.0** because v0.1.0 and previos versions are not compatible with Unity Package Manager. 

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master"
  }
}
```

To be able to run tests add these lines:

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master"
  },
  "testables": [
    "st.rect-ex"
  ]
}
```
