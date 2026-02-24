using UnityEngine;

[CreateAssetMenu(fileName = nameof(GlobalBoolSO), menuName = "Scriptable Objects/GlobalData/Create " + nameof(GlobalBoolSO))]
public class GlobalBoolSO : ScrubVariable<bool> {
    public static implicit operator bool(GlobalBoolSO globalBoolSOVariable) => globalBoolSOVariable.value;
}