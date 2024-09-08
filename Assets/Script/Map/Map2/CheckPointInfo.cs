using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckPointInfo : MonoBehaviour
{
    public GameObject pointNameBackground;
    public TextMeshProUGUI pointName;

    [Header("ここの場所の名前を入れてね")]
    public string placeName;

    [Header("プレイヤーのファストトラベル先の座標を書いて")]
    public float x;
    public float y;
    public float z;

    [Header("マップアイコンのファストトラベル先の座標を書いて (計算で求まるなら消していい)")]
    public float iconX;
    public float iconY;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Target"))
        {
            pointName.text = placeName;
            pointNameBackground.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Target"))
        {
            pointName.text = null;
            pointNameBackground.SetActive(false);
        }
    }
}
