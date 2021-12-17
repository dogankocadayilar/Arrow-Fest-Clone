using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveArrowController(ArrowController arrowController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ArrowController.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(arrowController);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerData LoadSave()
    {
        string path = Application.persistentDataPath + "/ArrowController.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
