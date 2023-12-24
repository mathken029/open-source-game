using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;
using unityroom.Api;

public class DisplayController : MonoBehaviour
{
    //ポイントと残り時間表示用のテキストです
    [SerializeField] private TextMeshProUGUI timeAndPointText;
    
    //操作説明用のテキストです
    [SerializeField] private TextMeshProUGUI explanationText;
    
    //勝敗表示用のテキストの親オブジェクトです
    [SerializeField] private GameObject resultTextParent;
    
    //勝敗表示用のテキストです
    [SerializeField] private TextMeshProUGUI resultText;
    
    //リトライボタンです
    [SerializeField] private GameObject retryButton;

    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController enemyController;
    
    //経過時間です
    private float timeElapsed = 0;
    
    //表示するポイントです
    private float displayPoints = 0;
    
    //ポイントが何度も送信されないためのフラグです
    private bool pointsSendFlag = true;
    
    private void Start()
    {
        audioSource.Play();
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
        if (enemyController.CheckBeated() || playerController.CheckBeated())
        {
            //結果・リトライボタンを表示します
            resultTextParent.SetActive(true);
            retryButton.SetActive(true);
            
            if (enemyController.CheckBeated() )
            {
                resultText.text = "Win";

                if (pointsSendFlag)
                {
                    //早く敵を倒したほど高得点とするため、ポイントから経過時間を引きます
                    displayPoints -= timeElapsed;
                
                    //unityroomにスコアを送付します
                    UnityroomApiClient.Instance.SendScore(1, displayPoints, ScoreboardWriteMode.HighScoreDesc);
                    pointsSendFlag = false;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Retry();
                }
            }
            else if (playerController.CheckBeated())
            {
                resultText.text = "Lose";
            }
        }
        else
        {
            //経過した時間を足していきます
            timeElapsed += Time.deltaTime;
        }
        
        timeAndPointText.text = timeElapsed.ToString("0.00")+ "\n" +
                                displayPoints + "点";
    }

    public void Retry()
    {
        //残り時間を初期化します
        timeElapsed = 0.00f;
        
        //再度ゲーム終了した際にスコアが送付されるようにします
        pointsSendFlag = true;
        
        //プレイヤーと敵を初期化します
        playerController.Reset();
        enemyController.Reset();
        
        //シーンを再度読み込みます
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        //リトライボタンを非表示にします
        retryButton.SetActive(false);
        
        //BGMを再度再生します
        audioSource.Play();

    }
}
