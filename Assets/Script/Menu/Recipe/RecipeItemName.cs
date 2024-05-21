using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeItemName : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    private TextMeshProUGUI text;//テキスト
    private GameObject icon;
    public Recipe recipeScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecipeSetting(List<int> RecipeList){
        int menuPage = recipeScript.menuPage;
        int RecipePageItem = RecipeList.Count - 6*menuPage;
        if(RecipePageItem > 6) RecipePageItem = 6;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < RecipePageItem; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            text.text = ingredientsDB.ingredientsList[RecipeList[i]+menuPage*6].name;
            
            var c = text.color;
            text.color = new Color(c.r, c.g, c.b, 255f);
        }
        //空白は透明度を0にする
        for(int i = RecipePageItem; i < 6; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            var c = text.color;
            text.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
