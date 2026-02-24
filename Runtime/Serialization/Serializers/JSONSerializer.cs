using System;
using UnityEngine;

namespace Serializers {
    public sealed class JSONSerializer : ISerializer {
        public string FileExtension => ".json";

        public string Serialize<T>(T obj) {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return JsonUtility.ToJson(obj, true);
        }

        public T Deserialize<T>(string fileText) {
            if (string.IsNullOrEmpty(fileText)) throw new ArgumentNullException(nameof(fileText));
            return JsonUtility.FromJson<T>(fileText);
        }
    }
}