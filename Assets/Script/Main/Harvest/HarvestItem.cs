using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public class HarvestItem
{
    public int Id;//アイテムのID
    public double percent;//出現確率(小数点第一位まで)
    // 確率は合計が100までにする
}
