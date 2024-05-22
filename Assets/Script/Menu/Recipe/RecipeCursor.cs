using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCursor : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private GameObject nowCursor;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    int[,] menuList = new int[3,2];
    private int nowListNumber = 0;
    private bool isLongPushUp;
    private bool isLongPushDown;
    private bool isLongPushRight;
    private bool isLongPushLeft;
    private  float pushDuration = 0.3f;
    private float downTime = 0f;
    public Recipe recipeScript;
    public ItemInfoRecipe itemInfoRecipe;

    public int rowNum = 10; //持ち物の横のアイコンの数
    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(recipeScript.opening){
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
                itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                    itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                    itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                    itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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
                    itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
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

    public void SetmenuSelect(List<int> RecipeList){
        int menuPage = recipeScript.menuPage;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 2; j++){
                // menuList[i,j] = j+8*i;
                if(RecipeList.Count > (j+2*i + menuPage*6)){
                    menuList[i,j] = RecipeList[j+2*i + menuPage*6];
                    Debug.Log("menuList["+i+","+j+"]に"+RecipeList[j+2*i + menuPage*6]+"をいれた");
                }
                else{
                    menuList[i,j] = -1;
                    Debug.Log("menuList["+i+","+j+"]に-1をいれた");
                }
            }
        }
        itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
    }
}
