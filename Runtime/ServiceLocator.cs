using System;
using System.Collections.Generic;

public static class ServiceLocator {
    private static readonly Dictionary<Type, object> Services = new();
    private static readonly HashSet<Type> RegisteredTypes = new();

    public static void Register<T>(T service) {
        var type = typeof(T);
        if (!RegisteredTypes.Contains(type)) {
            Services.Add(type, service);
            RegisteredTypes.Add(type);
        } else {
            throw new InvalidOperationException($"{type.Name} is already registered.");
        }
    }

    public static void Unregister<T>() {
        var type = typeof(T);
        if (RegisteredTypes.Contains(type)) {
            Services.Remove(type);
            RegisteredTypes.Remove(type);
        }
    }

    public static T Get<T>() {
        return (T)Services.GetValueOrDefault(typeof(T));
    }
}