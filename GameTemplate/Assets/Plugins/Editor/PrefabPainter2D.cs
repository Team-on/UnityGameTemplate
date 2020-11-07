using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("Prefab Painter 2D")]
public class PrefabPainter2D : EditorTool {
    float DiscRadius;

    public override void OnToolGUI(EditorWindow window) {

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); // disable ability to select in the scene when using this tool

        if (DiscRadius == 0) DiscRadius = 1.0f;

        Handles.color = new Color(0, 255, 0, 0.1f);
        Vector3 discPos = new Vector3(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.x, HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.y, 0.0f);
        Handles.DrawSolidDisc(discPos, Vector3.forward, DiscRadius);

        // NOTE: EventType.ScrollWheel is problematic with some Logitech mice, hence the workaround...

        if (Event.current.modifiers == EventModifiers.Shift && Event.current.type == EventType.Used) {
            DiscRadius += Event.current.delta.x * 0.1f;
            // has to be 1 because of unit circle later!
            if (DiscRadius < 1.0f) DiscRadius = 1.0f;
        }

        if (Selection.gameObjects.Length > 0 && Event.current.type == EventType.MouseDrag) {
            // register undo as well!
            GameObject temp = (GameObject)PrefabUtility.InstantiatePrefab(Selection.gameObjects[0]);
            Undo.RegisterCreatedObjectUndo(temp, "Painted Prefab");

            // place the object within a circle
            Vector3 mouseToWorld = new Vector3(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.x, HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.y, 0.0f);
            Vector2 rand = Random.insideUnitCircle;
            rand *= DiscRadius;
            Vector3 result = new Vector3(rand.x, rand.y, 0.0f);

            temp.transform.position = mouseToWorld + result;
        }
        HandleUtility.Repaint();
    }
}