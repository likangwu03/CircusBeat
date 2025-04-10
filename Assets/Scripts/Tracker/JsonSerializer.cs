using UnityEngine;

public class JsonSerializer: ISerializer
{
    public string Serialize(TrackerEvent e)
    {
        return JsonUtility.ToJson(e);
    }

   
}
