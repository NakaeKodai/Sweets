using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStatus : ScriptableObject
{
    public List<EnemyData> enemyList = new List<EnemyData>();
}

[System.Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public int hp;
    public int attack;
    public int defense;
    public float hostileDistance; //敵対距離
    public float lostDistance; //非敵対距離
    public float attackDistance;//攻撃距離
}
