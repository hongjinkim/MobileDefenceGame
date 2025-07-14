
public interface IJSONSerializer
{
    string Serialize<T>(T obj);

    T Deserialize<T>(string json);

}

