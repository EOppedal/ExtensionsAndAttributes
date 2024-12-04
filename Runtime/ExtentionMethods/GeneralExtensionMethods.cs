using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = System.Random;

namespace ExtensionMethods {
    public static class GeneralExtensionMethods {
        /// <summary>
        /// Retrieves all immediate child GameObjects of a given Transform.
        /// </summary>
        /// <param name="parent">The parent Transform from which to get the immediate children.</param>
        /// <returns>An array of GameObjects that are the immediate children of the specified parent Transform.</returns>
        public static IEnumerable<GameObject> GetImmediateChildren(this Transform parent) {
            var childCount = parent.childCount;
            var children = new GameObject[childCount];

            for (var i = 0; i < childCount; i++) {
                children[i] = parent.GetChild(i).gameObject;
            }

            return children;
        }

        /// <summary>
        /// Converts a time in seconds to a formatted string representation.
        /// </summary>
        /// <param name="timeInSeconds">The time in seconds to convert.</param>
        /// <param name="format">The string format to use for the time representation.</param>
        /// <returns>A string representing the time in the specified format.</returns>
        public static string ToStringTimeFormat(this float timeInSeconds, string format) {
            return TimeSpan.FromSeconds(timeInSeconds).ToString(format);
        }

        public static T GetRandom<T>(this IReadOnlyList<T> list) {
            return list[new Random().Next(0, list.Count)];
        }

        public static void Shuffle<T>(this IList<T> list) {
            var i = list.Count;
            while (i > 1) {
                i--;
                var k = new Random().Next(i + 1);
                (list[k], list[i]) = (list[i], list[k]);
            }
        }

        public static T GetOrAdd<T>(this MonoBehaviour monoBehaviour, T componentParam = null) where T : Component {
            return monoBehaviour.TryGetComponent<T>(out var component)
                ? component
                : monoBehaviour.gameObject.AddComponent<T>();
        }

        public static Component GetOrAdd(this MonoBehaviour monoBehaviour, Type componentType) {
            var component = monoBehaviour.GetComponent(componentType);
            if (component == null) {
                component = monoBehaviour.gameObject.AddComponent(componentType);
            }

            return component;
        }

        public static AnimationClip GetCurrentAnimationClip(this Animator animator) {
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        }

        public static T DeepClone<T>(this T source) {
            var json = JsonUtility.ToJson(source, true);
            return JsonUtility.FromJson<T>(json);
        }

        public static T ShallowClone<T>(this T source) where T : class {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (typeof(T).IsValueType) return source;

            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return (T)source.GetType().GetMethod("MemberwiseClone", bindingFlags)?.Invoke(source, null);
        }

        public static void CallFunctionAsDynamic(this object @class, string functionName, object param) {
            var method = @class.GetType().GetMethod(functionName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, new[] { param.GetType() }, null);

            if (method != null) {
                method.Invoke(@class, new[] { param });
            }
            else {
                throw new ArgumentOutOfRangeException(nameof(param), $"No handler found for {param.GetType()}");
            }
        }
    }
}