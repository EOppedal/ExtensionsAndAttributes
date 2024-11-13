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
            
            if (!IsValueNullOrEmpty(property)) return;
            
            position.y += EditorGUIUtility.singleLineHeight;
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            EditorGUI.HelpBox(position, $"Field: '{property.name}' requires a reference or value!", MessageType.Warning);
        }
        
        private static bool IsValueNullOrEmpty(SerializedProperty property) {
            return property.propertyType switch {
                SerializedPropertyType.ObjectReference => property.objectReferenceValue == null,
                SerializedPropertyType.String => string.IsNullOrEmpty(property.stringValue),
                SerializedPropertyType.ArraySize => property.arraySize == 0,
                SerializedPropertyType.AnimationCurve => property.animationCurveValue.length !> 0,
                _ => false
            };
        }
    }
#endif
}