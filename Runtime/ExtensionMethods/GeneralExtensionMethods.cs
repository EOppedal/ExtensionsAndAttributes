using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods {
    public static class GeneralExtensionMethods {
        private static readonly System.Random Random = new();

        public static IEnumerable<GameObject> GetImmediateChildren(this Transform parent) {
            var childCount = parent.childCount;

            for (var i = 0; i < childCount; i++) {
                yield return parent.GetChild(i).gameObject;
            }
        }
        
        public static IEnumerable<Transform> GetImmediateChildrenTransforms(this Transform parent) {
            var childCount = parent.childCount;

            for (var i = 0; i < childCount; i++) {
                yield return parent.GetChild(i);
            }
        }

        public static string ToStringTimeFormat(this float timeInSeconds, string format) {
            return TimeSpan.FromSeconds(timeInSeconds).ToString(format);
        }

        /// <summary>
        /// Returns a random element from the given list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to select a random element from.</param>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public static T GetRandom<T>(this IReadOnlyList<T> list) {
            return list[Random.Next(0, list.Count)];
        }

        /// <summary>
        /// Randomly shuffles the elements of the given list in place.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> list) {
            var i = list.Count;
            
            while (i > 1) {
                i--;
                var k = Random.Next(i + 1);
                (list[k], list[i]) = (list[i], list[k]);
            }
        }


        public static T GetOrAdd<T>(this MonoBehaviour monoBehaviour) where T : Component {
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
        
        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
            transform.position = new Vector3(x ?? transform.position.x, y ?? transform.position.y, z ?? transform.position.z);
        }
    }
}