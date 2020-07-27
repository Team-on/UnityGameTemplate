# Unity Game Template (On active development. Use on your own risk)
**Game Template** is all necessary stuff taken care for Unity users not to worry about creating most boring and tedious work.  
I'd be happy for any contribution to make this template as good as it can be.  
//TODO: use as template this https://github.com/nezvers/Godot-GameTemplate

## TODO docs:
 * UI for menu/popups (Main menu, credits, options...)
 * AudioManager
 * Build Manager
 * Changelog
 * Event bus
 * EventManager
 * GameManager
 * player prefs extended saver
 * Localization manager(polyglot, custom xml)
 * Camera shake (in LeanTween)
 * Unity crashlitics and feedback
 * Label with version in mainmenu and pause  
   
## TODO:
 * Keyboard/gamepad controll in menu
 * Indexed array
 * ToastMessage
 * Screenshot/video capture
 * Pause in-game 
 * Set scripting backend to il2cpp on release build? (except IOS and linux, unity can't build for it using il2cpp). (Increase windows build size by ~1mb and build time in 3 times(only unfinished main menu build))
 * Better reporting (https://unitytech.github.io/clouddiagnostics/userreporting/UnityCloudDiagnosticsUserReports.html)
 * Build size explorer. Like this https://github.com/aschearer/unitysizeexplorer but in editor
 * Obj pooling in AudioManager
 * Audio manager - 3d sound support
 * Complete all actions for build manager
	* Release notes like https://assetstore.unity.com/packages/tools/utilities/build-3720
	* VR build side-by-side with regular
	* Push to steam?
	* Push to google drive
 * Obj pooling
 * Hightscores(playfab?)
 * Auto add readme to build folders
 * Build with teamcity or unity cloud build
 * Achievements
 *  Use il2cpp in master mode
 
 ## Used assets/tools:
 TODO: import them in right way, cuz untiy now support git packages.  
 Some assets was reworked, see changelog in Assets\Thirdparty\CustomChanges.md  
 https://github.com/Team-on/CustomToolbar  
 http://wiki.unity3d.com/index.php/ArrayPrefs2  
 https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126  
 https://assetstore.unity.com/packages/tools/animation/leantween-3595  
 https://assetstore.unity.com/packages/2d/gui/icons/simple-ui-elements-53276  
 https://assetstore.unity.com/packages/tools/gui/ui-tools-for-unity-124299  
 https://archive.codeplex.com/?p=DotNetZip  
 https://itch.io/docs/butler/  
 https://github.com/Team-on/unity-editor-spotlight  
 https://github.com/yasirkula/UnityIngameDebugConsole  
 https://github.com/2irate2migrate/HierarchyHighlighter  
 https://github.com/dbrizov/NaughtyAttributes/  
 https://github.com/jedybg/yaSingleton  
 https://nvjob.itch.io/fps-counter-and-graph  
 https://github.com/ogxd/project-curator  
 https://seansleblanc.itch.io/better-minimal-webgl-template  
 https://github.com/agens-no/PolyglotUnity  
 https://github.com/Konash/arabic-support-unity  
 https://assetstore.unity.com/packages/tools/playersprefs-editor-and-utilities-26656  
 https://github.com/Maligan/unity-subassets-drag-and-drop  
 https://github.com/s-m-k/Unity-Animation-Hierarchy-Editor  
 https://github.com/DapperDino/Dapper-Tools  
 https://github.com/rfadeev/pump-editor  
 https://github.com/PhannGor/unity3d-rainbow-folders  
 https://github.com/rfadeev/presets-browser  
 https://github.com/rfadeev/unity-forge-extension-methods  
 https://github.com/rfadeev/unity-forge-property-drawers  
 
 ## Want to use: 
 https://github.com/TeamSirenix/odin-serializer  
 https://odininspector.com/	(Actually, I like this asset, but it's paid, so I'm not sure is it good to keep it in template)  
 https://github.com/5argon/OdinHierarchy  
 https://assetstore.unity.com/packages/tools/gui/cyro-build-debugger-65101  
 https://assetstore.unity.com/packages/tools/gui/debuggui-graph-139275  
   
## First launch(for every user):
 * Move to Edit/Preferences/Rainbow Folders and change *Folder Location* to *Assets/Thirdparty/RainbowFolders*

## Setting up git LFS:
* Download https://git-lfs.github.com/
* git lfs install
* It's all. .gitattributes already setup to move almost all files with 'big' extension to LFS

## Setting up for your game:
 * Move to *Edit/Project settings/Player* and set *Company name*, *Product name* and *Icon*
 * Move to *Window/Builds(ALT+B)* and set up build sequence. Default already setup good, so you just need to add right *itch.io link*
 * Move to *Window/General/Services(CTRL+0)/Settings* and link project to your project. Enable *Analytics* and *Cloud Diagnostics*
 * Copy [this](https://docs.google.com/spreadsheets/d/13YCRi6fHNaS_DRApBelilgdM6O833hLiCy68F47KWIU/edit#gid=296134756) translation sheet for polyglot localization and save it on your Google drive. Be sure to make it public
 * Move to *Window/Polyglot Localization/SConfigurate* and add your own *Docs Id* and *Sheet id*
 
## On every launch:
 * Select all singletones in *ScriptableObjects/Singletons* and reimport them. Also you must reimport any other singleton derived from Singleton<>. (Probably it's some yaSingleton bug)
 
## Before build:
 * Move to *Window/Builds(ALT+B)* and increase *Version* and *Android bundle version*
 * Move to *Window/Builds(ALT+B)/Changelog* and write little *Update name*
 * Move to *Window/Polyglot Localization/SConfigurate* and click *Download* button for both *Master* and *Custom* sheet
 * Use this checklist https://thegamedev.guru/unity-performance/checklist/
 
## Setting up itch.io page:
 * Move to *Window/Builds(ALT+B)* and click *Build Local + Zip + itch.io*. Wait untill all builds get pushed
 * Move to your itch.io page an checkmark all builds with approprivate labels
 * Set up *Kind of project* as HTML
 * Find *Embed options* and select *Embed in page* *Manually set size*
 * Find *Viewport dimensions* and set *Width* to 960 and *Height* to 540
 
 ## Setting up template for 3D games
 * Move to *Edit/Project settings/Editor* and set *Default Behaviour Mode/Mode* to 3D
 * Move to *Edit/Project settings/Player* and set *Color Space* to Gamma. (In 3D it looks better)
 
 ## Warning:
 * By default, *Reload Domain* and *Reload Scene* Disabled, so you need to write code, where all static fields initialize not in static ctor.
