using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動のスクリプト
public class Move : MonoBehaviour
{
   
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 

    private Vector2 playerDirection; //プレイヤーの向き
    public byte speed = 5; //移動の速さ
    
    public static bool tachWall = false; //壁に当たっているかを判別
    // public static bool ta
    [SerializeField]LayerMask WallLayer;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    void Update()
    {
        playerInputAction.Player.Dash.performed += ctx => speed = 20;
        playerInputAction.Player.Dash.canceled += ctx => speed = 5;

        playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();
        // 以下Raycastを利用した壁の判定（未完成）
        Vector3 playerPosition = transform.position;
        Vector3 rayDirection = new Vector3(playerDirection.x, 0, playerDirection.y);
        // RaycastHit wallHit = Physics.Raycast(playerPosition, rayDirection, 5f,WallLayer);
        Ray ray = new Ray(playerPosition, rayDirection);
        Debug.DrawRay(playerPosition,rayDirection,Color.blue,0.5f);
        RaycastHit wallHit;
        if(!Physics.Raycast(ray, out wallHit, 1.0f, WallLayer)){
            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            0.0f,
            playerDirection.y * speed * Time.deltaTime);
        }
        


    }

    // Trigerを使用した判定（没）
    // void OnTriggerEnter(Collider collider){
    //     if(collider.tag == "Wall"){
    //         // tachWall = true;
    //         // Debug.Log("tachWallをオン");
    //         speed = 0;
    //     }
    // }
    // void OnTriggerStay(Collider collider){
    //     if(collider.tag == "Wall" && speed != 0){
    // //         tachWall = true;
    // //         Debug.Log("tachWallをオン");
    // speed = 0;
    //     }
    // }
    // void OnTriggerExit(Collider collider){
    //     if(collider.tag == "Wall"){
    //         // tachWall = false;
    //         // Debug.Log("tachWallをオフ");
    //         // speed = 5;
    //     }
    // }
}
