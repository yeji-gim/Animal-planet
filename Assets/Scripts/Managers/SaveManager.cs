using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static readonly string FILEPATH = Application.persistentDataPath + "/Save.save";
    public static void Save(GameSaveState save)
    {
        using (FileStream file = File.Create(FILEPATH))
        {
            new BinaryFormatter().Serialize(file,save);

        }
    }

    public static GameSaveState Load()
    {
        GameSaveState loadedSave = null;

        if (File.Exists(FILEPATH))
        {
            using (FileStream file = File.Open(FILEPATH, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                loadedSave = (GameSaveState)loadedData;
            }
        }
        return loadedSave;
    }

    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }
}
