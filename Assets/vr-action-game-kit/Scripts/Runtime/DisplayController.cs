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
    
    //リトライボタンです
    [SerializeField] private GameObject retryButton;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController enemyController;

    [Header("初期ポイントです")][SerializeField] private float startPoints;
    [Header("敵を倒したときに加算されるポイントです")][SerializeField] private float enemyBeatPoints;
    
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
        startPoints += points;
    }
    
    public void SubtractionPoints(int points)
    {
        startPoints -= points;
    }


    private void Update()
    {
        if (enemyController.CheckBeated())
        {
            //結果・リトライボタンを表示します
            retryButton.SetActive(true);
            
            if (pointsSendFlag)
            {
                //敵を倒したのでポイントを加算します
                displayPoints += enemyBeatPoints;
                
                //unityroomにスコアを送付します
                UnityroomApiClient.Instance.SendScore(1, displayPoints, ScoreboardWriteMode.HighScoreDesc);
                pointsSendFlag = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Retry();
            }
        }
        else
        {
            //経過した時間を足していきます
            timeElapsed += Time.deltaTime;
            
            //早く敵を倒したほど高得点とするため、ポイントから経過時間を引きます
            displayPoints = startPoints - timeElapsed;
        }
        
        timeAndPointText.text = timeElapsed.ToString("0.00")+ "\n" +
                                displayPoints.ToString("0.00") + "点";
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
