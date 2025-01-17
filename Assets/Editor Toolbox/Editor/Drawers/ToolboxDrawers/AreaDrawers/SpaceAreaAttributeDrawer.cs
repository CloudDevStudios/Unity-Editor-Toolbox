﻿using UnityEngine;
using UnityEditor;

namespace Toolbox.Editor.Drawers
{
    public class SpaceAreaAttributeDrawer : ToolboxAreaDrawer<SpaceAreaAttribute>
    {
        public override void OnGuiBegin(SpaceAreaAttribute attribute)
        {
            GUILayout.Space(attribute.SpaceBefore);         
        }

        public override void OnGuiEnd(SpaceAreaAttribute attribute)
        {
            GUILayout.Space(attribute.SpaceAfter);
        }
    }
}