using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // 通信機能を使うための準備

public class NewLog : MonoBehaviour
{
    // どこからでも NewLog.Instance.SendLog(...) と呼び出せるようにする魔法
    public static NewLog Instance { get; private set; }

    [Header("GASの設定")]
    // GASのデプロイURLをここに貼り付けます
    [SerializeField] private string gasUrl = "https://script.google.com/macros/s/あなたのGASのURL/exec";

    // 何秒ごとにまとめてログを送るか
    [SerializeField] private float sendInterval = 1.0f;

    // 送信するデータを一時的に溜めておく箱（バッファ）
    private List<string[]> logBuffer = new List<string[]>();

    private void Awake()
    {
        // 準備
        Instance = this;
    }

    private void Start()
    {
        // ゲーム開始と同時に、定期送信ループをスタート
        StartCoroutine(LogUploadRoutine());
    }

    // =========================================================
    // 📝 ログを溜め込む関数（他のスクリプトから呼ばれる）
    // =========================================================
    /// <summary>
    /// ゲームの出来事を記録する
    /// </summary>
    public void SendLog(string eventType, string playerName, float posX, float posZ)
    {
        // スプレッドシートの列に合わせる形で配列を作る（例：シート1のA列〜D列）
        string[] row = new string[] {
            eventType,            // A列: 出来事の種類（例: "Jump", "GetItem"）
            playerName,           // B列: プレイヤー名
            posX.ToString(),      // C列: X座標
            posZ.ToString()       // D列: Z座標
        };

        // 一時保管用のリストに一旦追加するだけ（まだ送らない！）
        logBuffer.Add(row);
    }

    // =========================================================
    // 🚀 一定間隔でまとめて送信するループ処理
    // =========================================================
    private IEnumerator LogUploadRoutine()
    {
        // while(true) でゲーム中ずっと繰り返す
        while (true)
        {
            // 設定した秒数（sendInterval）だけ待つ
            yield return new WaitForSeconds(sendInterval);

            // 溜まっているログがあるかチェック
            if (logBuffer.Count > 0)
            {
                // 送信用にデータをコピーして、元の箱は空っぽにする
                List<string[]> dataToSend = new List<string[]>(logBuffer);
                logBuffer.Clear();

                // 実際の送信処理（GASへ）を呼び出す
                yield return PostToGAS(dataToSend);
            }
        }
    }

    // =========================================================
    // 🌐 GASへ通信（POSTリクエスト）を送る処理
    // =========================================================
    private IEnumerator PostToGAS(List<string[]> data)
    {
        // データをインターネットで送りやすい形（JSONという文字の塊）に変換する
        string json = JsonHelper.ToJson(data);

        // 通信の準備
        using (UnityWebRequest www = new UnityWebRequest(gasUrl, "POST"))
        {
            // JSONの文字列をバイトデータ（コンピューターがわかる形）に変換してセット
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // 通信開始！終わるまで待機
            yield return www.SendWebRequest();

            // 結果の確認
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"通信失敗: {www.error}");
            }
            else
            {
                Debug.Log($"{data.Count}件のログをスプレッドシートに送りました！");
            }
        }
    }
}

// =========================================================
// 🧩 C#のリストをJSON（文字の塊）に変換するお助けツール
// =========================================================
public static class JsonHelper
{
    public static string ToJson(List<string[]> list)
    {
        // 大量の文字をくっつける時は StringBuilder が高速！
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("[");
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append("[");
            for (int j = 0; j < list[i].Length; j++)
            {
                // 文字列の中にダブルクォーテーション(")が入らないようにエスケープ処理
                sb.Append("\"" + list[i][j].Replace("\"", "\\\"") + "\"");
                if (j < list[i].Length - 1) sb.Append(",");
            }
            sb.Append("]");
            if (i < list.Count - 1) sb.Append(",");
        }
        sb.Append("]");
        return sb.ToString(); // 完成した文字の塊を返す
    }
}