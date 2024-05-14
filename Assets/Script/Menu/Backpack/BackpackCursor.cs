using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackCursor : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private GameObject nowCursor;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    int[,] menuList;
    public int nowListNumber = 0;
    public Backpack backpackScript;

    public int rowNum = 10; //持ち物の横のアイコンの数

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        menuList = new int[4,10];
    }

    // Update is called once per frame
    void Update()
    {
        if(backpackScript.opening){
            nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = selectColor;

            if(playerInputAction.UI.CursorMoveRight.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber ++;
                if(nowListNumber >= menuList.Length) nowListNumber = 0;
            }
            
            if(playerInputAction.UI.CursorMoveLeft.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
            }

            if(playerInputAction.UI.CursorMoveUp.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber -= rowNum;
                if(nowListNumber < 0) nowListNumber +=  menuList.Length;
            }
            
            if(playerInputAction.UI.CursorMoveDown.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber += rowNum;
                if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
            }
        }
    }

    public void SetmenuSelect(){
        for(int i = 0; i < menuList.Length; i++){
            for(int j = 0; j < 10; i++){
                menuList[i,j] = j+10*i;
            }
        }
    }
}
