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
    RectTransform mapTransform;
    private float cursorSpeed = 3000.0f;

    private Vector2 mapDirection;

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();

        mapTransform = mapObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.OpenMap.triggered && !opening && !gameManager.pause && !talkScript.isTalking){
            // Time.timeScale = 0;
            gameManager.pause = true;
            opening = true;
            MapCreate();
            // mapObject.SetActive(true);
            Debug.Log("マップ開いているよん");
        }else if((playerInputAction.Map.Cancel.triggered || playerInputAction.Player.OpenMap.triggered) && opening && gameManager.pause){
            Time.timeScale = 1;
            gameManager.pause = false;
            opening = false;
            mapObject.SetActive(false);
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
    }

    void MoveMap(){
        // Transform mapTransform = mapObject.transform;
        Vector2 mapPos = mapTransform.position;
        if(playerInputAction.Map.CursorMoveRight.triggered){
            mapPos.x += -cursorSpeed * Time.deltaTime;
            mapTransform.position = mapPos;
            // mapTransform.position = new Vector2(-cursorSpeed,0f);
            // transform.position = Vecter3.MoveTowards(transform.position, pos, -cursorSpeed*Time.deltaTime);
            // mapTransform.Translate(-cursorSpeed*Time.deltaTime,0f,0f);
        }else if(playerInputAction.Map.CursorMoveLeft.triggered){
            mapPos.x += cursorSpeed * Time.deltaTime;
            mapTransform.position = mapPos;
            // mapTransform.position  = new Vector2(cursorSpeed * Time.deltaTime,0f);
            // transform.position = Vecter3.MoveTowards(transform.position, pos, cursorSpeed*Time.deltaTime);
            // mapTransform.Translate(cursorSpeed*Time.deltaTime,0f,0f);
        }
    }
}
