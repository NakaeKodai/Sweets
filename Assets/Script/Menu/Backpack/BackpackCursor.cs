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
    int[,] menuList = new int[5,8];
    public int nowListNumber = 0;
    public Backpack backpackScript;
    public ItemInfoBackpack itemInfoBackpack;

    public int rowNum = 10; //持ち物の横のアイコンの数

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        // menuList = new int[5,8];
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
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            
            if(playerInputAction.UI.CursorMoveLeft.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }

            if(playerInputAction.UI.CursorMoveUp.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber -= rowNum;
                if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            
            if(playerInputAction.UI.CursorMoveDown.triggered){
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber += rowNum;
                if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
        }
    }

    public void SetmenuSelect(List<int> BackpackList){
        int menuPage = backpackScript.menuPage;
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < 8; j++){
                // menuList[i,j] = j+8*i;
                if(BackpackList.Count > (j+8*i + menuPage*40)){
                    menuList[i,j] = BackpackList[j+8*i + menuPage*40];
                    Debug.Log("menuList["+i+","+j+"]に"+BackpackList[j+8*i + menuPage*40]+"をいれた");
                }
                else{
                    menuList[i,j] = -1;
                    Debug.Log("menuList["+i+","+j+"]に-1をいれた");
                }
            }
        }
        itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
    }
}
