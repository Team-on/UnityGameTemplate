[![license](https://img.shields.io/github/license/rfadeev/unity-forge-property-drawers.svg)](https://github.com/rfadeev/unity-forge-property-drawers/blob/master/LICENSE.md)
# Unity Forge Property Drawers
Custom propery drawers to ease fields value management in Unity editor.

## Attributes list
* [AnimationName](#animationname)
* [AnimatorLayerName](#animatorlayername)
* [AnimatorParameterName](#animatorparametername)
* [AnimatorStateName](#animatorstatename)
* [AssetPath](#assetpath)
* [GameObjectLayer](#gameobjectlayer)
* [GameObjectTag](#gameobjecttag)
* [ScenePath](#scenepath)
* [SortingLayerName](#sortinglayername)
* [SpriteAtlasSpriteName](#spriteatlasspritename)

## Installation
Project supports Unity Package Manager. To install project as Git package do following:
1. Close Unity project and open the `Packages/manifest.json` file.
2. Update `dependencies` to have `com.rfadeev.unityforge.propertydrawers` package:
```json
{
  "dependencies": {
    "com.rfadeev.unityforge.propertydrawers": "https://github.com/rfadeev/unity-forge-property-drawers.git"
  }
}
```
3. Open Unity project.

Alternatively, add this repository as submodule under `Assets` folder or download it and put to `Assets` folder of your Unity project. 

## Attributes usage
Import `UnityForge.PropertyDrawers` namespace to be able to use attribute from the [attributes list](#attributes-list).

## AnimationName
![screencast](Documentation/animation-name-example.png)

### Attribute usage
Add attribute to string field to enable selection of animation name value from dropdown list in Unity editor. Attribute without parameters works on Animation component attached to inspected object.
```csharp
[SerializeField, AnimationName]
private string animationName;
```

Specify animation component via `animationField` constructor parameter to enable animation name selection from that component.
```csharp
[SerializeField]
private Animation exampleAnimation;
[SerializeField, AnimationName(animationField: "exampleAnimation")]
private string animationName;
```

[Examples of attribute usage](../master/Runtime/Examples/AnimationName)

### Caveats
Unity manages clips internally specifically so for some reason order of clips returned by [AnimationUtility.GetAnimationClips](https://docs.unity3d.com/ScriptReference/AnimationUtility.GetAnimationClips.html) differs from the one displayed in the editor for Animation comoponent. Due to this expect different order of items in dropdown list for attribute.

## AnimatorLayerName
![screencast](Documentation/animator-layer-name-example.png)

Add attribute to string field to enable selection of animator layer name value from dropdown list in Unity editor. Attribute without parameters works on Animator component attached to inspected object.
```csharp
[SerializeField, AnimatorLayerName]
private string layerName;
```

Specify Animator component via `animatorField` constructor parameter to enable layer name selection from that Animator component.
```csharp
[SerializeField]
private Animator exampleAnimator;
[SerializeField, AnimatorStateName(animatorField: "exampleAnimator")]
private string exampleLayerName;
```

[Examples of attribute usage](../master/Runtime/Examples/AnimatorLayerName)

## AnimatorParameterName
![screencast](Documentation/animator-parameter-name-example.png)

Add attribute to string field to enable selection of animator parameter name value from dropdown list in Unity editor. Note that parameter type must be specified for attribute. Attribute without other parameters works on Animator component attached to inspected object.
```csharp
[SerializeField, AnimatorParameterName(AnimatorControllerParameterType.Float)]
private string exampleFloatParameterName;
```

Specify Animator component via `animatorField` constructor parameter to enable parameter name selection from that Animator component.
```csharp
[SerializeField]
private Animator exampleAnimator;
[SerializeField]
[AnimatorParameterName(AnimatorControllerParameterType.Float, animatorField: "exampleAnimator"))]
private string exampleFloatParameterName;
```

[Examples of attribute usage](../master/Runtime/Examples/AnimatorParameterName)

## AnimatorStateName
![screencast](Documentation/animator-state-name-example.gif)

### Attribute usage
Add attribute to string field to enable selection of animator state name value from dropdown list in Unity editor. Attribute without parameters works on Animator component attached to inspected object.
```csharp
[SerializeField, AnimatorStateName]
private string stateName;
```

Specify Animator component via `animatorField` constructor parameter to enable state name selection from that Animator component.
```csharp
[SerializeField]
private Animator exampleAnimator;
[SerializeField, AnimatorStateName(animatorField: "exampleAnimator")]
private string exampleStateName;
```

[Examples of attribute usage](../master/Runtime/Examples/AnimatorStateName)

### Caveats
Since layer index is [decoupled](https://docs.unity3d.com/ScriptReference/Animator.Play.html) from animator state name in Unity API, state name alone does not determine state and state index value should be managed separately. If only one animation layer is used, it's not the problem and `Play(string stateName)` overload can be used safely for fields using `AnimatorStateName` attribute.

## AssetPath
![screencast](Documentation/asset-path-example.png)

### Attribute usage
Add attribute to string field to enable show object field instead of text input for object path. Object selection updates serialized value of the string field. Attribute declataion requires object type. Attribute supports both full project path (like "Assets/Sprites/MySpriteA.png") and resources path type (like "Sprites/MySpriteB" for full path value "Assets/Resources/Sprites/MySpriteB.png"). Additionnal option allows to preview path value which is serialized.
```csharp
[SerializeField, AssetPath(typeof(Sprite), false)]
private string spriteProjectPath01;
[SerializeField, AssetPath(typeof(Sprite), true)]
private string spriteResourcesPath01;
[SerializeField, AssetPath(typeof(Sprite), false, true)]
private string spriteProjectPath02;
[SerializeField, AssetPath(typeof(Sprite), true, true)]
private string spriteResourcesPath02;
```

[Examples of attribute usage](../master/Runtime/Examples/AssetPath)

## GameObjectLayer
![screencast](Documentation/game-object-layer-example.png)

### Attribute usage
Add attribute to int field to enable selection of game object layer value from dropdown list in Unity editor. Layers are configured in [Tags and Layers Manager](https://docs.unity3d.com/Manual/class-TagManager.html).
```csharp
[SerializeField, GameObjectLayer]
private int exampleLayer;
```
[Examples of attribute usage](../master/Runtime/Examples/GameObjectLayer)

## GameObjectTag
![screencast](Documentation/game-object-tag-example.png)

### Attribute usage
Add attribute to string field to enable selection of game object tag value from dropdown list in Unity editor. Tags are configured in [Tags and Layers Manager](https://docs.unity3d.com/Manual/class-TagManager.html).
```csharp
[SerializeField, GameObjectTag]
private string exampleTag;
```
[Examples of attribute usage](../master/Runtime/Examples/GameObjectTag)

## ScenePath
![screencast](Documentation/scene-path-example.png)

### Attribute usage
Add attribute to string field to enable selection of scene path value from dropdown list in Unity editor. Path type can be set via `fullProjectPath` argument: if true, scene path will be full project path like "Assets/Scenes/MyScene.unity"; if false, path will be short project path without "Assets/" and ".unity" extension like "Scenes/MyScene". Source to populate dropdown can be set via `fromBuildSettings` argument: if true, only scenes from build settings will be shown in dropdown; if false, all scenes from project will be shown in dropdown. Additionally, `onlyEnabled` argument can be used to show only scenes enabled in build settings if `fromBuildSettings` is set to true.
```csharp
// Full scene path, only enabled scenes from build settings
[SerializeField, ScenePath]
private string exampleScenePath01;
// Short scene path, only enabled scenes from build settings
[SerializeField, ScenePath(fullProjectPath: false))]
private string exampleScenePath02;
// Full scene path, all scenes from project
[SerializeField, ScenePath(fromBuildSettings: false))]
private string exampleScenePath03;
// Short scene path, all scenes from build settings
[SerializeField, ScenePath(fullProjectPath: false, onlyEnabled: false))]
private string exampleScenePath04;
```

[Examples of attribute usage](../master/Runtime/Examples/ScenePath)

## SortingLayerName
![screencast](Documentation/sorting-layer-name-example.png)

### Attribute usage
Add attribute to string field to enable selection of sorting layer name value from dropdown list in Unity editor. Sorting layers are configured in [Tags and Layers Manager](https://docs.unity3d.com/Manual/class-TagManager.html).
```csharp
[SerializeField, SortingLayerName]
private string exampleSortingLayerName;
```

[Examples of attribute usage](../master/Runtime/Examples/SortingLayerName)

## SpriteAtlasSpriteName
![screencast](Documentation/sprite-atlas-sprite-name-example.png)

### Attribute usage
Add attribute to string field and specify sprite atlas field to enable selection of sprite name from that atlas via dropdown list in Unity editor.
```csharp
[SerializeField]
private SpriteAtlas atlas;
[SerializeField, SpriteAtlasSpriteName(spriteAtlasField: "atlas")]
private string spriteName;
```

[Examples of attribute usage](../master/Runtime/Examples/SpriteAtlasSpriteName)
