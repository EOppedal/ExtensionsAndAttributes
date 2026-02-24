using Attributes;
using UnityEngine;

[ResetFieldsOnExitPlayMode]
public abstract class ScrubVariable<T> : ScriptableObject {
    [SerializeField] public T value;
}