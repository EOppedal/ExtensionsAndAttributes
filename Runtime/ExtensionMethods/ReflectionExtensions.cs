using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtensionMethods {
    public static class ReflectionExtensions {
        public static T ShallowClone<T>(this T source) where T : class {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (typeof(T).IsValueType) return source;

            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return (T)source.GetType().GetMethod("MemberwiseClone", bindingFlags)?.Invoke(source, null);
        }

        /// <summary>
        /// Calls the specified function name on the given class instance if it exists and has a single parameter
        /// of type <paramref name="param"/>. If not, throws an exception.
        /// </summary>
        /// <param name="class">The class instance to call the method on.</param>
        /// <param name="functionName">Name of the function to call.</param>
        /// <param name="param">The parameter to pass to the method.</param>
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

        /// <summary>
        /// Gets all public and private fields of type T in an instance.
        /// </summary>
        /// <param name="instance">The object to search for fields.</param>
        /// <typeparam name="T">The type of the fields to search for.</typeparam>
        /// <returns>All public and private fields of type T in an instance.</returns>
        public static IEnumerable<T> GetFieldsOfType<T>(this object instance) {
            return instance.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.FieldType == typeof(T))
                .Select(f => (T)f.GetValue(instance));
        }

        /// <summary>
        /// Retrieves the value of a field with the specified name and type from an object instance.
        /// </summary>
        /// <typeparam name="T">The expected type of the field value.</typeparam>
        /// <param name="instance">The object from which to retrieve the field value.</param>
        /// <param name="fieldName">The name of the field to retrieve.</param>
        /// <returns>The value of the field if found and of the correct type; otherwise, the default value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="fieldName"/> is null or empty.</exception>
        public static T GetFieldByNameAndType<T>(this object instance, string fieldName) {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (string.IsNullOrEmpty(fieldName)) throw new ArgumentException("Field name cannot be null or empty", nameof(fieldName));

            var instanceType = instance.GetType();
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var field = instanceType.GetField(fieldName, bindingFlags);

            if (field == null || !typeof(T).IsAssignableFrom(field.FieldType)) {
                return default;
            }

            return (T)field.GetValue(instance);
        }
    }
}