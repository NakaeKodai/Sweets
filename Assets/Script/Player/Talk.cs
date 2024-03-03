using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    private bool isEnter;
    private bool isTalking;
    private bool interval = false;
    private byte talkPage;
    NPCState npcState;

    public GameObject talkWindow;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();

        // Text textWindow = talkWindow.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //仮としてFireにしている
        if(playerInputAction.Player.Fire.triggered && isEnter && !isTalking)
        {
            talkPage = 0;
            Text textWindow = talkWindow.GetComponent<Text>();
            textWindow.text = npcState.list[talkPage];
            talkWindow.SetActive(true);
            isTalking = true;
            interval = true;
        }
        playerInputAction.Player.Fire.canceled += ctx => interval = false;
        if(isTalking)
        {
            if(playerInputAction.Player.Fire.triggered && !interval) talkPage++;
            if(talkPage >= npcState.list.Count) 
            {
                talkWindow.SetActive(false);
                isTalking = false;
            }
            else
            {
                Text textWindow = talkWindow.GetComponent<Text>();
                textWindow.text = npcState.list[talkPage];    
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            npcState = other.gameObject.GetComponent<NPCState>();
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isEnter = false;
        }
    }   
}
