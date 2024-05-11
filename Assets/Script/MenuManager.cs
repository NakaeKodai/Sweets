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
    public Color nomalColor;
    public Color selectColor;
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInputAction.UI.OpenMenu.triggered){
            if(gameManager.pause){
                menuWindow.SetActive(false);
                Time.timeScale = 1;
                gameManager.pause = false;
                isMenu = false;
                selectBottuon = menuList[selectBottuonNum];
                Image buttonImage = selectBottuon.GetComponent<Image>();
                buttonImage.color = nomalColor;
                Debug.Log("そして時は動き出す");
            } 
            else{
                menuWindow.SetActive(true);
                Time.timeScale = 0;
                gameManager.pause = true;
                isMenu = true;
                selectBottuonNum = 0;
                Debug.Log("ザ・ワールド！");
            } 
        }

        if(isMenu)
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
        }
    }
}
