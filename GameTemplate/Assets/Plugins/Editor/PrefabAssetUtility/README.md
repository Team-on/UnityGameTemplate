# Prefab Asset Utility
A utility package that keeps track of the connections between prefabs and their attached assets

# Performance
This package has been tested on a project with 10K+ prefabs, initial cache generation took roughly 40 seconds and after that it incrementally updates itself

# Lazy Loading
By default the cache will be loaded, and generated if it doesn't exist yet, when you first try to use any of the methods. 
You can turn lazy loading off in the Preferences and it will take effect the next time you launch the editor.

# Asset Serialization Mode
Must be set to Force Text in order for this package to work

# Usage
Get all prefabs which use a specific asset/component based on its GUID
`PrefabUtility.GetPrefabsForGUID("f7a4213c60a3426995bb8b901c2ea1fd")`

Or perform the reverse and get all GUIDs assigned to a prefab based on its path
`PrefabUtility.GetGUIDsForPrefab("Assets/Prefabs/MyAwesomePrefab.prefab")`

Get all components that contain a MeshFilter
`PrefabUtility.GetPrefabsWithComponent<MeshFilter()`

Or get all components that a prefab uses
`PrefabUtility.GetComponentsForPrefab("Assets/Prefabs/MyAwesomePrefab.prefab")`

# Editor only
This package only works in the Editor, it has been tested on Windows but should also work on macOS though this hasn't been tested yet

# Settings
Open `Edit` -> `Preferences` -> `Prefab Asset Cache` to force refresh the cache and enable/disable saving the cache on every prefab change to the disk

# Cache location
This package stores its cache in the Library folder in `PrefabToGUID.json`, `GUIDToPrefab.json`, `PrefabToComponent.json` and `ComponentToPrefab.json`
