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
    [Header("以下は使用する素材のIDと個数")]
    public List<recipeItem> materialsList = new List<recipeItem>();
}
