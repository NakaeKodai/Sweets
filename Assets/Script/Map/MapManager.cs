using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public GameManager gameManager;

    public Talk talkScript;

    bool opening = false;

    public GameObject mapObject;
    public GameObject mapCursor;
    RectTransform mapTransform;
    private float cursorSpeed = 250.0f;
    RectTransform cursorTransform;

    private Vector2 mapDirection;
    float speedX = 0f;
    float speedY = 0f;

    Camera mainCamera;
    Vector3 mainCameraPos;
    public Camera mapCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();

        mapTransform = mapObject.GetComponent<RectTransform>();
        cursorTransform = mapCursor.GetComponent<RectTransform>();

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.OpenMap.triggered && !opening && !gameManager.pause && !talkScript.isTalking){
            // Time.timeScale = 0;
            gameManager.pause = true;
            opening = true;
            MapCreate();
            // mainCamera.enabled = false;
            // mapCamera.enabled = true;
            mainCameraPos = mainCamera.transform.position;
            // mapObject.SetActive(true);
            Debug.Log("マップ開いているよん");
        }else if((playerInputAction.Map.Cancel.triggered || playerInputAction.Player.OpenMap.triggered) && opening && gameManager.pause){
            Time.timeScale = 1;
            gameManager.pause = false;
            opening = false;
            // mainCamera.enabled = true;
            // mapCamera.enabled = false;
            mainCamera.transform.position = mainCameraPos;
            mapObject.SetActive(false);
            mapCursor.SetActive(false);
            Debug.Log("マップ閉じ");
        }

        if(opening){

            // mapDirection = playerInputAction.Map.Move.ReadValue<Vector2>();
            // mapTransform.RectTranslate(
            // mapDirection.x * -cursorSpeed * Time.deltaTime,
            // 0.0f,
            // mapDirection.y * -cursorSpeed * Time.deltaTime);

            MoveMap();
        }
    }

    void MapCreate(){


        mapObject.SetActive(true);
        mapCursor.SetActive(true);
    }

    void MoveMap(){
        // Transform mapTransform = mapObject.transform;
        // Vector2 mapPos = mapTransform.position;

        // playerInputAction.Map.CursorMoveRight.performed += ctx => {
        //     speedX = -cursorSpeed * Time.deltaTime;
        // };
        // playerInputAction.Map.CursorMoveRight.canceled += ctx => {
        //     speedX = 0;
        // };
        // playerInputAction.Map.CursorMoveLeft.performed += ctx => {
        //     speedX = cursorSpeed * Time.deltaTime;
        // };
        // playerInputAction.Map.CursorMoveLeft.canceled += ctx => {
        //     speedX = 0;
        // };
        // playerInputAction.Map.CursorMoveUp.performed += ctx => {
        //     speedY = -cursorSpeed * Time.deltaTime;
        // };
        // playerInputAction.Map.CursorMoveUp.canceled += ctx => {
        //     speedY = 0;
        // };
        // playerInputAction.Map.CursorMoveDown.performed += ctx => {
        //     speedY = cursorSpeed * Time.deltaTime;
        // };
        // playerInputAction.Map.CursorMoveDown.canceled += ctx => {
        //     speedY = 0;
        // };
        // mapPos.x += speedX;
        // mapPos.y += speedY;

        Vector2 mapPos = cursorTransform.position;

        playerInputAction.Map.CursorMoveRight.performed += ctx => {
            speedX = cursorSpeed * Time.deltaTime;
        };
        playerInputAction.Map.CursorMoveRight.canceled += ctx => {
            speedX = 0;
        };
        playerInputAction.Map.CursorMoveLeft.performed += ctx => {
            speedX = -cursorSpeed * Time.deltaTime;
        };
        playerInputAction.Map.CursorMoveLeft.canceled += ctx => {
            speedX = 0;
        };
        playerInputAction.Map.CursorMoveUp.performed += ctx => {
            speedY = cursorSpeed * Time.deltaTime;
        };
        playerInputAction.Map.CursorMoveUp.canceled += ctx => {
            speedY = 0;
        };
        playerInputAction.Map.CursorMoveDown.performed += ctx => {
            speedY = -cursorSpeed * Time.deltaTime;
        };
        playerInputAction.Map.CursorMoveDown.canceled += ctx => {
            speedY = 0;
        };
        mapPos.x += speedX;
        mapPos.y += speedY;
        cursorTransform.position = mapPos;
        // mainCamera.transform.position = mapPos;
    }

    public void KARI(){
        Debug.Log("おっぱい");
    }

    // void mapCameraSet(){
    //     mainCameraPos = mainCamera.transform.position;

    // }
}
