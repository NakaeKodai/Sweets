using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //InputSystemを入れている変数
    private PlayerInputAction playerInputAction;  

    private Vector2 playerDirection;
    public float speed = 1.0f;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.Fire.triggered)
        {
            Debug.Log("攻撃！");
        }

        playerDirection = playerInputAction.Player.Move.ReadValue<Vector2>();
        transform.Translate(
            playerDirection.x * speed* Time.deltaTime,
            0.0f,
            playerDirection.y * speed* Time.deltaTime);
    }
}
