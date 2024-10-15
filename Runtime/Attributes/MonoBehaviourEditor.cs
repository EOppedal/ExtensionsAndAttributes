#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Attributes {
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor {
        public static readonly Dictionary<Type, Action<Editor, FieldInfo>> OnInspectorGUIAttributes = new();
        public static readonly Dictionary<Type, Action<Editor, FieldInfo>> OnValidateAttributes = new();

        public override void OnInspectorGUI() {
            ExecuteFunctionForAttributesInDictionary(OnInspectorGUIAttributes);

            DrawDefaultInspector();
        }

        private void OnValidate() {
            ExecuteFunctionForAttributesInDictionary(OnValidateAttributes);
        }

        private void ExecuteFunctionForAttributesInDictionary(Dictionary<Type, Action<Editor, FieldInfo>> dictionary) {
            var targetType = target.GetType();
            var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields) {
                foreach (var attribute in field.GetCustomAttributes()) {
                    var attributeType = attribute.GetType();

                    foreach (var fieldType in dictionary.Keys.Where(t => t.IsAssignableFrom(attributeType))) {
                        dictionary[fieldType].Invoke(this, field);
                    }

                    if (dictionary.ContainsKey(attributeType)) {
                        dictionary[attributeType].Invoke(this, field);
                    }
                }
            }
        }
    }
}
#endif