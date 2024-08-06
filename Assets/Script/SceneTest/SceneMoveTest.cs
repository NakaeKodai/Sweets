using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveTest : MonoBehaviour
{
    //シーンを切り替えるテスト用のスクリプトです

    private PlayerInputAction playerInputAction;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.MoveSceneTest.triggered)
        {
            SceneManager.LoadScene("MiniGame");
        }
    }
}
