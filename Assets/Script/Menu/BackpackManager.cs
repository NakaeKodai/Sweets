using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    public class ItemData{
        public int ID;
        public string name;
        public int quantity;
        public ItemData(int i, string n, int q){
            ID = i;
            name = n;
            quantity = q;
        }
    }
    public static List<ItemData> dataList = new List<ItemData>();
    //dataListにAddとかしたい場合は、BackpackManager.dataList.Add(new BackpackManager.ItemData(i, n, q)) と書いてねㇵァㇳ

    [SerializeField] private IngredientsDB ingredientsDB;
    List<int> BackpackList = new List<int>();//所持アイテムだけのリスト（IDだけ）

    public GameObject itemInfoObject;
    public bool autoSetList;
    public bool opening = false;
    public bool firstOpening = false;//最初に開いたかを調べる

    string sortState = "ID";//ID順ならID,名前順はname,所字数順はquantity


    public int menuPage = 0;//ページ数-1で打ってほしい(プログラムのため)
    public GameObject items; //開いているページのオブジェクト？
    private GameObject backGround;
    private Image image;//画像
    private TextMeshProUGUI text;//テキスト
    private GameObject icon;

    //カーソル移動
    private GameObject nowCursor;
    private Image nowCursorImage;//画像
    public Color nomalColor;
    public Color selectColor;
    int[,] menuList = new int[5,8];
    private int nowListNumber = 0;
    private int beforeListNum = -1;
    private bool isLongPushUp,isLongPushDown,isLongPushRight,isLongPushLeft; //長押し判定
    private  float pushDuration = 0.3f;
    private float downTime = 0f;
    public ItemInfoBackpack itemInfoBackpack;

    public int rowNum = 10; //持ち物の横のアイコンの数

    [Header("説明欄")]
    public TextMeshProUGUI ItemName;//名前
    public Image ItemIcon;//アイコン
    public TextMeshProUGUI ItemInfomation;//説明文
    public TextMeshProUGUI itemNum;
    public GameObject ItemInfoObject;

    
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //メニュー開くボタンとキャンセルボタンで反応
        if(playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered){
            gameObject.SetActive(false);
            itemInfoObject.SetActive(false);
            opening = false;
        }
        if(playerInputAction.UI.Sort.triggered){
            if(sortState == "ID"){
            sortState = "name";
                ListSort();
            }else if(sortState == "name"){
                sortState = "quantity";
                ListSort();
            }else if(sortState == "quantity"){
                sortState = "ID";
                ListSort();
            }
        }
        if(playerInputAction.UI.MenuPageRight.triggered){
            TurnMenuPage(1);
        }
        if(playerInputAction.UI.MenuPageLeft.triggered){
            TurnMenuPage(-1);
        }
        MoveCursor();
    }

    public void AddDataList(int i, string n, int q)
    {
        dataList.Add(new ItemData(i, n, q));
    }


    //持ち物を開く＆持ち物のセット
    public void OpenBackpack(){
        Debug.Log("AAAA");
        if(!opening){
            Debug.Log("BBBB");
            opening = true;
            if(autoSetList){
                AutoSetDataList();
            }

            BackpackList.Clear();
            for(int i = 0; i < dataList.Count; i++){
                if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    Debug.Log("CCCC");
                    BackpackList.Add(dataList[i].ID);
                }
            }

            ItemSetting(BackpackList);
            // ItemQuantitySetting(BackpackList);
            SetmenuSelect(BackpackList);
            gameObject.SetActive(true);
        }
    }

    public void ListSort(){
        if(sortState == "ID"){
            dataList.Sort((a,b) => a.ID - b.ID);
            BackpackList.Clear();
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < dataList.Count; i++){
                if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    BackpackList.Add(dataList[i].ID);
                    j++;
                }
            }

        }else if(sortState == "name"){
            dataList.Sort((a,b) => string.Compare(a.name, b.name));
            BackpackList.Clear();
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < dataList.Count; i++){
                if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    BackpackList.Add(dataList[i].ID);
                    j++;
                }
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
        }else if(sortState == "quantity"){
            dataList.Sort((a,b) => b.quantity - a.quantity);
            BackpackList.Clear();
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < dataList.Count; i++){
                if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    BackpackList.Add(dataList[i].ID);
                    j++;
                }
            }
        }

        //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
        ItemSetting(BackpackList);
        // ItemIconSetting(BackpackList);
        // ItemQuantitySetting(BackpackList);
        SetmenuSelect(BackpackList);
    }

    public void TurnMenuPage(int TurnPage){//右にいくなら1,左なら-1
        menuPage += TurnPage;
        if(BackpackList.Count < 40*menuPage){
            menuPage = 0;
        }
        else if(menuPage < 0){
            menuPage = BackpackList.Count/40;
        }
        ItemSetting(BackpackList);
        // backpackItemIcon.ItemIconSetting(BackpackList);
        // backpackItemQuantity.ItemQuantitySetting(BackpackList);
        SetmenuSelect(BackpackList);
    }
    // アイテム使い切ったときに持ち物欄から削除する
    public void RemoveDataList(int ID){
        ItemData deleteItem;
        for(int i = 0; i < dataList.Count; i++){
            if(ID == dataList[i].ID){
                deleteItem = new ItemData(dataList[i].ID, dataList[i].name, dataList[i].quantity);
                dataList.Remove(deleteItem);
            }
        }
    }

    // メニューを開くたびにID順に並ぶ(デバッグ用)
    public void AutoSetDataList(){
        dataList.Clear();
        int j = 0;//インベントリのリスト用
        for(int i = 0; i < ingredientsDB.ingredientsList.Count; i++){
            if(ingredientsDB.ingredientsList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                dataList.Add(new ItemData(ingredientsDB.ingredientsList[i].ID, ingredientsDB.ingredientsList[i].name, ingredientsDB.ingredientsList[i].quantity));
                j++;
            }
        }
        for(int i = 0; i < dataList.Count; i++){
            if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                BackpackList.Add(dataList[i].ID);
                j++;
            }
        }
    }

    public void ItemSetting(List<int> BackpackList){
        Debug.Log(BackpackList.Count);
        int BackpackPageItem = BackpackList.Count - 40*menuPage;
        if(BackpackPageItem > 40) BackpackPageItem = 40;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < BackpackPageItem; i++){
            backGround = items.transform.GetChild(i).gameObject;
            icon = backGround.transform.GetChild(0).gameObject;
            image = icon.GetComponent<Image>();
            image.sprite = ingredientsDB.ingredientsList[BackpackList[i+menuPage*40]].image;
            // image.sprite = ingredientsDB.ingredientsList[0].image;
            icon = backGround.transform.GetChild(1).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            text.text = ingredientsDB.ingredientsList[BackpackList[i+menuPage*40]].quantity.ToString();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 255f);
            c = text.color;
            text.color = new Color(c.r, c.g, c.b, 255f);
        }
        //空白は透明度を0にする
        for(int i = BackpackPageItem; i < 40; i++){
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

    public void SetmenuSelect(List<int> BackpackList){
        // int menuPage = backpackScript.menuPage;
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
        SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
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
            if(nowListNumber >= menuList.Length) nowListNumber = 0;
            SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
        }
        if(isLongPushRight)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber ++;
                if(nowListNumber >= menuList.Length) nowListNumber = 0;
                SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
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
            if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
            SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
        }
        if(isLongPushLeft)
        {
        if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber --;
                if(nowListNumber < 0) nowListNumber =  menuList.Length - 1;
                SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
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
            if(nowListNumber < 0) nowListNumber +=  menuList.Length;
            SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
        }
        if(isLongPushUp)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber -= rowNum;
                if(nowListNumber < 0) nowListNumber +=  menuList.Length;
                SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
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
            if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
            SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
        }
        if(isLongPushDown)
        {
            if(Time.realtimeSinceStartup - downTime >=  pushDuration)
            {
                nowCursor = items.transform.GetChild(nowListNumber).gameObject;
                nowCursorImage = nowCursor.GetComponent<Image>();
                nowCursorImage.color = nomalColor;
                nowListNumber += rowNum;
                if(nowListNumber >= menuList.Length) nowListNumber -= menuList.Length;
                SetItemInfo(menuList[(nowListNumber/8), (nowListNumber%8)]);
                pushDuration = 0.1f;
                downTime = Time.realtimeSinceStartup;
            }
        }
        playerInputAction.UI.CursorMoveDown.canceled += ctx => {
            isLongPushDown = false;
            pushDuration = 0.3f;
        };
    }

    // 説明欄に書き込むやーつ
    public void SetItemInfo(int ItemID){
        if(ItemID != -1){//アイテムがある場合
            ItemName.text = ingredientsDB.ingredientsList[ItemID].name;
            ItemIcon.sprite = ingredientsDB.ingredientsList[ItemID].image;
            ItemInfomation.text = ingredientsDB.ingredientsList[ItemID].infomation;
            itemNum.text = "所持数　" + ingredientsDB.ingredientsList[ItemID].quantity + "";
            ItemInfoObject.SetActive(true);
        }else{//アイテムがない場合
            ItemInfoObject.SetActive(false);
        }
        
    }
}
