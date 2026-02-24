using UnityEngine;

[CreateAssetMenu(fileName = nameof(GlobalFloatSO), menuName = "Scriptable Objects/GlobalData/Create " + nameof(GlobalFloatSO))]
public class GlobalFloatSO : ScrubVariable<float> {
    public static implicit operator float(GlobalFloatSO globalFloatSOVariable) => globalFloatSOVariable.value;
}