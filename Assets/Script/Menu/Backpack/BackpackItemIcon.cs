using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackItemIcon : MonoBehaviour
{
    public IngredientsDB ingredientsDB;
    private Image image;
    [SerializeField] private Image[] itemList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemIconSetting(List<int> BackpackList){
        
        for(int i = 0; i < BackpackList.Count; i++){
            image = itemList[i].GetComponent<Image>();
            image.sprite = ingredientsDB.ingredientsList[BackpackList[i]].image;
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 255f);
        }
        for(int i = BackpackList.Count+1; i < itemList.Length; i++){
            image = itemList[i].GetComponent<Image>();
            var c = image.color;
            image.color = new Color(c.r, c.g, c.b, 0f);
        }
    }
}
