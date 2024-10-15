using System;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using ExtensionMethods;
using UnityEngine;
#endif

namespace Attributes {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredReferenceAttribute : Attribute {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void AddToMonobehaviourEditor() {
            MonoBehaviourEditor.OnInspectorGUIAttributes.Add(typeof(RequiredReferenceAttribute), GetOrAddRequiredReference);
        }

        private static void GetOrAddRequiredReference(Editor editor, FieldInfo field) {
            var valueToAssign = ((MonoBehaviour)editor.target).GetOrAdd(field.FieldType);
            field.SetValue(editor.target, valueToAssign);
        }
#endif
    }
}