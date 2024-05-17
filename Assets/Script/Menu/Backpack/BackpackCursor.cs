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
    private int nowListNumber = 0;
    private bool isLongPushUp;
    private bool isLongPushDown;
    private bool isLongPushRight;
    private bool isLongPushLeft;
    private  float pushDuration = 0.3f;
    private float downTime = 0f;
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


            //右移動
            if(playerInputAction.UI.CursorMoveRight.triggered){
                downTime = Time.realtimeSinceStartup;
                isLongPushRight = true;
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber ++;
                if(nowListNumber >= menuList.Length) nowListNumber = 0;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            if(isLongPushRight)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    nowCursorImage.color = nomalColor;
                    nowListNumber ++;
                    if(nowListNumber >= menuList.Length) nowListNumber = 0;
                    itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveRight.canceled += ctx => {
                isLongPushRight = false;
                pushDuration = 0.3f;
            };


            //左移動
            if(playerInputAction.UI.CursorMoveLeft.triggered){
                downTime = Time.realtimeSinceStartup;
                isLongPushLeft = true;
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            if(isLongPushLeft)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    nowCursorImage.color = nomalColor;
                    nowListNumber --;
                    if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
                    itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveLeft.canceled += ctx => {
                isLongPushLeft = false;
                pushDuration = 0.3f;
            };


            //上移動
            if(playerInputAction.UI.CursorMoveUp.triggered){
                downTime = Time.realtimeSinceStartup;
                isLongPushUp = true;
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber -= rowNum;
                if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            if(isLongPushUp)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    nowCursorImage.color = nomalColor;
                    nowListNumber -= rowNum;
                    if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                    itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveUp.canceled += ctx => {
                isLongPushUp = false;
                pushDuration = 0.3f;
            };
            

            //下移動
            if(playerInputAction.UI.CursorMoveDown.triggered){
                downTime = Time.realtimeSinceStartup;
                isLongPushDown = true;
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber += rowNum;
                if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
            }
            if(isLongPushDown)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    nowCursorImage.color = nomalColor;
                    nowListNumber += rowNum;
                    if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                    itemInfoBackpack.SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveDown.canceled += ctx => {
                isLongPushDown = false;
                pushDuration = 0.3f;
            };
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
