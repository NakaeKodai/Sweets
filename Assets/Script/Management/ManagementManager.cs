using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagementManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public IngredientsDB ingredientsDB;
    public SweetsDB sweetsDB;
    public GameManager gameManager;

    public int Customer;//客数
    public int managementFlug = 0;
    public TextMeshProUGUI testText;
    public TextMeshProUGUI testText2;

    private int salus;//売上（最後に所持金に加算）
    private int reputation;//評判（最後に店の評判に加算）

    // 売り出すスイーツの情報を入れる
    public class showcase{
        public int ID;//スイーツのID
        public int quantity;//売り出す個数
        public int price;//スイーツの値段（データベースからいちいち出してたらメモリ食いそうなのでここにおいておく）
        public showcase(int i, int q, int p){
            ID = i;
            quantity = q;
            price = p;
        }
    }
    private List<showcase> showcaseSweets = new List<showcase>();//売り出すスイーツの情報のリスト
    private int sumSweets;//スイーツの合計

    public class demand{
        public int 需要名;//需要に名前つけるとわかりやすい
        public int 甘さ;//甘さの数値
        public int 酸味;//酸味の数値
        public int 苦味;//苦味の数値
    }
    [Header("需要関連")]
    public List<demand> demandList = new List<demand>();//需要をリスト化する


    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();

        //デバッグ用のショーケース仮置き
        showcaseSweets.Add(new showcase(0,0,100));
        showcaseSweets.Add(new showcase(1,1,200));
        showcaseSweets.Add(new showcase(2,0,300));

    }

    // Update is called once per frame
    void Update()
    {
        // testText.text = "残りの客の数："+Customer;
        if(managementFlug == 0){
            if(playerInputAction.UI.MenuSelect.triggered){
                for(int i = 0; i < showcaseSweets.Count; i++){
                    sumSweets += showcaseSweets[i].quantity;
                }
                managementFlug++;
            }
        }
        if(managementFlug == 1){
            if(playerInputAction.UI.MenuSelect.triggered){
                // testText.text = "残りの客の数："+Customer;
                testText.text = "残りの客の数："+Customer;
                int r = Random.Range(0,showcaseSweets.Count);
                if(showcaseSweets[r].quantity > 0){
                    testText2.text = sweetsDB.sweetsList[showcaseSweets[r].ID].name+"が売れた！";
                    showcaseSweets[r].quantity--;
                    sumSweets--;
                    salus += showcaseSweets[r].price;
                    reputation += 100;//評判の増減（仮）
                }else{
                    testText2.text = sweetsDB.sweetsList[showcaseSweets[r].ID].name+"は品切れで売れなかった・・・";
                    reputation -= 100;//評判の増減（仮）
                }
                Customer--;
            }
            if(Customer == 0 || sumSweets <= 0){
                managementFlug = 2;
            }
        }
        else if(managementFlug == 2){
            testText.text = "終わり";
            Debug.Log("売上："+salus+"、評判："+reputation);
        }
        
    }

    
}
