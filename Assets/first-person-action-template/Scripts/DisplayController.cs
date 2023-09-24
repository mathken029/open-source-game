using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class DisplayController : MonoBehaviour
{
    //ポイントと残り時間表示用のテキストです
    [SerializeField] private TextMeshProUGUI timeAndPointText;
    
    //操作説明用のテキストです
    [SerializeField] private TextMeshProUGUI explanationText;
    
    //リトライボタンです
    [SerializeField] private GameObject retryButton;

    //残り時間です
    [SerializeField] private float timeRemaining;

    //BGMです
    [SerializeField] private AudioClip bgmOrpheusInTheUnderWorld;

    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;
    
    //表示するポイント
    private int displayPoints = 0;
    
    private void Start()
    {
        audioSource.PlayOneShot(bgmOrpheusInTheUnderWorld);
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
        }
        else
        {
            //表示が-0.01になってしまうため戻す
            timeRemaining = 0.00f;
            
            //リトライボタンを表示します
            retryButton.SetActive(true);
            
            //BGMを止めます
            audioSource.Stop();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Retry();
            }
        }
        
        timeAndPointText.text = timeRemaining.ToString("0.00")+ "\n" +
                                displayPoints + "点";
    }

    public void Retry()
    {
        //シーンを再度読み込みます
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        //リトライボタンを非表示にします
        retryButton.SetActive(false);
        
        //BGMを再度再生します
        audioSource.PlayOneShot(bgmOrpheusInTheUnderWorld);

    }
}
