/* Unity UDP受信の基本テンプレート
   適当な空のGameObjectにアタッチして使います。
*/
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class SimpleUDPReceiver : MonoBehaviour
{
    private UdpClient client;
    private readonly int port = 5020; // Python側と必ず合わせる

    // Pythonから送られてくるJSONの形に合わせたクラスを定義
    [System.Serializable]
    public class ReceivedData
    {
        public string message;
        public float value;
    }

    void Start()
    {
        // 1. ポートを開いて受信用の窓口を作る
        client = new UdpClient(port);

        // 2. 非同期（ゲームの動きを止めずに裏側）で受信の待機をスタート
        client.BeginReceive(ReceiveData, null);

        Debug.Log($"ポート {port} で受信を開始しました");
    }

    // 3. データが届いた瞬間に呼ばれる関数
    private void ReceiveData(IAsyncResult result)
    {
        try
        {
            // どこから来たデータでも受け取る設定
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);

            // 届いたバイトデータを受け取る
            byte[] data = client.EndReceive(result, ref ip);

            // バイトデータを文字列（JSON）に変換
            string json = Encoding.UTF8.GetString(data);

            // JSONの文字列を、上で定義したC#のクラス(ReceivedData)に変換
            ReceivedData receivedData = JsonUtility.FromJson<ReceivedData>(json);

            // 結果を確認
            Debug.Log($"受信メッセージ: {receivedData.message}, 値: {receivedData.value}");
        }
        catch (Exception e)
        {
            Debug.LogError($"受信エラー: {e.Message}");
        }

        // 4. 次のデータを受け取るために、もう一度待機をスタートする（超重要）
        if (client != null)
        {
            client.BeginReceive(ReceiveData, null);
        }
    }

    void OnDestroy()
    {
        // アプリ終了時やオブジェクトが消える時に、必ずポートを閉じる
        if (client != null)
        {
            client.Close();
            client = null;
            Debug.Log("ポートを閉じました");
        }
    }
}