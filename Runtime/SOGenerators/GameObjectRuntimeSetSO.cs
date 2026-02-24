using Attributes;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameObjectRuntimeSetSO), menuName = "Scriptable Objects/GlobalData/Create " +  nameof(GameObjectRuntimeSetSO))]
public class GameObjectRuntimeSetSO : ScriptableObject {
    [ResetFieldOnExitPlayMode, SerializeField] public RuntimeSet<GameObject> runtimeSet;
}