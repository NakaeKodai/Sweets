using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableTest : MonoBehaviour
{
    // シーンをまたいだ時に変数が保持されているかなどを確認するスクリプトです
    void Start()
    {
        for(int i = 0; i < BackpackManager.dataList.Count; i++){
            Debug.Log(BackpackManager.dataList[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
