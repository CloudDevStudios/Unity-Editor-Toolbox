﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace Toolbox.Editor
{
    using Toolbox.Editor.Drawers;

    [InitializeOnLoad]
    internal static class ToolboxEditorUtility
    {
        private static ToolboxEditorSettings settings;

        private readonly static Dictionary<Type, ToolboxAreaDrawerBase> areaDrawers = new Dictionary<Type, ToolboxAreaDrawerBase>();

        private readonly static Dictionary<Type, ToolboxGroupDrawerBase> groupDrawers = new Dictionary<Type, ToolboxGroupDrawerBase>();

        private readonly static Dictionary<Type, ToolboxPropertyDrawerBase> propertyDrawers = new Dictionary<Type, ToolboxPropertyDrawerBase>();

        private readonly static Dictionary<Type, ToolboxConditionDrawerBase> conditionDrawers = new Dictionary<Type, ToolboxConditionDrawerBase>();


        [InitializeOnLoadMethod]
        internal static void InitializeSettings()
        {
            var guids = AssetDatabase.FindAssets("t:ToolboxEditorSettings");
            if (guids == null || guids.Length == 0) return;
            var path = AssetDatabase.GUIDToAssetPath(guids.First());
            settings = AssetDatabase.LoadAssetAtPath(path, typeof(ToolboxEditorSettings)) as ToolboxEditorSettings;
        }

        [InitializeOnLoadMethod]
        internal static void InitializeDrawers()
        {
            if (!settings)
            {
                Debug.LogWarning("EditorSettings not found. Cannot initialize ToolboxDrawers. " +
                                 "You can create new settings file using CreateAsset menu -> Create -> Toolbox Editor -> Settings.");
                return;
            }

            //local method used in drawer creation
            void CreateDrawer<T>(Type drawerType, Dictionary<Type, T> drawersCollection) where T : ToolboxDrawer
            {
                if (drawerType == null) return;
                //create desired drawer instance
                var drawer = Activator.CreateInstance(drawerType) as T;
                var targetType = GetTargetType(drawerType);
                if (drawersCollection.ContainsKey(targetType))
                {
                    Debug.LogWarning(targetType + " is already associated to more than one ToolboxDrawer.");
                    return;
                }
                drawersCollection.Add(targetType, drawer);
            }

            //local method used in search for proper target attributes
            Type GetTargetType(Type drawerType)
            {
                return drawerType.GetMethod("GetAttributeType", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).Invoke(null, null) as Type;
            }

            //iterate over all assigned area drawers, create them and store
            for (var i = 0; i < settings.AreaDrawersCount; i++)
            {
                CreateDrawer(settings.GetAreaDrawerTypeAt(i), areaDrawers);
            }

            //iterate over all assigned group drawers, create them and store
            for (var i = 0; i < settings.GroupDrawersCount; i++)
            {
                CreateDrawer(settings.GetGroupDrawerTypeAt(i), groupDrawers);
            }

            //iterate over all assigned property drawers, create them and store
            for (var i = 0; i < settings.PropertyDrawersCount; i++)
            {
                CreateDrawer(settings.GetPropertyDrawerTypeAt(i), propertyDrawers);
            }

            //iterate over all assigned condition drawers, create them and store
            for (var i = 0; i < settings.ConditionDrawersCount; i++)
            {
                CreateDrawer(settings.GetConditionDrawerTypeAt(i), conditionDrawers);
            }
        }

        [InitializeOnLoadMethod]
        internal static void InitializeEvents()
        {
            Selection.selectionChanged += onEditorReload;
        }


        internal static void ReimportEditor()
        {
            var guids = AssetDatabase.FindAssets("ToolboxEditorUtility");
            if (guids == null || guids.Length == 0) return;
            var path = AssetDatabase.GUIDToAssetPath(guids.First());
            AssetDatabase.ImportAsset(path);
        }


        internal static ToolboxAreaDrawerBase GetAreaDrawer<T>(T attribute) where T : ToolboxAreaAttribute
        {
            if (!areaDrawers.TryGetValue(attribute.GetType(), out ToolboxAreaDrawerBase drawer))
            {
                throw new AttributeNotSupportedException(attribute);
            }
            return drawer;
        }

        internal static ToolboxGroupDrawerBase GetGroupDrawer<T>(T attribute) where T : ToolboxGroupAttribute
        {
            if (!groupDrawers.TryGetValue(attribute.GetType(), out ToolboxGroupDrawerBase drawer))
            {
                throw new AttributeNotSupportedException(attribute);
            }
            return drawer;
        }

        internal static ToolboxPropertyDrawerBase GetPropertyDrawer<T>(T attribute) where T : ToolboxPropertyAttribute
        {
            if (!propertyDrawers.TryGetValue(attribute.GetType(), out ToolboxPropertyDrawerBase drawer))
            {
                throw new AttributeNotSupportedException(attribute);
            }
            return drawer;
        }

        internal static ToolboxConditionDrawerBase GetConditionDrawer<T>(T attribute) where T : ToolboxConditionAttribute
        {
            if (!conditionDrawers.TryGetValue(attribute.GetType(), out ToolboxConditionDrawerBase drawer))
            {
                throw new AttributeNotSupportedException(attribute);
            }
            return drawer;
        }


        internal static bool ToolboxDrawersAllowed => settings?.UseToolboxDrawers ?? false;

        internal static bool ToolboxHierarchyAllowed => settings?.UseToolboxHierarchy ?? false;


        internal static Action onEditorReload;
    }
}