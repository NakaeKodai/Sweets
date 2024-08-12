using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WishListManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    public List<int> wishList = new List<int>();
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    [Header("ウィッシュリストの最大数")]
    public int wishListMax;
    private bool wishListMenuOpening;//ウィッシュリスト専用のメニュー開いたことを判別する
    private int removedID;//削除に使う
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
    // public ItemInfoRecipe itemInfoRecipe;
    // public GameObject itemInfoObject;
    private GameObject nowCursor;
    private GameObject nowImage;
    private Image nowCursorImage;//画像

    // 説明文
    [Header("説明文関連")]
    public GameObject InfoObject;
    public TextMeshProUGUI ItemName;//名前
    public Image ItemIcon;//アイコン
    public TextMeshProUGUI ItemInfomation;//説明文
    public GameObject infoMaterials;

    //ウィッシュリストの素材の最大数
    [Header("最大数関連")]
    public GameObject SumObject;
    // private int[] displayList = new int[6];//画面に表示させる配列
    // private int SumCursor=0;
    private int stackPointer=0;//素材のリストをぶち込んだ時の現在位置の場所
    public int SumCursorNumber=6;//アイコンの個数
    public GameObject sumBackground;
    private TextMeshProUGUI countText;
    private Color color;
    public Color countTextNomalColor;
    public Color countTextLackColor;

    // 素材の説明欄
    [Header("素材の説明欄関連")]
    public GameObject materialsInfo;//素材説明文
    public TextMeshProUGUI infoName;
    public Image infoIcon;
    public TextMeshProUGUI haveQuantity;
    public GameObject CollectionsPoints;//素材が取れる場所（現在未使用、のちのち追加したい）
    public GameObject UseMaterials;//素材が何のレシピにつかうか
    

    public class MaterialSumList{
        public int ID;//素材のID
        public int quantity;//素材の必要個数(合計)
        public MaterialSumList(int i, int q){
            ID = i;
            quantity = q;
        }
    }
    private List<MaterialSumList>  materialSumList = new List<MaterialSumList>();//必要素材をぶち込むリスト

    //0:ウィッシュリストに登録されたレシピを見る
    //1:ウィッシュリストに登録されているレシピの素材を見る
    private int wishListMode = 0;

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
            SumObject.SetActive(false);
            opening = false;
            wishListMenuOpening = false;
            GameManager.wishList = wishList;
        }
        if(wishListMode == 0 && wishListMenuOpening){
            if(wishListMenuOpening){
                MoveCursor();
            }
            if(playerInputAction.UI.MenuSelect.triggered && wishList.Count >= 1){
                // 削除処理
                sweetsDB.sweetsList[wishList[nowListNumber]].wishList = false;
                wishList.RemoveAt(nowListNumber);
                nowListNumber = 0;
                SetWishList();
                
            }
            if((playerInputAction.UI.MenuPageRight.triggered || playerInputAction.UI.MenuPageLeft.triggered) && (wishList.Count >= 1)){
                SetWishListSum();
                SetWishListSumIcon();
                // gameObject.SetActive(false);
                InfoObject.SetActive(false);
                SumObject.SetActive(true);
                wishListMode = 1;
                nowListNumber = 0;
                SetMaterialsInfo();
            }
        }
        else if(wishListMode == 1 && wishListMenuOpening){
            if(wishListMenuOpening){
                SumMoveCursor();
            }
            if(playerInputAction.UI.MenuPageRight.triggered || playerInputAction.UI.MenuPageLeft.triggered){
                // gameObject.SetActive(true);
                InfoObject.SetActive(true);
                SumObject.SetActive(false);
                wishListMode = 0;
                nowListNumber = 0;
            }
        }
        
    }

    public void OpenWishList(){
        opening = true;
        SetWishList();
        gameObject.SetActive(true);
        // InfoObject.SetActive(true);
        nowListNumber = 0;
    }

    // 指定したIDをウィッシュリストに追加
    public void AddWishList(int ID){
        if(wishList.Count < wishListMax){
            GameManager.wishList.Add(ID);
            sweetsDB.sweetsList[ID].wishList = true;
            Debug.Log(sweetsDB.sweetsList[ID].name+"をウィッシュリストに追加した。");
        }
    }

    // 指定したIDと一致する要素を削除
    public void RemoveWishList(int ID){
        sweetsDB.sweetsList[GameManager.wishList[ID]].wishList = false;
        GameManager.wishList.RemoveAt(ID);
        
    }

    // 指定した数字の要素を取得する。
    public int GiveWishList(int number){
        return wishList[number];
    }

    // レシピのとこ用のアイコンだけ表示
    public void SetWishListIcon(){
        // 見やすくするために別スクリプトの物を置いておく
        // int wishListMax = wishListMax;
        wishList = GameManager.wishList;
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
                image.color = canMakeColor;
            }
        }
    }

    // ウィッシュリスト専用のメニューでのアイコン設定（名前付き）
    public void SetWishList(){
        wishList = GameManager.wishList;
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
                image.color = canMakeColor;
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = "";
            }
        }
        if(wishList.Count != 0){
            SetItemInfo(wishList[nowListNumber]);
            // InfoObject.SetActive(true);
        }
        else{
            InfoObject.SetActive(false);
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
            SetItemInfo(wishList[nowListNumber]);
        }
        if (isLongPushUp)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = background.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();

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
                SetItemInfo(wishList[nowListNumber]);

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
            nowCursorImage = nowCursor.GetComponent<Image>();

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
            SetItemInfo(wishList[nowListNumber]);
        }
        if (isLongPushDown)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = background.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();

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
                SetItemInfo(wishList[nowListNumber]);

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

    // 最大数のカーソル
    void SumMoveCursor(){
        if(wishList.Count != 0){
        if (beforeListNum != nowListNumber)
        {
            nowCursor = sumBackground.transform.GetChild(nowListNumber).gameObject;
            //nowCursor = nowImage.transform.GetChild(0).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = selectColor;
        }
        if (playerInputAction.UI.CursorMoveUp.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushUp = true;
            nowCursor = sumBackground.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();

            // 一次元配列用
                if(nowListNumber+stackPointer < materialSumList.Count){
                if (materialSumList[nowListNumber+stackPointer].quantity < ingredientsDB.ingredientsList[materialSumList[nowListNumber+stackPointer].ID].quantity)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }else{
                nowCursorImage.color = notCanMakeColor;
            }
            
            nowListNumber--;

            if(nowListNumber < 0){
                if (stackPointer == 0){
                    nowListNumber += SumCursorNumber;
                    stackPointer = materialSumList.Count-SumCursorNumber;
                }else{
                    stackPointer--;
                    nowListNumber++;
                }
                SetWishListSumIcon();
            }
            // SetItemInfo(wishList[nowListNumber]);
            SetMaterialsInfo();
        }
        if (isLongPushUp)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = sumBackground.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();

                // 一次元配列用
                    if(nowListNumber+stackPointer < materialSumList.Count){
                if (materialSumList[nowListNumber+stackPointer].quantity < ingredientsDB.ingredientsList[materialSumList[nowListNumber+stackPointer].ID].quantity)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }else{
                nowCursorImage.color = notCanMakeColor;
            }
                
                nowListNumber--;

                if(nowListNumber < 0){
                if (stackPointer == 0){
                    nowListNumber += SumCursorNumber;
                    stackPointer = materialSumList.Count-SumCursorNumber;
                }else{
                    stackPointer--;
                    nowListNumber++;
                }
                SetWishListSumIcon();
            }
            //     if (nowListNumber < 0){
            //     nowListNumber += SumCursorNumber;
            //     stackPointer = materialSumList.Count-SumCursorNumber;
            //     SetWishListSumIcon();
            // }
                // SetItemInfo(wishList[nowListNumber]);
                SetMaterialsInfo();

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
            nowCursor = sumBackground.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();

            // 一次元配列
            if(nowListNumber+stackPointer < materialSumList.Count){
                if (materialSumList[nowListNumber+stackPointer].quantity < ingredientsDB.ingredientsList[materialSumList[nowListNumber+stackPointer].ID].quantity)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }else{
                nowCursorImage.color = notCanMakeColor;
            }
                
            
            nowListNumber++;

            if(nowListNumber >= SumCursorNumber){
                if (nowListNumber + stackPointer >= materialSumList.Count){
                    nowListNumber = 0;
                    stackPointer = 0;
                }else{
                    stackPointer++;
                    nowListNumber--;
                }
                SetWishListSumIcon();
            }
            // SetItemInfo(wishList[nowListNumber]);
            SetMaterialsInfo();
        }
        if (isLongPushDown)
        {
            if (Time.realtimeSinceStartup - downTime >= pushDuration)
            {
                nowCursor = sumBackground.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();

                // 一次元配列
                
                    if(nowListNumber+stackPointer < materialSumList.Count){
                if (materialSumList[nowListNumber+stackPointer].quantity < ingredientsDB.ingredientsList[materialSumList[nowListNumber+stackPointer].ID].quantity)
                {
                    nowCursorImage.color = nomalColor;
                }
                else
                {
                    nowCursorImage.color = notCanMakeColor;
                }
            }else{
                nowCursorImage.color = notCanMakeColor;
            }
                
                nowListNumber++;
                
                if(nowListNumber >= SumCursorNumber){
                if (nowListNumber + stackPointer >= materialSumList.Count){
                    nowListNumber = 0;
                    stackPointer = 0;
                }else{
                    stackPointer++;
                    nowListNumber--;
                }
                SetWishListSumIcon();
            }
                // SetItemInfo(wishList[nowListNumber]);
                SetMaterialsInfo();

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

    // アイテムの説明欄
    public void SetItemInfo(int ItemID){
        if(ItemID != -1){//アイテムがある場合
            ItemName.text = sweetsDB.sweetsList[ItemID].name;
            ItemIcon.sprite = sweetsDB.sweetsList[ItemID].image;
            ItemInfomation.text = sweetsDB.sweetsList[ItemID].infomation;
            GameObject mtText;
            // 素材の表示
            for(int i = 0; i < 4; i++){
                // 名前
                if(i < sweetsDB.sweetsList[ItemID].materialsList.Count){
                    int materialsID = sweetsDB.sweetsList[ItemID].materialsList[i].ID;
                    int red;//文字の赤色成分
                    if(ingredientsDB.ingredientsList[materialsID].quantity < sweetsDB.sweetsList[ItemID].materialsList[i].個数){
                        red = 255;
                    }
                    else{
                        red = 0;
                    }
                    mtText = infoMaterials.transform.GetChild(i).gameObject;
                    text = mtText.GetComponent<TextMeshProUGUI>();
                
                    text.text = ingredientsDB.ingredientsList[materialsID].name;
                    var c = text.color;
                        text.color = new Color(red, c.g, c.b, 255f);

                    // 個数
                    mtText = mtText.transform.GetChild(0).gameObject;
                    text = mtText.GetComponent<TextMeshProUGUI>();
                    text.text = ingredientsDB.ingredientsList[materialsID].quantity.ToString() + "/" + sweetsDB.sweetsList[ItemID].materialsList[i].個数;
                    c = text.color;
                        text.color = new Color(red, c.g, c.b, 255f);
                }
                else{
                    mtText = infoMaterials.transform.GetChild(i).gameObject;
                    text = mtText.GetComponent<TextMeshProUGUI>();
                    var c = text.color;
                    text.color = new Color(c.r, c.g, c.b, 0f);

                    mtText = mtText.transform.GetChild(0).gameObject;
                    text = mtText.GetComponent<TextMeshProUGUI>();
                    c = text.color;
                    text.color = new Color(c.r, c.g, c.b, 0f);

                }
            }
            InfoObject.SetActive(true);
        }else{//アイテムがない場合
            InfoObject.SetActive(false);
        }
        
    }

    // ウィッシュリストの素材の数を入れたリストを作る
    public void SetWishListSum(){
        for(int i = 0; i < wishList.Count; i++){
            for(int j = 0; j < sweetsDB.sweetsList[wishList[i]].materialsList.Count; j++){
                bool match = false;
                int matchL = 0;
                for(int l = 0; l < materialSumList.Count; l++){
                    if(materialSumList[l].ID == sweetsDB.sweetsList[wishList[i]].materialsList[j].ID){
                        match = true;
                        matchL = l;
                        break;
                    }
                }

                if(!match){
                    materialSumList.Add(new MaterialSumList(sweetsDB.sweetsList[wishList[i]].materialsList[j].ID,sweetsDB.sweetsList[wishList[i]].materialsList[j].個数));
                }else{
                    materialSumList[matchL].quantity += sweetsDB.sweetsList[wishList[i]].materialsList[j].個数;
                }
            }
        }


    }

    // ウィッシュリストの素材の最大数のアイコンを設定する
    public void SetWishListSumIcon(){
        for(int i = 0; i < SumCursorNumber; i++){
            if(i < materialSumList.Count){
                frame = sumBackground.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = ingredientsDB.ingredientsList[materialSumList[i+stackPointer].ID].image;
                if(materialSumList[i+stackPointer].quantity <= ingredientsDB.ingredientsList[materialSumList[i+stackPointer].ID].quantity){
                    // アイコンから色を指定
                    image.color = canMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = nomalColor;
                    color = countTextNomalColor;
                }
                else{
                    image.color = notCanMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = notCanMakeColor;
                    color = countTextLackColor;
                }
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = ingredientsDB.ingredientsList[materialSumList[i+stackPointer].ID].name;
                text.color = color;
                icon = frame.transform.GetChild(2).gameObject;
                countText = icon.GetComponent<TextMeshProUGUI>();
                countText.text = ingredientsDB.ingredientsList[materialSumList[i+stackPointer].ID].quantity+"/"+materialSumList[i+stackPointer].quantity;
                countText.color = color;
            }
            else{
                frame = sumBackground.transform.GetChild(i).gameObject;
                image = frame.GetComponent<Image>();
                image.color = notCanMakeColor;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
                image.color = canMakeColor;
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = "";
                icon = frame.transform.GetChild(2).gameObject;
                countText = icon.GetComponent<TextMeshProUGUI>();
                countText.text = "";
            }
        }
    }

    public void SetMaterialsInfo(){
        int itemPoint = nowListNumber+stackPointer;
        if(itemPoint < materialSumList.Count){
            int infoItemID = materialSumList[itemPoint].ID;
            infoName.text = ingredientsDB.ingredientsList[infoItemID].name;
            infoIcon.sprite = ingredientsDB.ingredientsList[infoItemID].image;
            haveQuantity.text = "所持数" + "：" + ingredientsDB.ingredientsList[infoItemID].quantity;

            // 素材がどのレシピに使うかを入れる処理
        int useListLength = 4;
        int[,] useList = new int[useListLength,2];
        // useList = new int[4][2]
        int useListCount = 0;
        for(int i = 0; i < wishList.Count; i++){
            for(int j = 0; j < sweetsDB.sweetsList[wishList[i]].materialsList.Count; j++){
                if(infoItemID == sweetsDB.sweetsList[wishList[i]].materialsList[j].ID){
                    useList[useListCount,0] = sweetsDB.sweetsList[wishList[i]].ID;
                    useList[useListCount,1] = sweetsDB.sweetsList[wishList[i]].materialsList[j].個数;
                    useListCount++;
                    break;
                }
            }
        }
        while(useListCount < useListLength){
            useList[useListCount,0] = -1;
            useListCount++;
        }

            //採取できる場所を設定するときここにいれてにゃ
        // for(int i = 0; i < 4; i++){

        // }

        for(int i = 0; i < useListLength; i++){
            icon = UseMaterials.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            if(useList[i,0] != -1){
                icon.SetActive(true);
                text.text = sweetsDB.sweetsList[useList[i,0]].name;
                icon = icon.transform.GetChild(0).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = "" + useList[i,1];
            }else{
                icon.SetActive(false);
            }
            
        }
            materialsInfo.SetActive(true);
        }else{
            materialsInfo.SetActive(false);
        }
        

        

        
    }
}
