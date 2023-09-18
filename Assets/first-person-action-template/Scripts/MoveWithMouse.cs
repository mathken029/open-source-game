using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour
{
    //座標用の変数
    Vector3 mousePos, worldPos;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //マウス座標の取得
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換。Zは10fにしないとなぜかうしろにさがっていく
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,10f));
        //ワールド座標を自身の座標に設定
        transform.position = worldPos;
    }
}
