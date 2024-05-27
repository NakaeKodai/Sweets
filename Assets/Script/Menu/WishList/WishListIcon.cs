using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WishListIcon : MonoBehaviour
{
    public WishListManager wishListManager;
    public SweetsDB sweetsDB;

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

    public void SetWishListIcon(){
        // 見やすくするために別スクリプトの物を置いておく
        List<int> wishList = new List<int>();
        wishList = wishListManager.wishList;
        int wishListMax = wishListManager.wishListMax;
        for(int i = 0; i < wishListMax; i++){
            if(i < wishList.Count){
                icon = gameObject.transform.GetChild(i).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = sweetsDB.sweetsList[wishList[i]].image;
            }
            else{
                icon = gameObject.transform.GetChild(i).gameObject;
                image = icon.GetComponent<Image>();
                image.sprite = nomalIcon;
            }
        }
    }
}
