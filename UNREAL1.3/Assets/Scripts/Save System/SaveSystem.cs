using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/player.data";

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //string path = "D:/Backup File 04072019/Desktop/Project/UNREAL/Data" + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
        
        Debug.Log("Save path is " + path);
    }

    public static PlayerData LoadData()
    {
        //string path = "D:/Backup File 04072019/Desktop/Project/UNREAL/Data" + "/player.data";
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
            Debug.Log("Not File found in " + path);
            return null;
        }
    }

    public static void DeleteData()
    {
        //string path = "D:/Backup File 04072019/Desktop/Project/UNREAL/Data" + "/player.data";
        if (File.Exists(path))
        {
            File.Delete(path);
            database.set_LoggedIn_false_online();
            Debug.Log("File is deleted");
        }
        else
        {
            Debug.Log("Not File found in " + path);
        }
    }
}
