using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetPreviewIconAttribute : PropertyAttribute, IMustBeSerialized {
        public readonly float Width;
        public readonly float Height;

        public AssetPreviewIconAttribute(float width = 128, float height = 128) {
            Width = width;
            Height = height;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssetPreviewIconAttribute))]
    public class PreviewIconDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);
            var singleLineHeight = EditorGUIUtility.singleLineHeight;

            var icon = AssetPreview.GetAssetPreview(property.objectReferenceValue);
            if (icon == null) {
                GUILayout.Height(singleLineHeight);
                return;
            }

            var propertyBoxedValue = (AssetPreviewIconAttribute)attribute;
            var iconWidth = propertyBoxedValue.Width;
            var iconHeight = propertyBoxedValue.Height;

            var iconRect = new Rect(position.x, position.y + singleLineHeight, iconWidth, iconHeight);

            GUI.DrawTexture(iconRect, icon);
            GUILayout.Space(iconHeight);
        }
    }
#endif
}