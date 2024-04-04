using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動のスクリプト
public class Move : MonoBehaviour
{
   
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 

    private Vector2 playerDirection; //プレイヤーの向き
    public byte speed = 5; //移動の速さ
    
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

        // 以下Raycastを利用した壁の判定
        Vector3 playerPosition = transform.position;//プレイヤーの位置を取得
        Vector3 rayDirection = new Vector3(playerDirection.x, 0, playerDirection.y);//プレイヤーの向きを取得
        Ray ray = new Ray(playerPosition, rayDirection);//プライヤーの向きにあるものを判別するものを飛ばす
        RaycastHit wallHit;
        if(!Physics.Raycast(ray, out wallHit, 1.0f, WallLayer)){//プレイヤーの方向の少し先にレイヤーの「Wall」がないかを判別する
            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            0.0f,
            playerDirection.y * speed * Time.deltaTime);
        }
        


    }

}
