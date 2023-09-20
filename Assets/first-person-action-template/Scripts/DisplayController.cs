using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayController : MonoBehaviour
{
    //ポイントと残り時間表示用のテキストです
    [SerializeField] private TextMeshProUGUI timeAndPointText;
    
    //操作説明用のテキストです
    [SerializeField] private TextMeshProUGUI explanationText;

    //残り時間です
    [SerializeField] private float timeRemaining;

    //表示するポイント
    private int displayPoints = 0;
    
    private void Start()
    {
        explanationText.text = "操作説明\n" +
                               "・マウス移動：武器を動かす\n" +
                               "・左クリック：攻撃\n" +
                               "\n" +
                               "・スイカを攻撃で加点\n" +
                               "・爆弾を攻撃してしまうと減点\n" +
                               "・鉄球を防げないと減点（攻撃は不要）\n";
    }

    public void AddPoints(int points)
    {
        displayPoints += points;
    }
    
    public void SubtractionPoints(int points)
    {
        displayPoints -= points;
    }


    private void Update()
    {
        if (timeRemaining > 0)
        {
            //経過した時間を引いていきます
            timeRemaining -= Time.deltaTime;
        
            timeAndPointText.text = timeRemaining.ToString("0.00")+ "\n" +
                                    displayPoints + "点";
        }
        else
        {
            //リトライ画面を表示します
        }
        
        
    }
}
