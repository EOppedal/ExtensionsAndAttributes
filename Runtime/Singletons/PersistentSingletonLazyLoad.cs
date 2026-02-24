using UnityEngine;

public class PersistentSingletonLazyLoad<T> : SingletonLazyLoad<T> where T : Component {
    protected override void InitializeSingleton() {
        transform.parent = null;

        if (instance == null) {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }
}