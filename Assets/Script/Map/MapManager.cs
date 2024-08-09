using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public GameManager gameManager;

    bool opening = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.OpenMap.triggered && !opening && !gameManager.pause){
            Time.timeScale = 0;
            gameManager.pause = true;
            opening = true;
            Debug.Log("マップ開いているよん");
        }else if((playerInputAction.Map.Cancel.triggered || playerInputAction.Player.OpenMap.triggered) && opening && gameManager.pause){
            Time.timeScale = 1;
            gameManager.pause = false;
            opening = false;
            Debug.Log("マップ閉じ");
        }
    }
}
