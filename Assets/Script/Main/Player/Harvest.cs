using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    private bool isEnter;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }


    void Update()
    {
        //仮としてFireにしている
        if(playerInputAction.Player.Fire.triggered && isEnter)
        {
            Debug.Log("採集");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Material"))
        {
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Material"))
        {
            isEnter = false;
        }
    }
}
