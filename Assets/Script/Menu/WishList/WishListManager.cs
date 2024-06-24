using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Sprite nomalIcon;

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
    public int GiveWithList(int number){
        return wishList[number];
    }

    public void OpenWithListInRecipe(){
        SetWishListIcon();
    }

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
            }
            else{
                // icon = gameObject.transform.GetChild(i).gameObject;
                frame = background.transform.GetChild(i).gameObject;
                icon = frame.transform.GetChild(0).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
            }
        }
    }
}
