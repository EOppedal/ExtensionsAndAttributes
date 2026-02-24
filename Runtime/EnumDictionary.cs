using System;
using System.Collections.Generic;

public sealed record EnumDictionary<TEnum, TValue> where TEnum : Enum {
    private readonly Dictionary<TEnum, TValue> _dictionary = new();

    public TValue this[TEnum key] {
        get => _dictionary[key];
        set => _dictionary[key] = value; 
    }
}