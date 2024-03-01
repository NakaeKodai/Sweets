using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
   
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 

    private Vector2 playerDirection; //プレイヤーの向き
    public byte speed = 5; //移動の速さ
    


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
        transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            0.0f,
            playerDirection.y * speed * Time.deltaTime);
    }
}
