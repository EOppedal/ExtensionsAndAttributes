using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Attributes {
    public class ResetFieldOnExitPlayModeAttributeSO : ScriptableObject {
        public Dictionary<ScriptableObject,List<(FieldInfo fieldInfo, object fieldValue)>> InitialState { get; set; } =  new();
    }
}