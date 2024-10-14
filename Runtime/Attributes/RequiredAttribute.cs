using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : PropertyAttribute {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PropertyField(position, property, label);

            if (property.objectReferenceValue != null) return;

            EditorGUILayout.HelpBox($"Field: '{property.name}' requires a reference!", MessageType.Warning);
        }
    }
#endif
}