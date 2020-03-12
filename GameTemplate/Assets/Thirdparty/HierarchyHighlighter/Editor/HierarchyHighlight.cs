using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEditor;

using Scripts.Utility;

namespace Assets.Editor
{
    [InitializeOnLoad]
    public class HierarchyHighlightManager
    {
        //==============================================================================
        //
        //                                    CONSTANTS
        //
        //==============================================================================



        public static readonly Color DEFAULT_COLOR_HIERARCHY_SELECTED = new Color(0.243f, 0.4901f, 0.9058f, 1f);







        //==============================================================================
        //
        //                                    CONSTRUCTORS
        //
        //==============================================================================



        static HierarchyHighlightManager()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyHighlight_OnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyHighlight_OnGUI;
        }







        //==============================================================================
        //
        //                                    EVENTS
        //
        //==============================================================================



        private static void HierarchyHighlight_OnGUI(int inSelectionID, Rect inSelectionRect)
        {
            GameObject GO_Label = EditorUtility.InstanceIDToObject(inSelectionID) as GameObject;

            if (GO_Label != null)
            {
                HierarchyHighlighter Label = GO_Label.GetComponent<HierarchyHighlighter>();

                if(Label != null && Event.current.type == EventType.Repaint)
                {
                    #region Determine Styling

                    bool ObjectIsSelected = Selection.instanceIDs.Contains(inSelectionID);

                    Color BKCol = Label.Background_Color;
                    Color TextCol = Label.Text_Color;
                    FontStyle TextStyle = Label.TextStyle;

                    if(!Label.isActiveAndEnabled)
                    {
                        if(Label.Custom_Inactive_Colors)
                        {
                            BKCol = Label.Background_Color_Inactive;
                            TextCol = Label.Text_Color_Inactive;
                            TextStyle = Label.TextStyle_Inactive;
                        }
                        else
                        {
                            if(BKCol != HierarchyHighlighter.DEFAULT_BACKGROUND_COLOR)
                                BKCol.a = BKCol.a * 0.5f; //Reduce opacity by half

                            TextCol.a = TextCol.a * 0.5f;
                        }
                    }

                    #endregion


                    Rect Offset = new Rect(inSelectionRect.position + new Vector2(2f, 0f), inSelectionRect.size);


                    #region Draw Background

                    //Only draw background if background color is not completely transparent
                    if (BKCol.a > 0f)
                    {
                        Rect BackgroundOffset = new Rect(inSelectionRect.position, inSelectionRect.size);

                        //If the background has transparency, draw a solid color first
                        if (Label.Background_Color.a < 1f || ObjectIsSelected)
                        {
                            //ToDo: Pull background color from GUI.skin Style
                            EditorGUI.DrawRect(BackgroundOffset, HierarchyHighlighter.DEFAULT_BACKGROUND_COLOR);
                        }

                        //Draw background
                        if (ObjectIsSelected)
                            EditorGUI.DrawRect(BackgroundOffset, Color.Lerp(GUI.skin.settings.selectionColor, BKCol, 0.3f));
                        else
                            EditorGUI.DrawRect(BackgroundOffset, BKCol);
                    }

                    #endregion


                    EditorGUI.LabelField(Offset, GO_Label.name, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = TextCol },
                        fontStyle = TextStyle
                    });

                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
}