using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]public class sweetsRecipe
{
    public int ID;//スイーツのID
    public string name;//スイーツの名前
    public Sprite image;//画像
    public int quantity;//所持している個数
    public bool canMake;//作成可能か否か(処理を楽にするため)
    public string infomation;//説明文
    public bool wishList = false;//ウィッシュリストに入っているかを判別する
    public int type;//スイーツの種類（ケーキ等）
    [Header("味関連")]
    public int 甘さ;//甘さの数値
    public int 酸味;
    public int 苦味;
    [Header("以下は使用する素材のIDと個数")]
    public List<recipeItem> materialsList = new List<recipeItem>();
}
