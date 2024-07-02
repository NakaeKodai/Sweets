using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeItemName : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    private TextMeshProUGUI text;//テキスト
    private GameObject icon;
    public Recipe recipeScript;

    public Color canMakeColor;
    public Color notCanMakeColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecipeNameSetting(List<int> RecipeList){
        int menuPage = recipeScript.menuPage;
        int RecipePageItem = RecipeList.Count - 6*menuPage;
        if(RecipePageItem > 6) RecipePageItem = 6;
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < RecipePageItem; i++){
            // image = itemList[i].GetComponent<Image>();
            icon = gameObject.transform.GetChild(i).gameObject;
            text = icon.GetComponent<TextMeshProUGUI>();
            text.text = sweetsDB.sweetsList[RecipeList[i]+menuPage*6].name;
            
            var c = text.color;
            if(sweetsDB.sweetsList[RecipeList[i]].canMake){
                 text.color = canMakeColor;
            }else{
                text.color = notCanMakeColor;
            }
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
