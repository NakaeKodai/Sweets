using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WishListManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    public List<int> wishList = new List<int>();
    public SweetsDB sweetsDB;
    [Header("ウィッシュリストの最大数")]
    public int wishListMax;
    private bool wishListMenuOpening;//ウィッシュリスト専用のメニュー開いたことを判別する

    public GameManager gameManager;

    public GameObject background;
    private GameObject frame;
    private Image image;//画像
    private GameObject icon;
    private TextMeshProUGUI text;

    public Sprite nomalIcon;

    public Color nomalColor;
    public Color selectColor;
    public Color canMakeColor;
    public Color notCanMakeColor;

    public bool opening;

    private int nowListNumber = 0;
    private int beforeListNum = -1;
    private bool isLongPushUp, isLongPushDown, isLongPushRight, isLongPushLeft;
    private float pushDuration = 0.3f;
    private float downTime = 0f;
    //public Recipe recipeScript;
    public ItemInfoRecipe itemInfoRecipe;
    public GameObject itemInfoObject;
    private GameObject nowCursor;
    private GameObject nowImage;
    private Image nowCursorImage;//画像

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered)
        {
            gameObject.SetActive(false);
            opening = false;
            wishListMenuOpening = false;
        }
        if(wishListMenuOpening){
            MoveCursor();
        }
    }

    public void OpenWishList(){
        opening = true;
        SetWishList();
        gameObject.SetActive(true);
    }

    // 指定したIDをウィッシュリストに追加
    public void AddWishList(int ID){
        if(wishList.Count < wishListMax){
            gameManager.wishList.Add(ID);
            sweetsDB.sweetsList[ID].wishList = true;
            Debug.Log(sweetsDB.sweetsList[ID].name+"をウィッシュリストに追加した。");
        }
    }

    // 指定したIDと一致する要素を削除
    public void RemoveWishList(int ID){
        gameManager.wishList.Remove(ID);
        sweetsDB.sweetsList[ID].wishList = false;
    }

    // 指定した数字の要素を取得する。
    public int GiveWishList(int number){
        return wishList[number];
    }

    // レシピのとこ用のアイコンだけ表示
    public void SetWishListIcon(){
        // 見やすくするために別スクリプトの物を置いておく
        // int wishListMax = wishListMax;
        wishList = gameManager.wishList;
        for(int i = 0; i < wishListMax; i++){
            if(i < wishList.Count){
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = sweetsDB.sweetsList[wishList[i]].image;
                if(sweetsDB.sweetsList[wishList[i]].canMake){
                    // アイコンから色を指定
                    image.color = canMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = nomalColor;
                }
                else{
                    image.color = notCanMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = notCanMakeColor;
                }
            }
            else{
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                image = frame.GetComponent<Image>();
                image.color = notCanMakeColor;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
            }
        }
    }

    // ウィッシュリスト専用のメニューでのアイコン設定（名前付き）
    public void SetWishList(){
        wishList = gameManager.wishList;
        wishListMenuOpening = true;
        for(int i = 0; i < wishListMax; i++){
            if(i < wishList.Count){
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = sweetsDB.sweetsList[wishList[i]].image;
                if(sweetsDB.sweetsList[wishList[i]].canMake){
                    // アイコンから色を指定
                    image.color = canMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = nomalColor;
                }
                else{
                    image.color = notCanMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = notCanMakeColor;
                }
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = sweetsDB.sweetsList[wishList[i]].name;
                
            }
            else{
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                image = frame.GetComponent<Image>();
                image.color = notCanMakeColor;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = "";
            }
        }
        if(wishList.Count != 0){
            itemInfoRecipe.SetItemInfo(wishList[nowListNumber]);
        }
        else{
            itemInfoObject.SetActive(false);
        }
        
    }

    // メニューのカーソルを移動する
    void MoveCursor()
    {
        if(wishList.Count != 0){
        if (beforeListNum != nowListNumber)
        {
            nowCursor = background.transform.GetChild(nowListNumber).gameObject;
            //nowCursor = nowImage.transform.GetChild(0).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = selectColor;
        }
        if (playerInputAction.UI.CursorMoveUp.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushUp = true;
            nowCursor = background.transform.GetChild(nowListNumber).gameObject;
            //nowCursor = nowImage.transform.GetChild(0).gameObject;
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
            if (wishList[nowListNumber] != -1)
            {
                if (sweetsDB.sweetsList[wishList[nowListNumber]].canMake)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }
            else
            {
                nowCursorImage.color = notCanMakeColor;
            }
            nowListNumber--;
            if (nowListNumber < 0) nowListNumber += wishList.Count;
            itemInfoRecipe.SetItemInfo(wishList[nowListNumber]);
        }
        if (isLongPushUp)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = background.transform.GetChild(nowListNumber).gameObject;
                //nowCursor = nowImage.transform.GetChild(0).gameObject;
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
                if (wishList[nowListNumber] != -1)
                {
                    if (sweetsDB.sweetsList[wishList[nowListNumber]].canMake)
                    {
                        nowCursorImage.color = nomalColor;
                    }
                    else
                    {
                        nowCursorImage.color = notCanMakeColor;
                    }
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
                nowListNumber--;
                if (nowListNumber < 0) nowListNumber += wishList.Count;
                itemInfoRecipe.SetItemInfo(wishList[nowListNumber]);

                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveUp.canceled += ctx =>
        {
            isLongPushUp = false;
            pushDuration = 0.3f;
        };


        //下移動
        if (playerInputAction.UI.CursorMoveDown.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushDown = true;
            nowCursor = background.transform.GetChild(nowListNumber).gameObject;
            //nowCursor = nowImage.transform.GetChild(0).gameObject;
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
            if (wishList[nowListNumber] != -1)
            {
                if (sweetsDB.sweetsList[wishList[nowListNumber]].canMake)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }
            else
            {
                nowCursorImage.color = notCanMakeColor;
            }
            nowListNumber++;
            if (nowListNumber >= wishList.Count) nowListNumber -= wishList.Count;
            itemInfoRecipe.SetItemInfo(wishList[nowListNumber]);
        }
        if (isLongPushDown)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = background.transform.GetChild(nowListNumber).gameObject;
                //nowCursor = nowImage.transform.GetChild(0).gameObject;
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
                if (wishList[nowListNumber] != -1)
                {
                    if (sweetsDB.sweetsList[wishList[nowListNumber]].canMake)
                    {
                        nowCursorImage.color = nomalColor;
                    }
                    else
                    {
                        nowCursorImage.color = notCanMakeColor;
                    }
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
                nowListNumber++;
                if (nowListNumber >= wishList.Count) nowListNumber -= wishList.Count;
                itemInfoRecipe.SetItemInfo(wishList[nowListNumber]);

                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveDown.canceled += ctx =>
        {
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
        // if (playerInputAction.UI.MenuSelect.triggered && (menuList[nowListNumber] != -1))
        // {
        //     int selectID = menuList[nowListNumber];
        //     if (!sweetsDB.sweetsList[selectID].wishList)
        //     {
        //         wishListManager.AddWishList(selectID);
        //         wishListManager.SetWishListIcon();
        //     }
        // }
        }
    }
}
