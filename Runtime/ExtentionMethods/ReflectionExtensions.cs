using System;
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

        public static T[] GetFieldsOfType<T>(this object instance) {
            return instance.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.FieldType == typeof(T))
                .Select(f => (T)f.GetValue(instance))
                .ToArray();
        }

        private static T GetFieldByNameAndType<T>(this object instance, string fieldName) {
            var field = instance.GetType()
                .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return field is T field1 ? field1 : default;
        }
    }
}