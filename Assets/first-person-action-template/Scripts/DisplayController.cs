using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    // ゲームのポイント（別クラスに分離する）
    private int point = 0;
    
    public void AddPoint(int amount)
    {
        point += amount;
    }
		
    void OnGUI ()
    {
        GUI.Box (new Rect (Screen.width - 260, 10, 100, 30), "得点：" + point + "点");

        GUI.Box (new Rect (Screen.width - 260, 40, 250, 150), "操作説明");
        GUI.Label (new Rect (Screen.width - 245, 60, 250, 30), "マウス移動：武器を動かす");
        GUI.Label (new Rect (Screen.width - 245, 80, 250, 30), "マウス右クリック：武器を振る");
        GUI.Label (new Rect (Screen.width - 245, 100, 250, 30), "");
        GUI.Label (new Rect (Screen.width - 245, 120, 250, 30), "・武器を動かして鉄球を防御する");
        GUI.Label (new Rect (Screen.width - 245, 150, 250, 30), "・武器を振ってスイカを斬る");
    }
}
