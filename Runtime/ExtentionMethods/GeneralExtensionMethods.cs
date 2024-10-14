using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Returns a random element from the list or array.
        /// </summary>
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
            return monoBehaviour.TryGetComponent<T>(out var component) ? component 
                : monoBehaviour.gameObject.AddComponent<T>();
        }
        
        public static Component GetOrAdd(this MonoBehaviour monoBehaviour, Type componentType) {
            var component = monoBehaviour.GetComponent(componentType);
            if (component == null) {
                component = monoBehaviour.gameObject.AddComponent(componentType);
            }
            return component;
        }

        /// <summary>
        /// Returns the current animation clip being played by the Animator
        /// </summary>
        public static AnimationClip GetCurrentAnimationClip(this Animator animator) {
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        }
    }
}