using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishListManager : MonoBehaviour
{
    private List<int> wishList = new List<int>();
    public SweetsDB sweetsDB;

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
        wishList.Add(ID);
        sweetsDB.sweetsList[ID].wishList = true;
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
}
