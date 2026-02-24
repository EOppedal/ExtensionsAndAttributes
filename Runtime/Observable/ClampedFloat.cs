using System;
using UnityEngine;

[Serializable] public class ClampedFloat {
    public event Action<float> OnIncreaseValue = delegate { };
    public event Action<float> OnDecreaseValue = delegate { };
    public event Action<float> OnValueChanged = delegate { };

    public float minValue;

    [SerializeField] private float maxValue;

    public float MaxValue {
        get => maxValue;
        set {
            maxValue = value;
            OnValueChanged.Invoke(Value);
        }
    }

    [SerializeField] private float value;
    
    private bool _updateOnlyOnChangingValue;

    public float Value {
        get => value;
        set {
            var oldValue = this.value;
            this.value = Mathf.Clamp(value, minValue, MaxValue);

            if (_updateOnlyOnChangingValue) {
                if (Mathf.Approximately(this.value, oldValue)) return;
            }
            
            OnValueChanged.Invoke(this.value);

            if (value > oldValue) {
                OnIncreaseValue.Invoke(this.value);
            }
            else if (value < oldValue) {
                OnDecreaseValue.Invoke(this.value);
            }
        }
    }

    public ClampedFloat(float minValue = -Mathf.Infinity, float maxValue = Mathf.Infinity, float initialValue = 0, bool updateOnlyOnChangingValue = false) {
        this.minValue = minValue;
        this.maxValue = maxValue;
        _updateOnlyOnChangingValue = updateOnlyOnChangingValue;

        value = Mathf.Clamp(initialValue, minValue, maxValue);
    }

    public static implicit operator float(ClampedFloat clampedFloat) {
        return clampedFloat.Value;
    }

    public static implicit operator ClampedFloat(float value) {
        return new ClampedFloat(value, float.MinValue, float.MaxValue);
    }

    public static ClampedFloat operator +(ClampedFloat clampedFloat, float amount) {
        clampedFloat.Value += amount;
        return clampedFloat;
    }

    public static ClampedFloat operator -(ClampedFloat clampedFloat, float amount) {
        clampedFloat.Value -= amount;
        return clampedFloat;
    }
}