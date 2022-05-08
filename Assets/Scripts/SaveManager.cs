using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{ 
    public static void SavePlayerData(PlayerData data) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game data saved!");
    }

    public static PlayerData LoadPlayerData() 
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;

        } else 
        {
            Debug.LogWarning("Save file not found in: " + path);
            return null;
        }
    }
    
    public static void SaveConfigData(ConfigData data) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/config.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game data saved!");
    }

    public static ConfigData LoadConfigData() 
    {
        try
        {
            string path = Application.persistentDataPath + "/config.dat";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                ConfigData data = formatter.Deserialize(stream) as ConfigData;
                stream.Close();

                return data;

            }
            else
            {
                Debug.LogWarning("Save file not found in: " + path);
                return null;
            }
        } catch(System.Exception e) 
        {
            return null;
        }
    }

    public static void ClearPlayerData() 
    {
        SavePlayerData(new PlayerData(0));
    }
}
