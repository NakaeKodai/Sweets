using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //TextMeshProを使う場合は読み込みが必要
using UnityEngine.SceneManagement;

public class Talk : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 
    private bool isEnter;
    private bool isTalking;
    private bool interval = false;
    private byte talkPage;
    NPCState npcState;

    public GameObject talkWindow;
    [SerializeField] private TextMeshProUGUI textWindow;
    private bool windowStop = false;//画面に文字を表示させたかを判別

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
            textWindow = talkWindow.GetComponent<TextMeshProUGUI>();
            textWindow.text = npcState.list[talkPage];
            talkWindow.SetActive(true);
            if(!windowStop){
                StartCoroutine(Simple());
            }
            isTalking = true;
            interval = true;
        }
        playerInputAction.Player.Fire.canceled += ctx => interval = false;
        if(isTalking)
        {
            if(playerInputAction.Player.Fire.triggered && !interval){
                talkPage++;
                windowStop = false;
            } 
            if(talkPage >= npcState.list.Count) 
            {
                talkWindow.SetActive(false);
                isTalking = false;
            }
            else if(!windowStop)
            {
                textWindow = talkWindow.GetComponent<TextMeshProUGUI>();
                textWindow.text = npcState.list[talkPage];
                StartCoroutine(Simple());    
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

    // 文字送りのプログラム
    private IEnumerator Simple(){
        textWindow.maxVisibleCharacters = 0;
        windowStop = true;

        for (var i = 0; i < textWindow.text.Length; i++)
        {
            yield return new WaitForSeconds(0.03f);//ここのかっこの中の数値を変えることで文字送りのスピードを変えられる(秒)

            // 文字の表示数を増やしていく
            textWindow.maxVisibleCharacters = i + 1;
        }

        
    }   
}
