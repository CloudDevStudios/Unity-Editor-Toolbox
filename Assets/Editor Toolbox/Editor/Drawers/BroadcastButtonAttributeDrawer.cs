﻿using UnityEditor;
using UnityEngine;

namespace Toolbox.Editor
{
    [CustomPropertyDrawer(typeof(BroadcastButtonAttribute))]
    public class BroadcastButtonAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return Style.height + Style.spacing;
        }

        public override void OnGUI(Rect position)
        {
            position.height = Style.height;

            var disable = false;
            switch (Attribute.Type)
            {
                case ButtonActivityType.Everything:
                    break;
                case ButtonActivityType.Nothing:
                    disable = true;
                    break;
                case ButtonActivityType.OnEditMode:
                    disable = Application.isPlaying;
                    break;
                case ButtonActivityType.OnPlayMode:
                    disable = !Application.isPlaying;
                    break;
            }

            EditorGUI.BeginDisabledGroup(disable);
            if (GUI.Button(position, string.IsNullOrEmpty(Attribute.Label) ? Attribute.MethodName : Attribute.Label))
            {
                var targetGameObject = Selection.activeGameObject;
                if (targetGameObject == null) return;
                targetGameObject.SendMessage(Attribute.MethodName, SendMessageOptions.RequireReceiver);
            }
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// A wrapper which returns the PropertyDrawer.attribute field as a <see cref="global::BroadcastButtonAttribute"/>.
        /// </summary>
        private BroadcastButtonAttribute Attribute => attribute as BroadcastButtonAttribute;


        /// <summary>
        /// Static representation of button style.
        /// </summary>
        private static class Style
        {
            internal static readonly float height = EditorGUIUtility.singleLineHeight * 1.25f;
            internal static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;

            internal static GUIStyle buttonStyle;

            static Style()
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
            }
        }
    }
}