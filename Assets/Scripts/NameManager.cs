using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    public static string playerName;
    public static string bestPlayerName;
    public static int highScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScore();
    }

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int highScore;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = playerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            bestPlayerName = data.bestPlayerName;
        }
    }

}
