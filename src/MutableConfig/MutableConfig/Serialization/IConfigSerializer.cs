namespace MutableConfig.Serialization {
    public interface IConfigSerializer {
        string SerializeObject<T>(T obj);
        T DeserializeObject<T>(string text);
    }
}