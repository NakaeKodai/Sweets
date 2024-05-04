using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    private bool isEnter;
    HarvestPoint material;

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
            material.Harvesting();
            isEnter = false;
            Debug.Log("採集");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Material"))
        {
            material = other.gameObject.GetComponent<HarvestPoint>();
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
