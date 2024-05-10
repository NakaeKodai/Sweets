using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public bool pause = false;
    [SerializeField] private GameObject menuWindow;
    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.UI.OpenMenu.triggered){
            if(pause){
                menuWindow.SetActive(false);
                Time.timeScale = 1;
                pause = false;
                Debug.Log("そして時は動き出す");
            } 
            else{
                menuWindow.SetActive(true);
                Time.timeScale = 0;
                pause = true;
                Debug.Log("ザ・ワールド！");
            } 
        }
    }
}
