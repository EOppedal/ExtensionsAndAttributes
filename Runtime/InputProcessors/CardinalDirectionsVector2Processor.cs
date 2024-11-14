using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class CardinalDirectionsVector2Processor : InputProcessor<Vector2> {
    public Directions NumberOfDirections;

    public enum Directions { Four, Eight }

#if UNITY_EDITOR
    static CardinalDirectionsVector2Processor() {
        InputSystem.RegisterProcessor<CardinalDirectionsVector2Processor>();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() {
        InputSystem.RegisterProcessor<CardinalDirectionsVector2Processor>();
    }

    public override Vector2 Process(Vector2 value, InputControl control) {
        if (value == Vector2.zero) return value;

        var angle = Vector2.SignedAngle(Vector2.right, value);

        return NumberOfDirections switch {
            Directions.Four => FourDirections(angle),
            Directions.Eight => EightDirections(angle),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Vector2 FourDirections(in float angle) {
        return angle switch {
            >= -45f and < 45f => Vector2.right,
            >= 45f and < 135f => Vector2.up,
            >= 135f or < -135f => Vector2.left,
            >= -135f and < -45f => Vector2.down,
            _ => Vector2.zero
        };
    }

    private static Vector2 EightDirections(in float angle) {
        return angle switch {
            >= -22.5f and < 22.5f => Vector2.right,
            >= 22.5f and < 67.5f => new Vector2(1, 1),
            >= 67.5f and < 112.5f => Vector2.up,
            >= 112.5f and < 157.5f => new Vector2(-1, 1),
            >= 157.5f or < -157.5f => Vector2.left,
            >= -157.5f and < -112.5f => new Vector2(-1, -1),
            >= -112.5f and < -67.5f => Vector2.down,
            >= -67.5f and < -22.5f => new Vector2(1, -1),
            _ => Vector2.zero
        };
    }
}