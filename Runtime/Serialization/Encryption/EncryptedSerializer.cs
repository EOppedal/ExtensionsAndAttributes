using Serializers;

namespace Encryption {
    /// <summary>
    /// An Encrypted Version Of A ISerializer.
    /// </summary>
    public sealed class EncryptedSerializer : ISerializer {
        public string FileExtension { get; }

        private readonly ISerializer _baseSerializer;

        public EncryptedSerializer(ISerializer basBaseSerializer = null) {
            _baseSerializer = basBaseSerializer ?? new JSONSerializer();
            FileExtension = _baseSerializer.FileExtension;
        }

        public string Serialize<T>(T obj) => _baseSerializer.Serialize(obj).Encrypted();

        public T Deserialize<T>(string fileText) => _baseSerializer.Deserialize<T>(fileText.Decrypted());
    }
}