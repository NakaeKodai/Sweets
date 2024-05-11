using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;

    private bool isMenu;//メニューウィンドウが開いているか
    private PlayerInputAction playerInputAction;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private Button[] menuList;//メニューウィンドウのボタンを格納しているリスト
    private int selectBottuonNum;//選択中のボタンのリスト番号
    private Button selectBottuon;//今選択中のボタン
    public bool selectMenuNow;//メニュー項目が選択されているかを判別
    public Color nomalColor;
    public Color selectColor;

    public Backpack backpackScript;

    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.pause && playerInputAction.UI.OpenMenu.triggered){
            menuWindow.SetActive(true);
                Time.timeScale = 0;
                gameManager.pause = true;
                isMenu = true;
                selectBottuonNum = 0;
                Debug.Log("ザ・ワールド！");
        }
        else if((gameManager.pause && playerInputAction.UI.OpenMenu.triggered) || (gameManager.pause && playerInputAction.UI.Cancel.triggered && !selectMenuNow)){
                menuWindow.SetActive(false);
                Time.timeScale = 1;
                gameManager.pause = false;
                isMenu = false;
                selectBottuon = menuList[selectBottuonNum];
                Image buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                selectMenuNow = false;
                Debug.Log("そして時は動き出す");
        } 
        

        if(isMenu && !selectMenuNow)
        {
            selectBottuon = menuList[selectBottuonNum];
            Image buttonImage = selectBottuon.GetComponent<Image>();
            buttonImage.color = selectColor;
            if(playerInputAction.UI.CursorMoveUp.triggered)
            {
                selectBottuon = menuList[selectBottuonNum];
                buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                selectBottuonNum --;
                if(selectBottuonNum < 0)
                {
                    selectBottuonNum = menuList.Length - 1;
                } 
                // selectBottuon = menuList[selectBottuonNum];
                // buttonImage = selectBottuon.GetComponent<Image>();
                // buttonImage.color = selectColor;
            }

            if(playerInputAction.UI.CursorMoveDown.triggered)
            {
                selectBottuon = menuList[selectBottuonNum];
                buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                selectBottuonNum ++;
                if(selectBottuonNum >= menuList.Length)
                {
                    selectBottuonNum = 0;
                } 
                // selectBottuon = menuList[selectBottuonNum];
                // buttonImage = selectBottuon.GetComponent<Image>();
                // buttonImage.color = selectColor;
            }

            // アイテムを開く項目
            if(selectBottuonNum == 0 && playerInputAction.UI.MenuSelect.triggered){
                selectMenuNow = true;
                Debug.Log("アイテム開く");
                backpackScript.OpenBackpack();
            }
        }
        else if(isMenu && selectMenuNow){

        }
    }
}
