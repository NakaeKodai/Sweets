using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatus : ScriptableObject
{
    [Header("店関連")]
    public int grade;//店のグレード
    public int money;//所持金
    public int popularity;//人気度

    [Header("プレイヤー関連")]
    public int hp;//生命
    public int attack;//攻撃力
    public int defense;//防御力
}

// [System.Serializable]
// public class StatusData{
//     [Header("店関連")]
//     public int grade;//店のグレード
//     public int money;//所持金
//     public int popularity;//人気度
    
// }
