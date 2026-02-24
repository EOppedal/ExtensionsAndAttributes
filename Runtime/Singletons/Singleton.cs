using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
    protected static T instance;

    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance => instance;

    /// <summary>
    /// Make sure to call base.Awake() in override if you need to use awake for something else as well.
    /// </summary>
    protected virtual void Awake() {
        InitializeSingleton();
    }

    protected virtual void OnDestroy() {
        if (instance == this as T) {
            instance = null;
        }
    }

    protected virtual void InitializeSingleton() {
        if (instance == null) {
            instance = this as T;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
}