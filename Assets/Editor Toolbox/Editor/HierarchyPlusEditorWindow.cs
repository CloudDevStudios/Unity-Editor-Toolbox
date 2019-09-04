using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

namespace Toolbox.Editor
{
    public class HierarchyPlusEditorWindow : SearchableEditorWindow//, IHasCustomMenu
    {
        [MenuItem("Window/Toolbox/Hierarchy+")]
        public static void Init()
        {
            var window = GetWindow<HierarchyPlusEditorWindow>("Hierarchy+");
            if (window)
            {
                window.Show();
                window.titleContent.image = Style.windowIcon;
            }
        }


        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Work in Progress");
        }

        /// <summary>
        /// Static representation of custom hierarchy style.
        /// </summary>
        internal static class Style
        {
            /// 
            /// Custom rect handling fields
            /// 

            internal static readonly float maxHeight = 16.0f;
            internal static readonly float maxWidth = 55.0f;
            internal static readonly float lineWidth = 1.0f;
            internal static readonly float lineOffset = 2.0f;
            internal static readonly float iconWidth = 17.0f;
            internal static readonly float iconHeight = 17.0f;
            internal static readonly float layerWidth = 17.0f;

            internal static readonly Color lineColor;
            internal static readonly Color labelColor;

            /// 
            /// Custom label styles
            /// 

            internal static readonly GUIStyle tagLabelStyle;
            internal static readonly GUIStyle layerLabelStyle;
            internal static readonly GUIStyle backgroundStyle;

            internal static readonly Texture windowIcon;

            static Style()
            {
                lineColor = new Color(0.59f, 0.59f, 0.59f);
                labelColor = EditorGUIUtility.isProSkin
                    ? new Color(0.22f, 0.22f, 0.22f)        //standard dark skin color
                    : new Color(0.855f, 0.855f, 0.855f);    //hierarchy header background color
                                                            //: new Color(0.76f, 0.76f, 0.76f);       //standard light skin color

                //set tag label style based on mini label
                tagLabelStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontSize = 8
                };
                tagLabelStyle.normal.textColor = new Color(0.35f, 0.35f, 0.35f);

                //set layer label style based on mini label
                layerLabelStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontSize = 8,
                    alignment = TextAnchor.UpperCenter
                };

                //set proper background texture object
                var backgroundTex = new Texture2D(1, 1);
                backgroundTex.SetPixel(0, 0, labelColor);
                backgroundTex.Apply();
                backgroundTex.hideFlags = HideFlags.HideAndDontSave;

                //set background style based on custom background texture
                backgroundStyle = new GUIStyle();
                backgroundStyle.normal.background = backgroundTex;

                windowIcon = EditorGUIUtility.TrIconContent("UnityEditor.HierarchyWindow").image;
            }
        }
    }
}