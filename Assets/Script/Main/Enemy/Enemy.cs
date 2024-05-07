using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private byte hp = 10;

    [Header("percentは合計が100以下で設定してにゃ")]
    [Header("percentは小数点第一位まで")]
    public List<HarvestItem> HarvestList = new List<HarvestItem>();//採取できるアイテムのリスト
    public int maxAmount = 2;//最大採取量、これをプレイヤーの変数からとってスキルによる採取量変化とかやりたい
    [SerializeField] public IngredientsDB ingredientsDB;//データベースの取得
    public int harvestCount;//採取する回数
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            // 取得できるアイテムの乱数設定
        int selectItemId = HarvestList[0].Id;
            int[] itemPercent = new int[1000];
            int listNumber = 0;
            int listPercentNum = 0;
            // itemPercentにListのpercent*10の分の要素にIDを入れることで乱数をやりやすくしている
            for(int l = 0; l < 1000; l++){
                if(listNumber < HarvestList.Count){
                    itemPercent[l] = HarvestList[listNumber].Id;
                    if(l == (int)(HarvestList[listNumber].percent * 10) + listPercentNum - 1){
                    listPercentNum += (int)(HarvestList[listNumber].percent * 10);
                    listNumber++;
                }
                } 
                else itemPercent[l] = -1;
            }

        // アイテム採取の乱数決定
        for(int i = 0; i < harvestCount; i++){
            int rand = Random.Range(1,101);//1から100を乱数で指定
            // Debug.Log(rand);
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

            // 乱数によって取得できるアイテムを調整
            do{
                int randItem = Random.Range(0,1000);
                selectItemId = itemPercent[randItem];
            }while(selectItemId == -1);
            

            ingredientsDB.ingredientsList[selectItemId].quantity += amount;//採取した個数分をアイテムの個数に追加
            
            Debug.Log(ingredientsDB.ingredientsList[selectItemId].name+"を"+amount+"個手に入れた");
        }
            Destroy(gameObject);
        }
    }

    public void Damage(byte attackPoint)
    {
        hp -= attackPoint;
        Debug.Log(hp);
    }
}
