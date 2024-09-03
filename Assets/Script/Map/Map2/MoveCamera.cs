using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    private Vector2 playerDirection; //プレイヤーの向き
    public float speed = 20; //移動の速さ
    [SerializeField] private MiniMapManager miniMapManager;
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(miniMapManager.isOpenMap)
        {
            playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();

            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            playerDirection.y * speed * Time.deltaTime,
            0);
        }
    }
}
