using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    public bool isEnter;
    public byte attackPoint = 2;
    Enemy enemy;
    
    public Pause pauseScript;
    
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.Player.Fire.triggered && isEnter && !pauseScript.pause)
        {
            enemy.Damage(attackPoint);
            isEnter = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isEnter = false;
        }
    }
}
