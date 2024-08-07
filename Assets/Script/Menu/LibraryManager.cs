using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LibraryManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction; 
    [SerializeField] private IngredientsDB ingredientsDB;

    public GameObject itemInfoObject;
    public bool autoSetList;
    public bool opening = false;
    public bool firstOpening = false;//最初に開いたかを調べる

    private int menuPage = 0;//ページ数-1で打ってほしい(プログラムのため)
    public int pageItem = 6;//１ページで表示されるアイテム数
    public GameObject items; //開いているページのオブジェクト？
    private GameObject backGround;
    private Image image;//画像
    private TextMeshProUGUI text;//テキスト
    private GameObject icon;

    private int endPageNumber;//最後のページの番号

    //カーソル移動
    [Header("カーソル関連")]
    private GameObject nowCursor;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    // int[,] menuList = new int[3,2];
    private int nowListNumber = 0;
    private int beforeListNum = -1;
    private bool isLongPushUp,isLongPushDown,isLongPushRight,isLongPushLeft; //長押し判定
    private  float pushDuration = 0.3f;
    private float downTime = 0f;

    public int rowNum = 2; //持ち物の横のアイコンの数
    private int columnNum;

    [Header("説明欄")]
    public ItemInfoBackpack itemInfoBackpack;
    public TextMeshProUGUI ItemName;//名前
    public Image ItemIcon;//アイコン
    public TextMeshProUGUI ItemInfomation;//説明文
    public TextMeshProUGUI itemNum;
    public GameObject ItemInfoObject;
    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        columnNum = pageItem/rowNum;
    }

    // Update is called once per frame
    void Update()
    {
        //メニュー開くボタンとキャンセルボタンで反応
        if(playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered){
            gameObject.SetActive(false);
            // itemInfoObject.SetActive(false);
            opening = false;
        }
        if(playerInputAction.UI.MenuPageRight.triggered){
            TurnMenuPage(1);
        }
        if(playerInputAction.UI.MenuPageLeft.triggered){
            TurnMenuPage(-1);
        }
        MoveCursor();
    }

    public void OpenLibrary(){
        // if(!opening){
        //     opening = true;
            
        //     gameObject.SetActive(true);
        // }

        // 最後のページの設定
        for(int i = ingredientsDB.ingredientsList.Count-1; i > endPageNumber*pageItem; i--){
            if(ingredientsDB.ingredientsList[i].got){
                endPageNumber = i/pageItem;
                break;
            }
        }

        LibrarySetting();
        SetItemInfo(0);
        gameObject.SetActive(true);
    }

    public void LibrarySetting(){
        int LibraryPageItem = ingredientsDB.ingredientsList.Count - pageItem*menuPage;
        if(LibraryPageItem > pageItem) LibraryPageItem = pageItem;
        for(int i = 0; i < LibraryPageItem; i++){
            if(ingredientsDB.ingredientsList[i+pageItem*menuPage].got){
                backGround = items.transform.GetChild(i).gameObject;
                icon = backGround.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = ingredientsDB.ingredientsList[i+pageItem*menuPage].image;
                // image.sprite = ingredientsDB.ingredientsList[0].image;
                icon = backGround.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = ingredientsDB.ingredientsList[i+pageItem*menuPage].name.ToString();
                var c = image.color;
                image.color = new Color(c.r, c.g, c.b, 255f);
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, 255f);
            }else{
                backGround = items.transform.GetChild(i).gameObject;
                icon = backGround.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                icon = backGround.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                var c = image.color;
                image.color = new Color(c.r, c.g, c.b, 0f);
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, 0f);
            }
        }
        //空白は透明度を0にする
        for(int i = LibraryPageItem; i < pageItem; i++){
            backGround = items.transform.GetChild(i).gameObject;
            icon = backGround.transform.GetChild(0).gameObject;
            image = icon.GetComponent<Image>();
            icon = backGround.transform.GetChild(1).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0f);
            c = text.color;
            text.color = new Color(c.r, c.g, c.b, 0f);
        }
        SetItemInfo(pageItem*menuPage + nowListNumber);
    }

    // ページ変えの処理
    public void TurnMenuPage(int TurnPage){//右にいくなら1,左なら-1
        menuPage += TurnPage;
        if(pageItem*endPageNumber < pageItem*menuPage){
            menuPage = 0;
        }
        else if(menuPage < 0){
            menuPage = endPageNumber;
        }
        // ItemSetting(BackpackList);
        // backpackItemIcon.ItemIconSetting(BackpackList);
        // backpackItemQuantity.ItemQuantitySetting(BackpackList);
        LibrarySetting();
    }

    // 説明欄に書き込むやーつ
    public void SetItemInfo(int ItemID){
        if(ItemID < ingredientsDB.ingredientsList.Count){//アイテムがある場合
        if(ingredientsDB.ingredientsList[ItemID].got){//アイテムを入手済みの場合
            ItemName.text = ingredientsDB.ingredientsList[ItemID].name;
            ItemIcon.sprite = ingredientsDB.ingredientsList[ItemID].image;
            ItemInfomation.text = ingredientsDB.ingredientsList[ItemID].infomation;
            itemNum.text = "所持数　" + ingredientsDB.ingredientsList[ItemID].quantity + "";
            ItemInfoObject.SetActive(true);
        }else{
            ItemInfoObject.SetActive(false);
        }
        }else{//アイテムがない場合
            ItemInfoObject.SetActive(false);
        }
        
    }

    //カーソル移動
    public void MoveCursor()
    {
        if(nowListNumber != beforeListNum)
        {//多分後で修正する
            nowCursor = items.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = selectColor;
        }
        //右移動
        if(playerInputAction.UI.CursorMoveRight.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushRight = true;
            nowCursor = items.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = nomalColor;
            nowListNumber ++;
            if(nowListNumber >= pageItem) nowListNumber = 0;
            SetItemInfo(nowListNumber + pageItem*menuPage);
        }
        if(isLongPushRight)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber ++;
                if(nowListNumber >= pageItem) nowListNumber = 0;
                SetItemInfo(nowListNumber + pageItem*menuPage);
                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveRight.canceled += ctx => {
            isLongPushRight = false;
            pushDuration = 0.3f;
        };

        //左移動
        if(playerInputAction.UI.CursorMoveLeft.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushLeft = true;
            nowCursor = items.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = nomalColor;
            nowListNumber --;
            if(nowListNumber < 0) nowListNumber =  pageItem - 1;
            SetItemInfo(nowListNumber + pageItem*menuPage);
        }
        if(isLongPushLeft)
        {
        if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber =  pageItem - 1;
                SetItemInfo(nowListNumber + pageItem*menuPage);
                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveLeft.canceled += ctx => {
            isLongPushLeft = false;
            pushDuration = 0.3f;
        };

        //上移動
        if(playerInputAction.UI.CursorMoveUp.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushUp = true;
            nowCursor = items.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = nomalColor;
            nowListNumber -= rowNum;
            if(nowListNumber < 0) nowListNumber +=  pageItem;
            SetItemInfo(nowListNumber + pageItem*menuPage);
        }
        if(isLongPushUp)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber -= rowNum;
                if(nowListNumber < 0) nowListNumber +=  pageItem;
                SetItemInfo(nowListNumber + pageItem*menuPage);
                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveUp.canceled += ctx => {
            isLongPushUp = false;
            pushDuration = 0.3f;
        };
            
        //下移動
        if(playerInputAction.UI.CursorMoveDown.triggered)
        {
            downTime = Time.realtimeSinceStartup;
            isLongPushDown = true;
            nowCursor = items.transform.GetChild(nowListNumber).gameObject;
            nowCursorImage = nowCursor.GetComponent<Image>();
            nowCursorImage.color = nomalColor;
            nowListNumber += rowNum;
            if(nowListNumber >= pageItem) nowListNumber -= pageItem;
            SetItemInfo(nowListNumber + pageItem*menuPage);
        }
        if(isLongPushDown)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber += rowNum;
                if(nowListNumber >= pageItem) nowListNumber -= pageItem;
                SetItemInfo(nowListNumber + pageItem*menuPage);
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
