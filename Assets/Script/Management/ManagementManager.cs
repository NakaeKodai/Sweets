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
    // [Header("需要関連")]

    // [System.Serializable]public class demand{
    //     [SerializeField]public string 需要名;//需要に名前つけるとわかりやすい
    //     [SerializeField]public int 甘さ;//甘さの数値
    //     [SerializeField]public int 酸味;//酸味の数値
    //     [SerializeField]public int 苦味;//苦味の数値
    // }
    
    // [SerializeField]public List<demand> demandList = new List<demand>();//需要をリスト化する
    [Header("↓使用する需要をここで指定してネ")]
    public int demandListNumber;//使用している需要
    public DemandDB demandDB;

    
    private List<List<int>> customerDemand = new List<List<int>>();//客が持っているほしいスイーツの味のリスト
    public int customerDemandMax = 5;//上の最大数を割り振る最大の数
    public int demandNumber = 10;//需要にかける変数

    int customerNumber = 0;//客のリストを回すやつ

    public int typeMax = 5;//スイーツの種類数
    

    [Header("倍率系")]
    public List<float> customorDiameter = new List<float>();//客のほしい味による倍率
    public List<float> demandDiameter = new List<float>();//需要の味による倍率
    public float typeMagnification = 1.5f;//ほしい種類がマッチしたときの倍率

    // Start is called before the first frame update
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        

        //デバッグ用のショーケース仮置き
        showcaseSweets.Add(new showcase(0,10,100));
        showcaseSweets.Add(new showcase(1,1,200));
        showcaseSweets.Add(new showcase(2,0,300));

    }

    // Update is called once per frame
    void Update()
    {
        // testText.text = "残りの客の数："+Customer;
        if(managementFlug == 0){
            testText.text = "残りの客の数："+Customer;
            if(playerInputAction.UI.MenuSelect.triggered){
                Setting();
                for(int i = 0; i < showcaseSweets.Count; i++){
                    sumSweets += showcaseSweets[i].quantity;
                }
                managementFlug++;
                
            }
        }
        if(managementFlug == 1){
            if(playerInputAction.UI.MenuSelect.triggered){
                // testText.text = "残りの客の数："+Customer;
                

                float a,s,n;//甘さ、酸味、苦味
                // a = customerDemand[customerNumber][0]*(1+demandDB.demandList[demandListNumber].甘さ)*demandNumber;
                // s = customerDemand[customerNumber][1]*(1+demandDB.demandList[demandListNumber].酸味)*demandNumber;
                // n = customerDemand[customerNumber][2]*(1+demandDB.demandList[demandListNumber].苦味)*demandNumber;

                a = customorDiameter[customerDemand[customerNumber][0]]*demandDiameter[demandDB.demandList[demandListNumber].甘さ]*demandNumber;
                s = customorDiameter[customerDemand[customerNumber][1]]*demandDiameter[demandDB.demandList[demandListNumber].酸味]*demandNumber;
                n = customorDiameter[customerDemand[customerNumber][2]]*demandDiameter[demandDB.demandList[demandListNumber].苦味]*demandNumber;
                //客のほしい味とか需要に基づいたスイーツごとの確率を格納する
                int[] showcaseR = new int[showcaseSweets.Count];
                int showcaseRSum = 0;
                for(int i = 0; i < showcaseR.Length; i++){
                    showcaseR[i] = sweetsDB.sweetsList[showcaseSweets[i].ID].甘さ*(int)a + sweetsDB.sweetsList[showcaseSweets[i].ID].酸味*(int)s + sweetsDB.sweetsList[showcaseSweets[i].ID].苦味*(int)n;
                    //スイーツの種類が一致したときの処理
                    if(customerDemand[customerNumber][3] == sweetsDB.sweetsList[showcaseSweets[i].ID].type){
                        showcaseR[i] = showcaseR[i]+typeMagnification;
                        Debug.Log("客のほしいスイーツと一致した");
                        if(customerDemand[customerNumber][3] == demandDB.demandList[demandListNumber].type){
                            showcaseR[i] = showcaseR[i]+typeMagnification;
                            Debug.Log("客のほしいスイーツと需要が一致した");
                        }
                    }
                    showcaseRSum += showcaseR[i];
                }

                //需要とか客が欲しい味のかみ合いですべてが0になったときの対処ですべて無作為にする
                if(showcaseRSum == 0){
                    showcaseRSum = showcaseSweets.Count;
                }

                int r = Random.Range(0,showcaseRSum);
                int result = 0;
                for(int i = 0; i < showcaseR.Length; i++){
                    r -= showcaseR[i];
                    if(r < 0){
                        result = i;
                        break;
                    }
                }
                if(showcaseSweets[result].quantity > 0){
                    testText2.text = sweetsDB.sweetsList[showcaseSweets[result].ID].name+"が売れた！";
                    showcaseSweets[result].quantity--;
                    sumSweets--;
                    salus += showcaseSweets[result].price;
                    reputation += 100;//評判の増減（仮）
                }else{
                    testText2.text = sweetsDB.sweetsList[showcaseSweets[result].ID].name+"は品切れで売れなかった・・・";
                    reputation -= 100;//評判の増減（仮）
                }
                Customer--;
                customerNumber++;
                testText.text = "残りの客の数："+Customer;
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

    //経営パートを始める前のセッティング
    void Setting(){
        //ここで客数の設定を行う

        //

        //客のほしいものの設定
        for(int i = 0; i < Customer; i++){
            int m = customerDemandMax;//客が欲しい味の最大数の割り振りをする数
            int a,s,n,t;//甘さ、酸味、苦味、ほしいスイーツの種類
            a = Random.Range(0,m);
            m -= a;
            s = Random.Range(0,m);
            m -= s;
            n = m;
            t = Random.Range(0,typeMax);
            List<int> demandResult = new List<int>();
            demandResult.Add(a);
            demandResult.Add(s);
            demandResult.Add(n);
            demandResult.Add(t);
            customerDemand.Add(demandResult);
            Debug.Log("客"+(1+i)+"のほしい味、甘さ："+a+"酸味："+s+"苦味："+n+"種類："+t);
        }
    }

    
}
