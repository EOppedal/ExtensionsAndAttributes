using UnityEngine;

[CreateAssetMenu(fileName = nameof(GlobalIntSO), menuName = "Scriptable Objects/GlobalData/Create " + nameof(GlobalIntSO))]
public class GlobalIntSO : ScrubVariable<int> {
    public static implicit operator int(GlobalIntSO globalIntSOVariable) => globalIntSOVariable.value;
}