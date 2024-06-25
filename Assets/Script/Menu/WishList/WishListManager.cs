using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WishListManager : MonoBehaviour
{
    public List<int> wishList = new List<int>();
    public SweetsDB sweetsDB;
    [Header("ウィッシュリストの最大数")]
    public int wishListMax;

    public GameObject background;
    private GameObject frame;
    private Image image;//画像
    private GameObject icon;
    private TextMeshProUGUI text;

    public Sprite nomalIcon;

    public Color nomalColor;
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

    // 指定したIDをウィッシュリストに追加
    public void AddWishList(int ID){
        if(wishList.Count < wishListMax){
            wishList.Add(ID);
            sweetsDB.sweetsList[ID].wishList = true;
            Debug.Log(sweetsDB.sweetsList[ID].name+"をウィッシュリストに追加した。");
        }
    }

    // 指定したIDと一致する要素を削除
    public void RemoveWishList(int ID){
        wishList.Remove(ID);
        sweetsDB.sweetsList[ID].wishList = false;
    }

    // 指定した数字の要素を取得する。
    public int GiveWishList(int number){
        return wishList[number];
    }

    // レシピのとこ用のアイコンだけ表示
    public void SetWishListIcon(){
        // 見やすくするために別スクリプトの物を置いておく
        // int wishListMax = wishListMax;
        for(int i = 0; i < wishListMax; i++){
            if(i < wishList.Count){
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = sweetsDB.sweetsList[wishList[i]].image;
                if(sweetsDB.sweetsList[wishList[i]].canMake){
                    // アイコンから色を指定
                    image.color = canMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = nomalColor;
                }
                else{
                    image.color = notCanMakeColor;
                    image = frame.GetComponent<Image>();
                    image.color = notCanMakeColor;
                }
            }
            else{
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                image = frame.GetComponent<Image>();
                image.color = notCanMakeColor;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
            }
        }
    }

    // ウィッシュリスト専用のメニューでのアイコン設定（名前付き）
    public void SetWishList(){
        for(int i = 0; i < wishListMax; i++){
            if(i < wishList.Count){
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = sweetsDB.sweetsList[wishList[i]].image;
                icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = sweetsDB.sweetsList[wishList[i]].name;
            }
            else{
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;icon = frame.transform.GetChild(1).gameObject;
                text = icon.GetComponent<TextMeshProUGUI>();
                text.text = "";
            }
        }
    }
}
