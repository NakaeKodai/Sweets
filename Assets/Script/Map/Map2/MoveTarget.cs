using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    private Vector2 playerDirection; //プレイヤーの向き
    private CheckPointInfo check;

    private bool canFastTravel = false;
    public Transform player;
    public Transform moveObject;
    public float x,y;
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
            x = playerDirection.x;
            y = playerDirection.y;

            transform.Translate(
            playerDirection.x * speed * Time.deltaTime,
            playerDirection.y * speed * Time.deltaTime,
            0);
        }

        if(playerInputAction.Map.MenuSelect.triggered && canFastTravel)
        {
            Debug.Log("いどー");
            player.position = new Vector3(check.x, check.y, check.z);
            moveObject.position = new Vector2(check.iconX, check.iconY);
            miniMapManager.CloseMap();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Checkpoint"))
        {
            check = other.GetComponent<CheckPointInfo>();
            canFastTravel = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Checkpoint"))
        {
            check = null;
            canFastTravel = false;
        }
    }
}
