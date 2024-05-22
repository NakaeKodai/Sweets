using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoBackpack : MonoBehaviour
{
    public TextMeshProUGUI ItemName;//名前
    public Image ItemIcon;//アイコン
    public TextMeshProUGUI ItemInfomation;//説明文
    public TextMeshProUGUI itemNum;
    public IngredientsDB ingredientsDB;
    public GameObject ItemInfoObject;

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
