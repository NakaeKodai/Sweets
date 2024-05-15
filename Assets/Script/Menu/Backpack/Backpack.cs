using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public bool opening = false;
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
            }
        }
    }

    public void OpenBackpack(){
        if(!opening){
            opening = true;
            List<int> BackpackList = new List<int>();//所持アイテムだけのリスト（IDだけ）
            int j = 0;//インベントリのリスト用
            for(int i = 0; i < ingredientsDB.ingredientsList.Count; i++){
                if(ingredientsDB.ingredientsList[i].quantity != 0){//アイテムの所字数が0じゃなければBackpackListに追加する
                    BackpackList.Add(ingredientsDB.ingredientsList[i].ID);
                    Debug.Log(ingredientsDB.ingredientsList[BackpackList[j]].name);
                    j++;
                }
            }
            //アイコンの代入を行うスクリプトにBackpackListを投げたのち、UIを表示させる
            backpackItemIcon.ItemIconSetting(BackpackList);
            backpackItemQuantity.ItemQuantitySetting(BackpackList);
            itemInfoBackpack.SetItemInfo(BackpackList[0]);
            backpackCursor.SetmenuSelect(BackpackList);
            BackpackObjekt.SetActive(true);
            ItemInfoObject.SetActive(true);
        }
    }
}
