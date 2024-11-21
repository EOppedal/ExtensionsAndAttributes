using System;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using ExtensionMethods;
using UnityEngine;
#endif

namespace Attributes {
    /// <summary>
    /// Must be accompanied by [SerializeField], but can still be hidden by [HideInInspector]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetOrAddComponentAttribute : Attribute, IMustBeSerialized {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void AddToMonobehaviourEditor() {
            MonoBehaviourEditor.OnInspectorGUIAttributes.Add(typeof(GetOrAddComponentAttribute), GetOrAddRequiredReference);
        }

        private static void GetOrAddRequiredReference(Editor editor, FieldInfo field) {
            var target = editor.target;
            var valueToAssign = ((MonoBehaviour)target).GetOrAdd(field.FieldType);
            field.SetValue(target, valueToAssign);
            EditorUtility.SetDirty(target);
        }
#endif
    }
}