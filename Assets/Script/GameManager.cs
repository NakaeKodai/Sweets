using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Rendering;

[System.Serializable]
public class SaveData
{
    public Vector3 position; //プレイヤーの位置
    public float[] scrolbarValue; //現在　0はLight 1はAudio
    public List<string> destroyObjectNames;
}

public class GameManager : MonoBehaviour
{
    public PlayerInputAction playerInputAction;

    [SerializeField] private Transform player;
    [SerializeField] SettingManager settingManager;
    [SerializeField] Image lightImage;
    public bool pause = false;
    public List<string> destroyObjects = new List<string>();

    public static List<int> wishList = new List<int>();//ウィッシュリスト

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
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
            position = player.position,
            scrolbarValue = new float[settingManager.scrollbar.Count]
        };

        for(int i = 0;i < data.scrolbarValue.Length; i++)
        {
            data.scrolbarValue[i] = settingManager.scrollbar[i].value;
        }

        data.destroyObjectNames = destroyObjects;

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
            for(int i = 0;i < data.scrolbarValue.Length; i++)
            {
                settingManager.scrollbar[i].value = data.scrolbarValue[i];
                switch(i)
                {
                    case 0:
                    Color color = lightImage.color;
                    color.a = settingManager.scrollbar[i].value;
                    if(color.a >= 0.96f) color.a = 0.95f;
                    lightImage.color = color;
                    //RenderSettings.ambientIntensity = SettingManager.scrollbar[i].value;
                    break;
                }
            }

            foreach(string obectName in data.destroyObjectNames)
            {
                GameObject obj = GameObject.Find(obectName);
                if(obj != null)
                {
                    Destroy(obj);
                }
            }
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
        // Save();
    }

    // ウィッシュリストの同期
    public List<int> WishListSetting(){
        return wishList;
    }

    // マップを隠しているやつのセーブ
    public void UpdateDestroyObject(List<string> list)
    {
        destroyObjects = list;
    }
}
