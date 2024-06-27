using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 position; //プレイヤーの位置
}

public class GameManager : MonoBehaviour
{

    [SerializeField] private Transform player;
    public bool pause = false;

    public List<int> wishList = new List<int>();//ウィッシュリスト

    void Start()
    {
        Load();
    }

    void Update()
    {
        //後に消す
        if(Input.GetKeyDown(KeyCode.K))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    public void Save()
    {
        SaveData data = new SaveData()
        {
            position = player.position
        };

        string json = JsonUtility.ToJson(data,true);
        Debug.Log(json);
        File.WriteAllText(GetFilePath(),json);

        // PlayerPrefs.SetString("SaveData", json);
        // PlayerPrefs.Save();
    }

    public void Load()
    {
        string path = GetFilePath();
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            player.position = data.position;
        }
        else
        {
            Debug.Log("SaveDataが存在しません");
        }
        // if(PlayerPrefs.HasKey("SaveData"))
        // {
        //     string json = PlayerPrefs.GetString("SaveData");
        //     SaveData data = JsonUtility.FromJson<SaveData>(json);
        //     player.position = data.position;
        // }
        // else
        // {
        //     Debug.Log("SaveDataが存在しません");
        // }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    // ウィッシュリストの同期
    public List<int> WishListSetting(){
        return wishList;
    }
}
