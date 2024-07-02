using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    public bool opening = false;
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    public GameObject RecipeMenuObjekt;//メニューUIの表示
    public GameObject RecipeInfoObject;//説明メニューのUI
    public GameObject WishListObject;//ウィッシュリストのオブジェクト
    public RecipeItemIcon recipeItemIcon;//アイコン設定のスクリプト
    public RecipeItemName recipeItemName;//名前設定のスクリプト
    public RecipeCursor recipeCursor;//カーソル移動のスクリプト
    public ItemInfoRecipe recipeInfoBackpack;//説明文設定のスクリプト

    private PlayerInputAction playerInputAction;

    string sortState = "ID";//ID順ならID,名前順はname,作成可能順はcanMake
    List<int> RecipeList = new List<int>();//所持アイテムだけのリスト（IDだけ）

    public int menuPage = 0;//ページ数-1で打ってほしい(プログラムのため)

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

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            //メニュー開くボタンとキャンセルボタンで反応
            if (playerInputAction.UI.OpenMenu.triggered || playerInputAction.UI.Cancel.triggered)
            {
                RecipeMenuObjekt.SetActive(false);
                RecipeInfoObject.SetActive(false);
                WishListObject.SetActive(false);
                opening = false;
            }
            else if (playerInputAction.UI.Sort.triggered)
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
            else if (playerInputAction.UI.MenuPageRight.triggered)
            {
                TurnMenuPage(1);
            }
            else if (playerInputAction.UI.MenuPageLeft.triggered)
            {
                TurnMenuPage(-1);
            }
        }
    }

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

            recipeItemIcon.RecipeIconSetting(RecipeList);
            recipeItemName.RecipeNameSetting(RecipeList);
            recipeCursor.SetmenuSelect(RecipeList);
            RecipeMenuObjekt.SetActive(true);
            WishListObject.SetActive(true);
        }
    }

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
            recipeItemIcon.RecipeIconSetting(RecipeList);
            recipeItemName.RecipeNameSetting(RecipeList);
            recipeCursor.SetmenuSelect(RecipeList);

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
            recipeItemIcon.RecipeIconSetting(RecipeList);
            recipeItemName.RecipeNameSetting(RecipeList);
            recipeCursor.SetmenuSelect(RecipeList);
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
            recipeItemIcon.RecipeIconSetting(RecipeList);
            recipeItemName.RecipeNameSetting(RecipeList);
            recipeCursor.SetmenuSelect(RecipeList);
        }
    }

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
        recipeItemIcon.RecipeIconSetting(RecipeList);
        recipeItemName.RecipeNameSetting(RecipeList);
        recipeCursor.SetmenuSelect(RecipeList);
    }

    //レシピ入手時にレシピ欄に追加する
    public void AddDataList(int i, string n, bool c)
    {
        dataList.Add(new ItemData(i, n, c));
    }

    // 作成可能かを調べる
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
}
