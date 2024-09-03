using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public GameManager gameManager;
    public bool isOpenMap = false; //マップを大きく開いているか

    public GameObject target;
    private Quaternion targetRotation;

    public GameObject cameraObject;
    public Camera mapCamera;
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        targetRotation = target.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.OpenMap.triggered && !isOpenMap && !gameManager.pause)
        {
            Debug.Log("おーきくなーる");
            isOpenMap = true;
            gameManager.pause = true;
            target.SetActive(true);
            target.transform.rotation = targetRotation;
            mapCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        }else if((playerInputAction.Map.Cancel.triggered || playerInputAction.Player.OpenMap.triggered) && isOpenMap && gameManager.pause)
        {
            isOpenMap = false;
            gameManager.pause = false;
            target.transform.localPosition = new Vector3(0, -1000, 48.5f);
            target.SetActive(false);
            cameraObject.transform.localPosition = new Vector3(0,-1000,-0.5f);
            mapCamera.rect = new Rect(0.04f, 0.04f, 0.2f, 0.2f);
        }
    }
}
