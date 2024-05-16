using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackItemIcon : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    private Image image;//画像
    private GameObject icon;
    public Backpack backpackScript;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemIconSetting(List<int> BackpackList){
        int menuPage = backpackScript.menuPage;
        int BackpackPageItem = BackpackList.Count - 40*menuPage;
        if(BackpackPageItem > 40) BackpackPageItem = 40;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < BackpackPageItem; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            image = icon.GetComponent<Image>();
            image.sprite = ingredientsDB.ingredientsList[BackpackList[i+menuPage*40]].image;
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 255f);
        }
        //空白は透明度を0にする
        for(int i = BackpackPageItem; i < 40; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            image = icon.GetComponent<Image>();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
