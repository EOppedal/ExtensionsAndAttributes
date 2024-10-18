using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : PropertyAttribute, IMustBeSerialized {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);

            var targetType = property.serializedObject.targetObject.GetType();

            if (targetType == typeof(string) && !string.IsNullOrWhiteSpace(property.stringValue)) return;
            if (property.objectReferenceValue != null) return;

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.HelpBox(position, $"Field: '{property.name}' requires a valid reference!", MessageType.Warning);
        }
    }

#endif
}