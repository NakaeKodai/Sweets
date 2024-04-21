using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //TextMeshProを使う場合は読み込みが必要

public class Renda : MonoBehaviour
{
    private PlayerInputAction playerInputAction;  //InputSystemを入れている変数 

    private int score;
    private float time;
    public float limitTime = 10.0f;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] GameObject resultTextWindow;
    void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        time = limitTime;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            if(playerInputAction.Player.MiniGameAction.triggered)
            {
                score++;
            }
            timeText.text = "残り時間：" + time.ToString("F2");
            scoreText.text = "スコア:" + score;
        }else 
        {
            if(score <= 10) resultText.text = "うーん……";
            else if(score <= 20) resultText.text = "もう少し";
            else if(score <= 30) resultText.text = "いい感じ！";
            else if(score <= 40) resultText.text = "とてもいい感じ！!";
            else resultText.text = "最高！！！";
            resultTextWindow.SetActive(true);
        }
        
    }
}
