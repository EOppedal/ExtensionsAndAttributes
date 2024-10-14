using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class MustBeSerializedAttribute : PropertyAttribute, IMustBeSerialized {
    }
    
    public interface IMustBeSerialized {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MustBeSerializedEditor : Editor {
        public override void OnInspectorGUI() {
            var targetType = target.GetType();
            var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
    
            foreach (var field in fields) {
                var attributes = field.GetCustomAttributes(typeof(IMustBeSerialized), true);
                if (attributes.Length <= 0) continue;
    
                var isSerialized = field.IsPublic || Attribute.IsDefined(field, typeof(SerializeField));
    
                if (!isSerialized) {
                    EditorGUILayout.HelpBox($"Field: '{field.Name}' must be serialized!", MessageType.Warning);
                }
            }
    
            DrawDefaultInspector();
        }
    }
#endif
}