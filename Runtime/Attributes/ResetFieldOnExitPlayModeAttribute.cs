using System;
using ExtensionMethods;
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
        #region ---Fields---
        private const string SOPath =
            "Packages/com.erlend-eiken-oppedal.extensionsandattributes/Runtime/Attributes/ResetFieldOnExitPlayModeInitialStateSO.asset";

        private static ResetFieldOnExitPlayModeAttributeSO ResetFieldSO {
            get {
                if (_resetFieldSO != null) return _resetFieldSO;

                _resetFieldSO = AssetDatabase.LoadAssetAtPath<ResetFieldOnExitPlayModeAttributeSO>(SOPath);

                if (_resetFieldSO == null) {
                    var so = ScriptableObject.CreateInstance<ResetFieldOnExitPlayModeAttributeSO>();

                    AssetDatabase.CreateAsset(so, SOPath);

                    _resetFieldSO = so;
                    Debug.Log($"{nameof(ResetFieldOnExitPlayModeAttributeSO)} file created");
                }

                Debug.Log($"{nameof(ResetFieldOnExitPlayModeAttributeSO)} loaded manually");

                return _resetFieldSO;
            }
        }

        private static ResetFieldOnExitPlayModeAttributeSO _resetFieldSO;

        private static readonly string LogPrefix = $"[{nameof(ResetFieldOnExitPlayModeAttribute)}]";
        #endregion

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
            ResetFieldSO.InitialState.Clear();

            foreach (var (scriptableObject, field) in GetAllResetFieldsOnExitPlayModeFields()) {
                if (!ResetFieldSO.InitialState.ContainsKey(scriptableObject)) {
                    ResetFieldSO.InitialState[scriptableObject] = new List<ScriptableObjectFields>();
                }

                ResetFieldSO.InitialState[scriptableObject]
                    .Add(new ScriptableObjectFields(field, field.GetValue(scriptableObject)));
                
                Debug.Log(field.GetValue(scriptableObject));
            }

            Debug.Log(LogPrefix + "Save Initial State | script count: " + ResetFieldSO.InitialState.Count);
        }

        private static void RestoreInitialState() {
            foreach (var (scriptableObject, value) in ResetFieldSO.InitialState) {
                foreach (var objectFields in value) {
                    objectFields.FieldInfo.SetValue(scriptableObject, default);
                    objectFields.FieldInfo.SetValue(scriptableObject, objectFields.FieldValue);
                    Debug.Log(objectFields.FieldValue);
                }

                EditorUtility.SetDirty(scriptableObject);
            }

            Debug.Log(LogPrefix + " Restore Initial State | script count:" + ResetFieldSO.InitialState.Count);

            ResetFieldSO.InitialState.Clear();
        }

        private static IEnumerable<(ScriptableObject target, FieldInfo field)> GetAllResetFieldsOnExitPlayModeFields() {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var scriptableObject in GetAllScriptableObjects()) {
                var objType = scriptableObject.GetType();
                var obj = scriptableObject.ShallowClone();
                var hasClassAttribute = objType.GetCustomAttributes(typeof(ResetFieldsOnExitPlayModeAttribute), true).Any();

                foreach (var field in objType.GetFields(bindingFlags)) {
                    if (hasClassAttribute || field.GetCustomAttribute(typeof(ResetFieldOnExitPlayModeAttribute)) != null) {
                        yield return (obj, field);
                    }
                }

                // var objType = obj.GetType();
                //
                // if (objType.GetCustomAttributes(typeof(ResetFieldsOnExitPlayModeAttribute), true).Any()) {
                //     foreach (var field in objType.GetFields(bindingFlags)) {
                //         yield return (obj, field);
                //     }
                // }
                // else {
                //     foreach (var field in objType.GetFields(bindingFlags).Where(field =>
                //                  field.GetCustomAttribute(typeof(ResetFieldOnExitPlayModeAttribute)) != null)) {
                //         yield return (obj, field);
                //     }
                // }
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