using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    private bool isEnter = false;
    MakeSweets makeSweets;
    public int sweetsID;//作るスイーツの指定
    public Pause pauseScript;
    // Start is called before the first frame update
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
            // クリックによってスイーツを作成
            makeSweets.Cook(sweetsID);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cooking")
        {
            makeSweets = other.gameObject.GetComponent<MakeSweets>();
            isEnter = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Cooking")
        {
            isEnter = false;
        }
    }
}
