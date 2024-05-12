using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackItemIcon : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    private Image image;//画像
    [SerializeField] private Image[] itemList;//アイテムの枠
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemIconSetting(List<int> BackpackList){
        //先に所持アイテムを設定した分アイテムの枠に入れる
        for(int i = 0; i < BackpackList.Count; i++){
            image = itemList[i].GetComponent<Image>();
            image.sprite = ingredientsDB.ingredientsList[BackpackList[i]].image;
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 255f);
        }
        //空白は透明度を0にする
        for(int i = BackpackList.Count+1; i < itemList.Length; i++){
            image = itemList[i].GetComponent<Image>();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
