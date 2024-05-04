using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestPoint : MonoBehaviour
{
    public int itemId;//採取できるアイテムIDの指定
    public int maxAmount = 2;//最大採取量、これをプレイヤーの変数からとってスキルによる採取量変化とかやりたい
    [SerializeField] public IngredientsDB ingredientsDB;//データベースの取得
    bool HarvestPointKiller = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HarvestPointKiller) Destroy(gameObject);
    }

    public void Harvesting(){
        int rand = Random.Range(1,101);//1から100を乱数で指定
            Debug.Log(rand);
            int amount;//採取量の指定
            // 採取量を乱数で変更
            if(rand >= 80){
                amount = maxAmount;
            }
            else if(rand >= 50){
                amount = maxAmount-1;
                if(amount <= 0) amount = 1;
            }
            else if(rand >= 20){
                amount = maxAmount-2;
                if(amount <= 0) amount = 1;
            }
            else{
                amount = maxAmount-3;
                if(amount <= 0) amount = 1;
            }

            ingredientsDB.ingredientsList[itemId].quantity += amount;//採取した個数分をアイテムの個数に追加
            
            Debug.Log(ingredientsDB.ingredientsList[itemId].name+"を"+amount+"個手に入れた");
            this.HarvestPointKiller = true;
    }
}
