using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericRuntimeSetScrub<T> : ScriptableObject {
    public List<T> items = new ();
        
    public event Action<int> OnCountChanged;
        
    public int Count => items.Count;
        
    public void Add(T item) {
        if (!items.Contains(item)) items.Add(item);
        OnCountChanged?.Invoke(items.Count);
    }
        
    public void Remove(T item) {
        if (items.Contains(item)) items.Remove(item);
        OnCountChanged?.Invoke(items.Count);
    }
}