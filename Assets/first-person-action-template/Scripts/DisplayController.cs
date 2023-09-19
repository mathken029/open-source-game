using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayController : MonoBehaviour
{
    //ポイント表示
    [SerializeField] private TextMeshProUGUI pointText;
    
    //ポイント表示
    [SerializeField] private TextMeshProUGUI explanationText;

    // ゲームのポイント（別クラスに分離する）
    private int point = 0;

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

    public void AddPoint(int amount)
    {
        point += amount;
    }

    private void Update()
    {
        pointText.text = "得点：" + point + "点";
        
    }
}
