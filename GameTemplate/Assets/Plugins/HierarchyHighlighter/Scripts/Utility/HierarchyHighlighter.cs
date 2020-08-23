using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Utility
{
    public class HierarchyHighlighter : MonoBehaviour
    {
        //==============================================================================
        //
        //                                    CONSTANTS
        //
        //==============================================================================



        public static readonly Color DEFAULT_BACKGROUND_COLOR = new Color(0.76f, 0.76f, 0.76f, 1f);

        public static readonly Color DEFAULT_BACKGROUND_COLOR_INACTIVE = new Color(0.306f, 0.396f, 0.612f, 1f);

        public static readonly Color DEFAULT_TEXT_COLOR = Color.black;





        //==============================================================================
        //
        //                                    CONSTRUCTORS
        //
        //==============================================================================



        public HierarchyHighlighter(){}



        public HierarchyHighlighter(Color inBackgroundColor)
        {
            this.Background_Color = inBackgroundColor;
        }



        public HierarchyHighlighter(Color inBackgroundColor, Color inTextColor, FontStyle inFontStyle = FontStyle.Normal)
        {
            this.Background_Color = inBackgroundColor;
            this.Text_Color = inTextColor;
            this.TextStyle = inFontStyle;
        }





        //==============================================================================
        //
        //                                    PROPERTIES
        //
        //==============================================================================


        [Header("Active State")]
        public Color Text_Color = DEFAULT_TEXT_COLOR;

        public FontStyle TextStyle = FontStyle.Normal;

        /// <summary>
        /// Color of the background.  Set alpha 0 to not draw a background period.
        /// </summary>
        public Color Background_Color = DEFAULT_BACKGROUND_COLOR;



        [Header("Inactive State")]
        public bool Custom_Inactive_Colors = false;

        public Color Text_Color_Inactive = DEFAULT_TEXT_COLOR;

        public FontStyle TextStyle_Inactive = FontStyle.Normal;

        /// <summary>
        /// Color of the background in an inactive state.  Set alpha 0 to not draw a background period.
        /// </summary>
        public Color Background_Color_Inactive = DEFAULT_BACKGROUND_COLOR_INACTIVE;
    }
}
