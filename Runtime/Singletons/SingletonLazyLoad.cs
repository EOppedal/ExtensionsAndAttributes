using UnityEngine;

public class SingletonLazyLoad<T> : Singleton<T> where T : Component {
    public new static T Instance {
        get {
            if (instance != null) return instance;

            instance = FindAnyObjectByType<T>();
            if (instance != null) return instance;

            var o = new GameObject(typeof(T).Name + " | Auto Generated");
            instance = o.AddComponent<T>();
            return instance;
        }
        protected set => instance = value;
    }
}