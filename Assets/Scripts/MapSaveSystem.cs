using UnityEngine;
using System.IO;

public static class MapSaveSystem
{
    private static string path = Application.persistentDataPath + "/mapdata.json";

    public static void SaveMap(MapData map)
    {
        string json = JsonUtility.ToJson(map, true);
        File.WriteAllText(path, json);
    }

    public static MapData LoadMap()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<MapData>(json);
        }

        return null;
    }

    public static void DeleteMap()
    {
        if (File.Exists(path)) File.Delete(path);
    }
}