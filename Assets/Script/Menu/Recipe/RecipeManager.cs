using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    public class ItemData
    {
        public int ID;
        public string name;
        public bool canMake;
        public ItemData(int i, string n, bool c)
        {
            ID = i;
            name = n;
            canMake = c;
        }
    }
    public List<ItemData> dataList = new List<ItemData>();
    List<int> RecipeList = new List<int>();//所持アイテムだけのリスト（IDだけ）

    string sortState = "ID";//ID順ならID,名前順はname,作成可能順はcanMake

    private GameObject frame;
    private GameObject icon;
    private Image image;
    private TextMeshProUGUI text;//テキスト

    public bool opening = false;
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;

    public GameObject background;
    private GameObject nowCursor;
    private GameObject nowImage;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    public Color canMakeColor;
    public Color notCanMakeColor;
    // int[,] menuList = new int[3,2];
    int[] menuList = new int[6];
    private int nowListNumber = 0;
    private int beforeListNum = -1;
    private bool isLongPushUp, isLongPushDown, isLongPushRight, isLongPushLeft;
    private float pushDuration = 0.3f;
    private float downTime = 0f;
    //public Recipe recipeScript;
    public ItemInfoRecipe itemInfoRecipe;
    int menuPage = 0;

    //ウィッシュリスト
    public GameObject WishListObject;
    public WishListManager wishListManager;
    public WishListIcon wishListIcon;


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
            WishListObject.SetActive(false);
            opening = false;
        }
        if (playerInputAction.UI.Sort.triggered)
        {
            if (sortState == "ID")
            {
                sortState = "name";
                ListSort();
            }
            else if (sortState == "name")
            {
                sortState = "canMake";
                ListSort();
            }
            else if (sortState == "canMake")
            {
                sortState = "ID";
                ListSort();
            }
        }

        if (playerInputAction.UI.MenuPageRight.triggered)
        {
            TurnMenuPage(1);
        }
        if (playerInputAction.UI.MenuPageLeft.triggered)
        {
            TurnMenuPage(-1);
        }
        MoveCursor();
    }

    //レシピのメニューを開いたときに起動する
    public void OpenRecipe()
    {
        if (!opening)
        {
            opening = true;
            // if(autoSetList){
            AutoSetDataList();
            // }

            CanMakeSet();

            RecipeList.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                // if(dataList[i].canMake){
                RecipeList.Add(dataList[i].ID);
                // }
            }

            RecipeSetting(RecipeList);
            // recipeItemName.RecipeNameSetting(RecipeList);
            SetmenuSelect(RecipeList);
            gameObject.SetActive(true);
            WishListObject.SetActive(true);
        }
    }


    // メニュー項目のレシピをリストから取り出す
    public void RecipeSetting(List<int> RecipeList)
    {
        int RecipePageItem = RecipeList.Count - 6 * menuPage;
        if (RecipePageItem > 6) RecipePageItem = 6;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for (int i = 0; i < RecipePageItem; i++)
        {
            frame = background.transform.GetChild(i).gameObject;
            icon = frame.transform.GetChild(0).gameObject;
            image = icon.GetComponent<Image>();
            image.sprite = sweetsDB.sweetsList[RecipeList[i+menuPage*6]].image;
            icon = frame.transform.GetChild(1).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            text.text = sweetsDB.sweetsList[RecipeList[i+menuPage*6]].name;

            var c = text.color;
            if (sweetsDB.sweetsList[RecipeList[i]].canMake)
            {
                text.color = canMakeColor;
                image.color = canMakeColor;
            }
            else
            {
                text.color = notCanMakeColor;
                image.color = notCanMakeColor;
            }
        }
        //空白は透明度を0にする
        for (int i = RecipePageItem; i < 6; i++)
        {
            frame = background.transform.GetChild(i).gameObject;
            icon = frame.transform.GetChild(0).gameObject;
            image = icon.transform.GetComponent<Image>();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0f);
            icon = frame.transform.GetChild(1).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            c = text.color;
            text.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    // レシピのリストをソートする
    public void ListSort()
    {
        if (sortState == "ID")
        {
            dataList.Sort((a, b) => a.ID - b.ID);
            RecipeList.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                // if(dataList[i].canMake){
                RecipeList.Add(dataList[i].ID);
                // }
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            RecipeSetting(RecipeList);
            // recipeItemName.RecipeNameSetting(RecipeList);
            SetmenuSelect(RecipeList);

        }
        else if (sortState == "name")
        {
            dataList.Sort((a, b) => string.Compare(a.name, b.name));
            RecipeList.Clear();
            for (int i = 0; i < dataList.Count; i++)
            {
                // if(dataList[i].canMake){
                RecipeList.Add(dataList[i].ID);
                // }
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            RecipeSetting(RecipeList);
            // recipeItemName.RecipeNameSetting(RecipeList);
            SetmenuSelect(RecipeList);
        }
        else if (sortState == "canMake")
        {
            // 作成可能なスイーツと作成不可能なスイーツを分けてマージする。
            List<int> canMakeData = new List<int>();
            List<int> notCanMakeData = new List<int>();
            if (dataList.Count != 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].canMake) canMakeData.Add(dataList[i].ID);
                    else notCanMakeData.Add(dataList[i].ID);
                }
            }

            // それぞれのリストをID順に並び替える
            canMakeData.Sort((a, b) => a - b);
            notCanMakeData.Sort((a, b) => a - b);

            // 合体
            RecipeList.Clear();
            for (int i = 0; i < canMakeData.Count; i++)
            {
                RecipeList.Add(canMakeData[i]);
            }
            for (int i = 0; i < notCanMakeData.Count; i++)
            {
                RecipeList.Add(notCanMakeData[i]);
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            RecipeSetting(RecipeList);
            // recipeItemName.RecipeNameSetting(RecipeList);
            SetmenuSelect(RecipeList);
        }
    }

    // ページ遷移が行われたときに項目を切り替える
    public void TurnMenuPage(int TurnPage)
    {//右にいくなら1,左なら-1
        menuPage += TurnPage;
        if (RecipeList.Count < 6 * menuPage)
        {
            menuPage = 0;
        }
        else if (menuPage < 0)
        {
            menuPage = RecipeList.Count / 6;
        }
        RecipeSetting(RecipeList);
        // recipeItemName.RecipeNameSetting(RecipeList);
        SetmenuSelect(RecipeList);
    }

    //レシピ入手時にレシピ欄に追加する
    public void AddDataList(int i, string n, bool c)
    {
        dataList.Add(new ItemData(i, n, c));
    }

    // そのスイーツが作成可能かを設定する
    public void CanMakeSet()
    {
        if (dataList.Count != 0)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                bool allMaterialsOk = true;//これがtrueのままなら全部の素材がそろっている
                for (int j = 0; j < sweetsDB.sweetsList[dataList[i].ID].materialsList.Count; j++)
                {
                    if (ingredientsDB.ingredientsList[dataList[i].ID].quantity < sweetsDB.sweetsList[dataList[i].ID].materialsList[j].個数)
                    {
                        allMaterialsOk = false;
                    }
                }

                if (allMaterialsOk)
                {
                    dataList[i].canMake = true;
                    sweetsDB.sweetsList[dataList[i].ID].canMake = true;
                }
                else
                {
                    dataList[i].canMake = false;
                    sweetsDB.sweetsList[dataList[i].ID].canMake = false;
                }

                //デバッグ
                if (dataList[i].canMake)
                {
                    Debug.Log(sweetsDB.sweetsList[dataList[i].ID].name + "は作成可能");
                }
                else
                {
                    Debug.Log(sweetsDB.sweetsList[dataList[i].ID].name + "は作成不可能");
                }

            }
        }
    }

    // 自動的にレシピを追加する(デバッグ用)
    public void AutoSetDataList()
    {
        dataList.Clear();
        int j = 0;//インベントリのリスト用
        for (int i = 0; i < sweetsDB.sweetsList.Count; i++)
        {
            // if(sweetsDB.sweetsList[i].canMake){
            dataList.Add(new ItemData(sweetsDB.sweetsList[i].ID, sweetsDB.sweetsList[i].name, sweetsDB.sweetsList[i].canMake));
            j++;
            // }
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            // if(dataList[i].canMake){
            RecipeList.Add(dataList[i].ID);
            j++;
            // }
        }
    }

    // メニューのカーソルを移動する
    void MoveCursor()
    {
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
            if (menuList[nowListNumber] != -1)
            {
                if (sweetsDB.sweetsList[menuList[nowListNumber]].canMake)
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
            if (nowListNumber < 0) nowListNumber += menuList.Length;
            itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);
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
                if (menuList[nowListNumber] != -1)
                {
                    if (sweetsDB.sweetsList[menuList[nowListNumber]].canMake)
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
                if (nowListNumber < 0) nowListNumber += menuList.Length;
                itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);

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
            if (menuList[nowListNumber] != -1)
            {
                if (sweetsDB.sweetsList[menuList[nowListNumber]].canMake)
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
            if (nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
            itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);
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
                if (menuList[nowListNumber] != -1)
                {
                    if (sweetsDB.sweetsList[menuList[nowListNumber]].canMake)
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
                if (nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                itemInfoRecipe.SetItemInfo(menuList[nowListNumber]);

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
        if (playerInputAction.UI.MenuSelect.triggered && (menuList[nowListNumber] != -1))
        {
            int selectID = menuList[nowListNumber];
            if (!sweetsDB.sweetsList[selectID].wishList)
            {
                wishListManager.AddWishList(selectID);
                wishListIcon.SetWishListIcon();
            }
        }
    }

    // レシピのIDの配列を作成する
    public void SetmenuSelect(List<int> RecipeList){
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
            menuBackground = background.transform.GetChild(i).gameObject;
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

