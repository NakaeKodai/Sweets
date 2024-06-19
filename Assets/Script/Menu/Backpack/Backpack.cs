using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [Header("メニュー開くたびにリスト作るやつ(デバッグ用)")]
    public bool autoSetList;
    public bool opening = false;
    public bool firstOpening = false;//最初に開いたかを調べる
    // private List<int> BackpackList = new List<int>();
    public IngredientsDB ingredientsDB;
    // public GameObject 
    public GameObject BackpackObjekt;//メニューUIの表示
    public GameObject ItemInfoObject;//説明メニューのUI
    public BackpackItemIcon backpackItemIcon;//アイコン設定のスクリプト
    public BackpackItemQuantity backpackItemQuantity;//所字数設定のスクリプト
    public BackpackCursor backpackCursor;//カーソル移動のスクリプト
    public ItemInfoBackpack itemInfoBackpack;//説明文設定のスクリプト

    private PlayerInputAction playerInputAction;
    public MenuManager menuManager;
    public GameManager gameManager;

    string sortState = "ID";//ID順ならID,名前順はname,所字数順はquantity
    List<int> BackpackList = new List<int>();//所持アイテムだけのリスト（IDだけ）
    
    public int menuPage = 0;//ページ数-1で打ってほしい(プログラムのため)

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

    public List<ItemData> dataList = new List<ItemData>();

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(opening){
            //メニュー開くボタンとキャンセルボタンで反応
            if(playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered){
                BackpackObjekt.SetActive(false);
                ItemInfoObject.SetActive(false);
                opening = false;
            }else if(playerInputAction.UI.Sort.triggered){
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
            else if(playerInputAction.UI.MenuPageRight.triggered){
                TurnMenuPage(1);
            }
            else if(playerInputAction.UI.MenuPageLeft.triggered){
                TurnMenuPage(-1);
            }
        }
    }

    public void OpenBackpack(){
        if(!opening){
            opening = true;
            if(autoSetList){
                AutoSetDataList();
            }

             BackpackList.Clear();
            for(int i = 0; i < dataList.Count; i++){
                    if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                        BackpackList.Add(dataList[i].ID);
                    }
                }

            backpackItemIcon.ItemIconSetting(BackpackList);
            backpackItemQuantity.ItemQuantitySetting(BackpackList);
            backpackCursor.SetmenuSelect(BackpackList);
            BackpackObjekt.SetActive(true);
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
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            backpackItemIcon.ItemIconSetting(BackpackList);
            backpackItemQuantity.ItemQuantitySetting(BackpackList);
            backpackCursor.SetmenuSelect(BackpackList);

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
            backpackItemIcon.ItemIconSetting(BackpackList);
            backpackItemQuantity.ItemQuantitySetting(BackpackList);
            backpackCursor.SetmenuSelect(BackpackList);
        }else if(sortState == "quantity"){
            BackpackList.Clear();
            dataList.Sort((a,b) => b.quantity - a.quantity);
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < dataList.Count; i++){
                if(dataList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    BackpackList.Add(dataList[i].ID);
                    j++;
                }
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            backpackItemIcon.ItemIconSetting(BackpackList);
            backpackItemQuantity.ItemQuantitySetting(BackpackList);
            backpackCursor.SetmenuSelect(BackpackList);
        }
    }

    public void TurnMenuPage(int TurnPage){//右にいくなら1,左なら-1
        menuPage += TurnPage;
        if(BackpackList.Count < 40*menuPage){
            menuPage = 0;
        }
        else if(menuPage < 0){
            menuPage = BackpackList.Count/40;
        }
        backpackItemIcon.ItemIconSetting(BackpackList);
        backpackItemQuantity.ItemQuantitySetting(BackpackList);
        backpackCursor.SetmenuSelect(BackpackList);
    }

    //アイテム入手時に持ち物欄に追加する
    public void AddDataList(int i, string n, int q){
         dataList.Add(new ItemData(i, n, q));
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
}
