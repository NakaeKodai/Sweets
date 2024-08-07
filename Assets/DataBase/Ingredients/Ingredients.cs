using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public class Ingredients
{
    public int ID;//アイテムのID
    public string name;//アイテム名
    public Sprite image;//アイテムの画像
    public int quantity;//アイテムの個数
    public string infomation;//アイテムの説明文
    public bool got;//入手済みを判別する
}
