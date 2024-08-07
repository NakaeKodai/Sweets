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
    public Color notCanMakeColor;
    // int[,] menuList = new int[3,2];
    int[] menuList = new int[6];
    private int nowListNumber = 0;
    private bool isLongPushUp;
    private bool isLongPushDown;
    private bool isLongPushRight;
    private bool isLongPushLeft;
    private  float pushDuration = 0.3f;
    private float downTime = 0f;
    public Recipe recipeScript;
    public ItemInfoRecipe itemInfoRecipe;
    public WishListIcon wishListIcon;
    public SweetsDB sweetsDB;
    int menuPage;

    public int rowNum = 10; //持ち物の横のアイコンの数

    public WishListManager wishListManager;
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
                // downTime = Time.realtimeSinceStartup;
                // isLongPushRight = true;
                // nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                // nowCursorImage = nowCursor.GetComponent<Image>();
                // if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                //     if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                //         nowCursorImage.color = nomalColor;
                //     }else{
                //         nowCursorImage.color = notCanMakeColor;
                //     }
                // }else{
                //     nowCursorImage.color = notCanMakeColor;
                // }
                // nowListNumber ++;
                // if(nowListNumber >= menuList.Length) nowListNumber = 0;
                // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
            }
            if(isLongPushRight)
            {
                // if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                // {
                //     nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                //     nowCursorImage = nowCursor.GetComponent<Image>();
                //     if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                //         if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                //             nowCursorImage.color = nomalColor;
                //         }else{
                //             nowCursorImage.color = notCanMakeColor;
                //         }
                //     }else{
                //         nowCursorImage.color = notCanMakeColor;
                //     }
                //     nowListNumber ++;
                //     if(nowListNumber >= menuList.Length) nowListNumber = 0;
                //     itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
                //     pushDuration = 0.1f;
                //     downTime = Time.realtimeSinceStartup;
                // }
            // }
            // playerInputAction.UI.CursorMoveRight.canceled += ctx => {
            //     isLongPushRight = false;
            //     pushDuration = 0.3f;
            };


            //左移動
            // if(playerInputAction.UI.CursorMoveLeft.triggered){
            //     downTime = Time.realtimeSinceStartup;
            //     isLongPushLeft = true;
            //     nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
            //     nowCursorImage = nowCursor.GetComponent<Image>();
            //     if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
            //         if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
            //             nowCursorImage.color = nomalColor;
            //         }else{
            //             nowCursorImage.color = notCanMakeColor;
            //         }
                    
            //     }else{
            //         nowCursorImage.color = notCanMakeColor;
            //     }
            //     nowListNumber --;
            //     if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
            //     itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
            // }
            // if(isLongPushLeft)
            // {
            //     if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            //     {
            //         nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
            //         nowCursorImage = nowCursor.GetComponent<Image>();
            //         if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
            //             if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
            //                 nowCursorImage.color = nomalColor;
            //             }else{
            //                 nowCursorImage.color = notCanMakeColor;
            //             }
                    
            //         }else{
            //             nowCursorImage.color = notCanMakeColor;
            //         }
            //         nowListNumber --;
            //         if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
            //         itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);
            //         pushDuration = 0.1f;
            //         downTime = Time.realtimeSinceStartup;
            //     }
            // }
            // playerInputAction.UI.CursorMoveLeft.canceled += ctx => {
            //     isLongPushLeft = false;
            //     pushDuration = 0.3f;
            // };


            //上移動
            if(playerInputAction.UI.CursorMoveUp.triggered){
                downTime = Time.realtimeSinceStartup;
                isLongPushUp = true;
                nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                // 二次元配列用
                // if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                //     if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                //         nowCursorImage.color = nomalColor;
                //     }else{
                //         nowCursorImage.color = notCanMakeColor;
                //     }
                // }else{
                //     nowCursorImage.color = notCanMakeColor;
                // }
                // nowListNumber -= rowNum;
                // if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);

                // 一次元配列用
                if(menuList[nowListNumber] != -1){
                    if(sweetsDB.sweetsList[menuList[nowListNumber]].canMake){
                        nowCursorImage.color = nomalColor;
                    }else{
                        nowCursorImage.color = notCanMakeColor;
                    }
                }else{
                    nowCursorImage.color = notCanMakeColor;
                }
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);
            }
            if(isLongPushUp)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    // 二次元配列用
                    // if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                    //     if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                    //         nowCursorImage.color = nomalColor;
                    //     }else{
                    //         nowCursorImage.color = notCanMakeColor;
                    //     }
                    // }else{
                    //     nowCursorImage.color = notCanMakeColor;
                    // }
                    // nowListNumber -= rowNum;
                    // if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                    // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);

                    // 一次元配列用
                    if(menuList[nowListNumber] != -1){
                        if(sweetsDB.sweetsList[menuList[nowListNumber]].canMake){
                            nowCursorImage.color = nomalColor;
                        }else{
                            nowCursorImage.color = notCanMakeColor;
                        }
                    }else{
                        nowCursorImage.color = notCanMakeColor;
                    }
                    nowListNumber --;
                    if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                    itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);

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
                // 二次元配列
                // if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                //     if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                //         nowCursorImage.color = nomalColor;
                //     }else{
                //         nowCursorImage.color = notCanMakeColor;
                //     }
                // }else{
                //     nowCursorImage.color = notCanMakeColor;
                // }
                // nowListNumber += rowNum;
                // if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);

                // 一次元配列
                if(menuList[nowListNumber] != -1){
                    if(sweetsDB.sweetsList[menuList[nowListNumber]].canMake){
                        nowCursorImage.color = nomalColor;
                    }else{
                        nowCursorImage.color = notCanMakeColor;
                    }
                }else{
                    nowCursorImage.color = notCanMakeColor;
                }
                nowListNumber ++;
                if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);
            }
            if(isLongPushDown)
            {
                if(Time.realtimeSinceStartup - downTime >=  pushDuration)
                {
                    nowCursor = gameObject.transform.GetChild(nowListNumber).gameObject;
                    nowCursorImage = nowCursor.GetComponent<Image>();
                    // 二次元配列
                    // if(menuList[(nowListNumber/2), (nowListNumber%2)] != -1){
                    //     if(sweetsDB.sweetsList[menuList[(nowListNumber/2), (nowListNumber%2)]].canMake){
                    //         nowCursorImage.color = nomalColor;
                    //     }else{
                    //         nowCursorImage.color = notCanMakeColor;
                    //     }
                    // }else{
                    //     nowCursorImage.color = notCanMakeColor;
                    // }
                    // nowListNumber += rowNum;
                    // if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                    // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);

                    // 一次元配列
                    if(menuList[nowListNumber] != -1){
                        if(sweetsDB.sweetsList[menuList[nowListNumber]].canMake){
                            nowCursorImage.color = nomalColor;
                        }else{
                            nowCursorImage.color = notCanMakeColor;
                        }
                    }else{
                        nowCursorImage.color = notCanMakeColor;
                    }
                    nowListNumber ++;
                    if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                    itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);

                    pushDuration = 0.1f;
                    downTime = Time.realtimeSinceStartup;
                }
            }
            playerInputAction.UI.CursorMoveDown.canceled += ctx => {
                isLongPushDown = false;
                pushDuration = 0.3f;
            };

            // 二次元配列
            // if(playerInputAction.UI.MenuSelect.triggered && (menuList[(nowListNumber/2), (nowListNumber%2)] != -1)){
            //     int selectID = menuList[(nowListNumber/2), (nowListNumber%2)];
            //     if(!sweetsDB.sweetsList[selectID].wishList){
            //         wishListManager.AddWishList(selectID);
            //         wishListIcon.SetWishListIcon();
            //     }
            // }

            // 一次元配列
            if(playerInputAction.UI.MenuSelect.triggered && (menuList[nowListNumber] != -1)){
                int selectID = menuList[nowListNumber];
                if(!sweetsDB.sweetsList[selectID].wishList){
                    wishListManager.AddWishList(selectID);
                    wishListIcon.SetWishListIcon();
                }
            }
        }
    }

    public void SetmenuSelect(List<int> RecipeList){
        menuPage = recipeScript.menuPage;
        // 二次元配列用のプログラム
        // for(int i = 0; i < 3; i++){
        //     for(int j = 0; j < 2; j++){
        //         GameObject menuBackground;// 背景色の設定用
        //         Image menuBackgroundImage;
        //         menuBackground = gameObject.transform.GetChild(j+2*i).gameObject;
        //         menuBackgroundImage = menuBackground.GetComponent<Image>();
                // if(RecipeList.Count > (j+2*i + menuPage*6)){
                //     menuList[i,j] = RecipeList[j+2*i + menuPage*6];
                //     if(sweetsDB.sweetsList[menuList[i, j]].canMake){
                //         menuBackgroundImage.color = nomalColor;
                //     }else{
                //         menuBackgroundImage.color = notCanMakeColor;
                //     }
                    
                // }
                // else{
                //     menuList[i,j] = -1;
                //     menuBackgroundImage.color = notCanMakeColor;
                // }
        //     }
        // }
        // itemInfoRecipe.SetItemInfo(menuList[(nowListNumber/2), (nowListNumber%2)]);

        // 一次元配列用
        for(int i = 0; i < 6; i++){
            GameObject menuBackground;// 背景色の設定用
            Image menuBackgroundImage;
            menuBackground = gameObject.transform.GetChild(i).gameObject;
            menuBackgroundImage = menuBackground.GetComponent<Image>();
            if(RecipeList.Count > (i + menuPage*6)){
                    menuList[i] = RecipeList[i + menuPage*6];
                    if(sweetsDB.sweetsList[menuList[i]].canMake){
                        menuBackgroundImage.color = nomalColor;
                    }else{
                        menuBackgroundImage.color = notCanMakeColor;
                    }
                    
                }
                else{
                    menuList[i] = -1;
                    menuBackgroundImage.color = notCanMakeColor;
                }
        }
        itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);
    }
}
