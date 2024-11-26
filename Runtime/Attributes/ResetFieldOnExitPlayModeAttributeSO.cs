using System.Collections.Generic;
using System.Reflection;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Attributes {
    public class ResetFieldOnExitPlayModeAttributeSO : ScriptableObject {
        [field: SerializeField]
        public SerializedDictionary<ScriptableObject, List<(FieldInfo fieldInfo, object fieldValue)>>
            InitialState { get; set; } = new();
    }
}