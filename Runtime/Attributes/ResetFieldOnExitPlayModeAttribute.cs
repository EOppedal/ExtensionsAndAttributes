using System;
using ExtensionMethods;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.Assertions;
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
        private const string SOFolder = "ExtentionsAndAttributes";
        private const string SOName = "SceneCollectionManagerSO";
        private const string ResourcesPath = "Assets/Resources";

        private static ResetFieldOnExitPlayModeAttributeSO ResetFieldSO {
            get {
                if (_resetFieldSO != null) return _resetFieldSO;

                _resetFieldSO = ScrubUtils
                    .GetAllScrubsInResourceFolder<ResetFieldOnExitPlayModeAttributeSO>(SOFolder)
                    .GetByName(SOName);

                if (_resetFieldSO == null) {
                    var so = ScriptableObject.CreateInstance<ResetFieldOnExitPlayModeAttributeSO>();
                
                    var folderPath = Path.Join(ResourcesPath, SOFolder);

                    if (!Directory.Exists(folderPath)) {
                        Directory.CreateDirectory(folderPath);
                    }

                    var path = Path.Join(ResourcesPath, SOFolder, SOName) + ".asset";

                    AssetDatabase.CreateAsset(so, path);
                    Debug.Log($"{nameof(ResetFieldOnExitPlayModeAttributeSO)} file created");

                    _resetFieldSO = so;
                }
                
                Assert.IsNotNull(_resetFieldSO);

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

                var fieldValue = field.GetValue(scriptableObject);

                switch (fieldValue) {
                    case ICloneable cloneableValue:
                        fieldValue = cloneableValue.Clone();
                        Debug.Log("cloneable ");
                        break;
                    case IList list:
                        var elementType = list.GetType().GenericTypeArguments[0];
                        var genericListType = typeof(List<>).MakeGenericType(elementType);
                        var newList = (IList)Activator.CreateInstance(genericListType);
                            
                        Assert.AreNotEqual(list, newList);

                        foreach (var item in list) {
                            newList.Add(item);
                        }
                        
                        fieldValue = newList;
                        
                        Debug.Log($"Cloned list of type {elementType} with {newList.Count} elements");
                        break;
                }

                ResetFieldSO.InitialState[scriptableObject]
                    .Add(new ScriptableObjectFields(field, fieldValue));

                Debug.Log(fieldValue);
            }

            Debug.Log(LogPrefix + "Save Initial State | script count: " + ResetFieldSO.InitialState.Count);
        }

        private static void RestoreInitialState() {
            foreach (var (scriptableObject, value) in ResetFieldSO.InitialState) {
                foreach (var objectFields in value) {
                    var fieldValue = objectFields.FieldInfo.GetValue(scriptableObject);

                    switch (fieldValue) {
                        case IList list:
                            list.Clear();

                            foreach (var o in (IList)objectFields.FieldValue) {
                                list.Add(o);
                            }
                            
                            continue;
                    }
                    
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
                var hasClassAttribute =
                    objType.GetCustomAttributes(typeof(ResetFieldsOnExitPlayModeAttribute), true).Any();

                foreach (var field in objType.GetFields(bindingFlags)) {
                    if (hasClassAttribute ||
                        field.GetCustomAttribute(typeof(ResetFieldOnExitPlayModeAttribute)) != null) {
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