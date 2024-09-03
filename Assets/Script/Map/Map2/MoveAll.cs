using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAll : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    private Vector2 playerDirection; //プレイヤーの向き
    public float speed = 5; //移動の速さ
    public float normalSpeed = 5;
    public float maxSpeed = 20;

    private bool isWalk;
    public bool isDush;
    [SerializeField] private MiniMapManager miniMapManager;
    
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!miniMapManager.isOpenMap)
        {
            playerInputAction.Player.Dash.performed += ctx => {
                speed = maxSpeed;
                isDush = true;
            };
            
            playerInputAction.Player.Dash.canceled += ctx => {
                speed = normalSpeed;
                isDush = false;
            };
            playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();

            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            playerDirection.y * speed * Time.deltaTime,
            0,Space.World);
        }
    }
}
