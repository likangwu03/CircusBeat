using UnityEngine;

public class JsonSerializer : ISerializer
{
    public string FileExtension { get; protected set; } = ".json";

    public string Serialize(TrackerEvent e) 
    {
        return JsonUtility.ToJson(e);
    }
}
