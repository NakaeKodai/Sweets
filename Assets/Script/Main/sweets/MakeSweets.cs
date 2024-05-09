using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSweets : MonoBehaviour
{
    [SerializeField] public IngredientsDB ingredientsDB;
    [SerializeField] public SweetsDB sweetsDB;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cook(int sweetsID){
        bool canCook = true;
        int materialID;//素材のID
        // List<int> materialIDList = new List<int>();
        int[] materialIDs = new int[sweetsDB.sweetsList[sweetsID].materialsList.Count];
        int difference;//素材の現在所字数と必要個数の差
        int nowQuantity;//現在個数
        int requiredQuantity;//必要個数
        for(int i = 0; i < sweetsDB.sweetsList[sweetsID].materialsList.Count; i++){
            materialID = sweetsDB.sweetsList[sweetsID].materialsList[i].ID;
            nowQuantity = ingredientsDB.ingredientsList[materialID].quantity;
            requiredQuantity = sweetsDB.sweetsList[sweetsID].materialsList[i].個数;
            if(nowQuantity < requiredQuantity){
                difference = requiredQuantity - nowQuantity;
                Debug.Log(ingredientsDB.ingredientsList[materialID].name+"が"+difference+"個足りない");
                canCook = false;
            }
            materialIDs[i] = materialID;
        }

        // スイーツがすべて足りているときのみ以下を実行しスイーツを作成
        if(canCook){
            for(int i = 0; i < sweetsDB.sweetsList[sweetsID].materialsList.Count; i++){
                materialID = materialIDs[i];
                requiredQuantity = sweetsDB.sweetsList[sweetsID].materialsList[i].個数;
                ingredientsDB.ingredientsList[materialID].quantity -= requiredQuantity;
            }
            sweetsDB.sweetsList[sweetsID].quantity++;
            Debug.Log(sweetsDB.sweetsList[sweetsID].name+"を一つ作った。");
        }
    }
}
