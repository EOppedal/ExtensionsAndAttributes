using System;
using System.Collections.Generic;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Attributes {
    public class ResetFieldOnExitPlayModeAttributeSO : ScriptableObject {
        [field: SerializeField]
        public SerializedDictionary<ScriptableObject, List<ScriptableObjectFields>> InitialState { get; set; } = new();
    }
    
    [Serializable] public record ScriptableObjectFields {
        public FieldInfo FieldInfo;
        public object FieldValue;

        public ScriptableObjectFields(FieldInfo fieldInfo, object fieldValue) {
            FieldInfo = fieldInfo;
            FieldValue = fieldValue;
        }
    }
}