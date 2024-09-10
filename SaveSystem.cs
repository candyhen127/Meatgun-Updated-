using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveMeta(MetaDataManager m)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.yay";
        FileStream stream = new FileStream(path, FileMode.Create);

        MetaData data = new MetaData(m);

        formatter.Serialize(stream, data);

        stream.Close();    
    }

    public static MetaData LoadMeta()
    {
        string path = Application.persistentDataPath + "/savefile.yay";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            MetaData data = formatter.Deserialize(stream) as MetaData;
            
            return data;
            stream.Close();
        }
        else
        {
            return null;
            
        }
    }
}
