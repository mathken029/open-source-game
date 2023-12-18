using System.IO;
using System.Text;
using UnityEngine;
using unityroom.Api;
using System.Reflection;

public class WriteAPIKeyWhenBuild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //APIキーを書いたファイルはプロジェクトフォルダの直下に以下のファイル名で配置してください
        StreamReader sr = new StreamReader(
            "apiKey.txt", Encoding.GetEncoding("Shift_JIS"));

        string apiKey = sr.ReadToEnd();

        sr.Close();

        UnityroomApiClient unityroomApiClient = GetComponent<UnityroomApiClient>();

        // HmacKeyを設定
        // privateなためリフレクションでアクセス
        var type = unityroomApiClient.GetType();
        var clientHmacKeyFieldInfo = type.GetField("HmacKey", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.SetField);
        clientHmacKeyFieldInfo?.SetValue(unityroomApiClient, apiKey);
        
        Debug.Log(apiKey);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
