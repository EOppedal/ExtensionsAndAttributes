using System;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
#endif

namespace Attributes {
    /// <summary>
    /// Only works on Fields in a ScriptableObject in the 'Resources' folder
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResetFieldOnExitPlayModeAttribute : PropertyAttribute {
    }

    /// <summary>
    /// Only works on ScriptableObjects in the 'Resources' folder
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResetFieldsOnExitPlayModeAttribute : PropertyAttribute {
    }

#if UNITY_EDITOR
    [InitializeOnLoad] public static class ResetOnExitPlayModeHandler {
        private const string SOPath =
            "Packages/com.erlend-eiken-oppedal.extensionsandattributes/Runtime/Attributes/ResetFieldOnExitPlayModeInitialStateSO.asset";

        private static ResetFieldOnExitPlayModeAttributeSO ResetFieldSO {
            get {
                if (_resetFieldSO == null) {
                    _resetFieldSO = AssetDatabase.LoadAssetAtPath<ResetFieldOnExitPlayModeAttributeSO>(SOPath);

#if UNITY_EDITOR
                    if (_resetFieldSO == null) {
                        var so = ScriptableObject.CreateInstance<ResetFieldOnExitPlayModeAttributeSO>();

                        AssetDatabase.CreateAsset(so, SOPath);

                        _resetFieldSO = so;
                        Debug.Log($"{nameof(ResetFieldOnExitPlayModeAttributeSO)} file created");
                    }
#endif

                    Debug.Log($"{nameof(ResetFieldOnExitPlayModeAttributeSO)} loaded manually");
                }

                return _resetFieldSO;
            }
        }

        private static ResetFieldOnExitPlayModeAttributeSO _resetFieldSO;

        private static readonly string LogPrefix = $"[{nameof(ResetFieldOnExitPlayModeAttribute)}]";

        private static Dictionary<ScriptableObject, List<(FieldInfo fieldInfo, object fieldValue)>>
            InitialState => ResetFieldSO.InitialState;

        static ResetOnExitPlayModeHandler() {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            switch (state) {
                case PlayModeStateChange.EnteredPlayMode:
                    SaveInitialState();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    RestoreInitialState();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingEditMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private static void SaveInitialState() {
            InitialState.Clear();

            foreach (var (scriptableObject, field) in GetAllResetFieldsOnExitPlayModeFields()) {
                if (!InitialState.ContainsKey(scriptableObject)) {
                    InitialState[scriptableObject] = new List<(FieldInfo, object)>();
                }

                InitialState[scriptableObject].Add((field, field.GetValue(scriptableObject)));
            }

            Debug.Log(LogPrefix + "Save Initial State | script count: " + InitialState.Count);
        }

        private static void RestoreInitialState() {
            foreach (var (scriptableObject, value) in InitialState) {
                foreach (var (fieldInfo, originalValue) in value) {
                    fieldInfo.SetValue(scriptableObject, default);
                    fieldInfo.SetValue(scriptableObject, originalValue);
                }

                EditorUtility.SetDirty(scriptableObject);
            }

            Debug.Log(LogPrefix + " Restore Initial State | script count:" + InitialState.Count);

            InitialState.Clear();
        }

        private static IEnumerable<(ScriptableObject target, FieldInfo field)> GetAllResetFieldsOnExitPlayModeFields() {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var obj in GetAllScriptableObjects()) {
                var objType = obj.GetType();

                if (objType.GetCustomAttributes(typeof(ResetFieldsOnExitPlayModeAttribute), true).Any()) {
                    foreach (var field in objType.GetFields(bindingFlags)) {
                        yield return (obj, field);
                    }
                }
                else {
                    foreach (var field in objType.GetFields(bindingFlags).Where(field =>
                                 field.GetCustomAttribute(typeof(ResetFieldOnExitPlayModeAttribute)) != null)) {
                        yield return (obj, field);
                    }
                }
            }
        }

        private static ScriptableObject[] GetAllScriptableObjects() {
            return Resources.FindObjectsOfTypeAll<ScriptableObject>();

            // var assetGuids = AssetDatabase.FindAssets("t:ScriptableObject");
            // return assetGuids
            //     .Select(AssetDatabase.GUIDToAssetPath)
            //     .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
            //     .ToArray();
        }
    }
#endif
}