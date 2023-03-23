using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // save the party stats \\
    public static void SaveTime(float[] highscores) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/timeLeaderboard.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        TimeData data = new TimeData(highscores);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // load the party stats \\
    public static TimeData LoadTimeLeaderboard() {
        string path = Application.persistentDataPath + "/timeLeaderboard.txt";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TimeData data = formatter.Deserialize(stream) as TimeData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    // delete save data \\
    public static void deleteSaveData() {
        string timeDataPath = Application.persistentDataPath + "/timeLeaderboard.txt";

        // delete the party data
        if (File.Exists(timeDataPath)) {
            File.Delete(timeDataPath);
        }
        else {
            Debug.LogError("File not found in " + timeDataPath);
        }
    }
}
