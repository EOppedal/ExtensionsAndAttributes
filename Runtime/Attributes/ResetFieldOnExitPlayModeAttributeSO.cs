using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Attributes {
    public class ResetFieldOnExitPlayModeAttributeSO : ScriptableObject {
        public Dictionary<ScriptableObject, List<ScriptableObjectFields>> InitialState { get; set; } = new();
    }
    
    [Serializable] public class ScriptableObjectFields {
        public FieldInfo FieldInfo;
        public object FieldValue;

        public ScriptableObjectFields(FieldInfo fieldInfo, object fieldValue) {
            FieldInfo = fieldInfo;
            FieldValue = fieldValue;
        }
    }
}