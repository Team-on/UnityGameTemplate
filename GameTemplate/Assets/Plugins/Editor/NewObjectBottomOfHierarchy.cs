using UnityEngine;
using UnityEditor;
 
[InitializeOnLoad]
public static class NewObjectBottomOfHierarchy
{
    static NewObjectBottomOfHierarchy()
    {
        SceneView.duringSceneGui += PlaceDroppedObjectToTheBottom;   
    }

    static void PlaceDroppedObjectToTheBottom(SceneView sceneView)
    {
        if(Event.current.type == EventType.DragExited)
        {
            if(Selection.gameObjects.Length > 0)
            {
                // this checks out because it's only possible to drag and drop a single prefab to the scene at a time!
                Selection.gameObjects[0].transform.SetAsLastSibling();
            }
        }
    }
}