using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackItemQuantity : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    private TextMeshProUGUI text;//テキスト
    private GameObject icon;
    public Backpack backpackScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemQuantitySetting(List<int> BackpackList){
        int menuPage = backpackScript.menuPage;
        int BackpackPageItem = BackpackList.Count - 40*menuPage;
        if(BackpackPageItem > 40) BackpackPageItem = 40;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < BackpackPageItem; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            text.text = ingredientsDB.ingredientsList[BackpackList[i]+menuPage*40].quantity.ToString();
            
            var c = text.color;
            text.color = new Color(c.r, c.g, c.b, 255f);
        }
        //空白は透明度を0にする
        for(int i = BackpackPageItem; i < 40; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            var c = text.color;
            text.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
