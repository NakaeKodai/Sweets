using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 position; //プレイヤーの位置
}

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Load();
    }

    [SerializeField] private Transform player;
    public bool pause = false;

    public void Save()
    {
        SaveData data = new SaveData()
        {
            position = player.position
        };

        string json = JsonUtility.ToJson(data,true);
        Debug.Log(json);

        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            player.position = data.position;
        }
        else
        {
            Debug.Log("SaveDataが存在しません");
        }
    }
}
