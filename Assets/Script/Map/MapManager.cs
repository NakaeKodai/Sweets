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

    [Header("主人公の場所関連")]
    [SerializeField]private GameObject playerMark;
    private RectTransform playerMarkTransform;
    [SerializeField]private Vector2 playerPoint;//マップの左端を表す
    [SerializeField]private GameObject playerPosition;//プレイヤーの位置
    Vector2 playerPositionXZ;//二次元

    [Header("マップ隠し関連")]
    [SerializeField]private GameObject hiddenMap;

    [System.Serializable]public class hideMass{
        [SerializeField] public int left;
        [SerializeField] public int right;
        [SerializeField] public int top;
        [SerializeField] public int under;
        [SerializeField] public bool opened;
    }
    [SerializeField]public List<hideMass> hideMassList = new List<hideMass>();


    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();

        mapTransform = mapObject.GetComponent<RectTransform>();
        cursorTransform = mapCursor.GetComponent<RectTransform>();

        playerMarkTransform = playerMark.GetComponent<RectTransform>();

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
        playerPositionXZ.x = playerPosition.transform.position.x;
        playerPositionXZ.y = playerPosition.transform.position.z;
        Debug.Log(playerPositionXZ.x+":"+playerPositionXZ.y);

        // playerMark.RectTransform = new Vector2(playerPositionXZ.x, playerPositionXZ.y);
        // Debug.Log(playerMark.transform.position.x+":"+playerMark.transform.position.y);
        Vector2 MarkPos = playerMarkTransform.position;
        MarkPos = new Vector2(playerPositionXZ.x+160, playerPositionXZ.y+130);
        Debug.Log(MarkPos.x+":"+MarkPos.y);
        playerMarkTransform.position = MarkPos;
        // playerMark.transform.x = playerPositionXZ.x-playerPoint.x;
        // playerMark.transform.y = playerPositionXZ.y-playerPoint.y;
        HideMap();
        mapObject.SetActive(true);
        mapCursor.SetActive(true);
    }

    void HideMap(){
        GameObject mass;
        for(int i = 0; i < hideMassList.Count; i++){
            mass = hiddenMap.transform.GetChild(i).gameObject;
            if(hideMassList[i].opened) mass.SetActive(false);
        }
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
