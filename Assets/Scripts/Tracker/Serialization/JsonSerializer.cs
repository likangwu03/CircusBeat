using UnityEngine;

public class JsonSerializer : ISerializer
{
    public string FileExtension { get; protected set; } = ".json";

    public string Closer { get; protected set; } = "]";

    public string Opener { get; protected set; } = "[";

    public string Separator { get; protected set; } = ",";

    public string Serialize(TrackerEvent e) 
    {
        return JsonUtility.ToJson(e);
    }
}
