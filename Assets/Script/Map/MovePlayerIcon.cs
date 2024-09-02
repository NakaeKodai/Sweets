using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerIcon : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    private Vector2 playerDirection; //プレイヤーの向き
    public float speed = 5; //移動の速さ
    public float normalSpeed = 5;
    public float maxSpeed = 20;

    private bool isWalk;
    public bool isDush;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.pause)
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

            if (playerDirection != Vector2.zero)
            {
                // Vector2から角度を計算（ラジアンをデグリーに変換）
                float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg - 90;
                
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            }

            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            playerDirection.y * speed * Time.deltaTime,
            0,Space.World);
        }
    }
}
