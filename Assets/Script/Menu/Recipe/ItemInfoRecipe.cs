using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoRecipe : MonoBehaviour
{
    public TextMeshProUGUI ItemName;//名前
    public Image ItemIcon;//アイコン
    public TextMeshProUGUI ItemInfomation;//説明文
    // public TextMeshProUGUI itemNum;
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    public GameObject ItemInfoObject;

    private GameObject materials;
    private TextMeshProUGUI text;
    public GameObject materialsName;
    public GameObject materialsQuantity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo(int ItemID){
        if(ItemID != -1){//アイテムがある場合
            ItemName.text = sweetsDB.sweetsList[ItemID].name;
            ItemIcon.sprite = sweetsDB.sweetsList[ItemID].image;
            ItemInfomation.text = sweetsDB.sweetsList[ItemID].infomation;
            // 素材の表示
            for(int i = 0; i < 4; i++){
                // 名前
                if(i < sweetsDB.sweetsList[ItemID].materialsList.Count){
                materials = materialsName.transform.GetChild(i).gameObject;
                text = materials.GetComponent<TextMeshProUGUI>();
                int materialsID = sweetsDB.sweetsList[ItemID].materialsList[i].ID;
                text.text = ingredientsDB.ingredientsList[materialsID].name;
                var c = text.color;
                text.color = new Color(c.r, c.g, c.b, 255f);

                // 個数
                materials = materialsQuantity.transform.GetChild(i).gameObject;
                text = materials.GetComponent<TextMeshProUGUI>();
                text.text = ingredientsDB.ingredientsList[materialsID].quantity.ToString() + "/" + sweetsDB.sweetsList[ItemID].materialsList[i].個数;
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, 255f);
                }
                else{
                materials = materialsName.transform.GetChild(i).gameObject;
                text = materials.GetComponent<TextMeshProUGUI>();
                var c = text.color;
                text.color = new Color(c.r, c.g, c.b, 0f);

                materials = materialsQuantity.transform.GetChild(i).gameObject;
                text = materials.GetComponent<TextMeshProUGUI>();
                c = text.color;
                text.color = new Color(c.r, c.g, c.b, 0f);

                }
            }
            ItemInfoObject.SetActive(true);
        }else{//アイテムがない場合
            ItemInfoObject.SetActive(false);
        }
        
    }
}
