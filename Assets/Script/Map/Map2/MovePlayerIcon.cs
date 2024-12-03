using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerIcon : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    private Vector2 playerDirection; //プレイヤーの向き
    [SerializeField] private MiniMapManager miniMapManager;
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
        if(!miniMapManager.isOpenMap)
        {
            playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();

            if (playerDirection != Vector2.zero)
            {
                // Vector2から角度を計算（ラジアンをデグリーに変換）
                float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg - 90;
                
                transform.rotation = Quaternion.Euler(new Vector3(90, 0, angle));
            }
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("HiddenObject"))
    //     {
    //         miniMapManager.RecordDestroyedObject(other.gameObject);
    //         Destroy(other.gameObject);
    //     }
    // }
}
