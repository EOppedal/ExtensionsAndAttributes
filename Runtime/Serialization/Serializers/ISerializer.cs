namespace Serializers {
    public interface ISerializer {
        /// <summary>
        /// Example Value: ".json" or ".xml"
        /// </summary>
        public string FileExtension { get; }

        string Serialize<T>(T obj);
        T Deserialize<T>(string fileText);
    }
}