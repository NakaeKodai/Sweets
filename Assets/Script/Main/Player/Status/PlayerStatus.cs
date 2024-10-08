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
}

// [System.Serializable]
// public class StatusData{
//     [Header("店関連")]
//     public int grade;//店のグレード
//     public int money;//所持金
//     public int popularity;//人気度
    
// }
