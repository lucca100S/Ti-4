using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/save.json";

    public static void SaveToFile(SaveableData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("SAVE gravado em: " + path);
    }

    public static SaveableData LoadFromFile()
    {
        if (!File.Exists(path))
        {
            Debug.Log("Nenhum save encontrado. Criando novo.");
            return new SaveableData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveableData>(json);
    }

    public static void ResetSave()
    {
        if (File.Exists(path))
            File.Delete(path);

        Debug.Log("Save RESETADO.");
    }
}
