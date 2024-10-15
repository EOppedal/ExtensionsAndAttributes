using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class MustBeSerializedAttribute : PropertyAttribute, IMustBeSerialized {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void AddToMonobehaviourEditor() {
            MonoBehaviourEditor.OnInspectorGUIAttributes.Add(typeof(IMustBeSerialized), MustBeSerialized);
        }

        private static void MustBeSerialized(Editor editor, FieldInfo fieldInfo) {
            if (!(fieldInfo.IsPublic || IsDefined(fieldInfo, typeof(SerializeField)))) {
                EditorGUILayout.HelpBox($"Field: '{fieldInfo.Name}' must be serialized!", MessageType.Warning);
            }
        }
#endif
    }

    public interface IMustBeSerialized {

    }
}