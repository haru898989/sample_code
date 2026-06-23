/* =========================================================
   Unity C# 基礎学習辞書 パート2 (追加の必須知識)
   


　　5.アクセス修飾子
　　6.メソッド（関数）の作り方と使い方
　　7.条件分岐と繰り返しの書き方
　　8.コンポーネントの取得
　　9.当たり判定



   ※このファイルも学習・閲覧用のチートシートです。
========================================================= */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBasicsDictionary
{
    // =========================================================
    // 5. アクセス修飾子 (public / private / SerializeField)
    // =========================================================
    public class AccessModifiers : MonoBehaviour
    {
        // 【 public 】
        // 外部のスクリプトから自由に読み書きでき、Unityのエディタ（Inspector）にも表示されます。
        // ※注意：どこからでも書き換えられるため、初心者は多用しがちですがバグの原因になります。
        public int publicScore = 0;

        // 【 private 】
        // このスクリプトの内部でしか使えません。エディタにも表示されません。
        // ※基本：変数はなるべくprivateにして安全に守るのがプログラミングの鉄則です。
        private int privateHealth = 100;

        // 【 [SerializeField] + private 】 ★一番のオススメ！
        // 外部のスクリプトからの変更は防ぎつつ（安全）、Unityのエディタ上からは数値を調整できます。
        // スピードや攻撃力など、ゲームバランスの調整が必要な数値に使います。
        [SerializeField] private float moveSpeed = 5.0f;
    }


    // =========================================================
    // 6. メソッド（関数）の作り方と使い方
    // =========================================================
    public class MethodBasics : MonoBehaviour
    {
        private void Start()
        {
            // 自分で作った関数を呼び出す
            TakeDamage(20);

            // 戻り値（結果）を受け取る
            int result = CalculateAdd(5, 10);
        }

        // 【 void 】：戻り値（結果のデータ）を返さない、ただ処理を実行するだけの関数
        // (int damage) は引数（ひきすう）と呼び、外部から受け取るデータです。
        private void TakeDamage(int damage)
        {
            Debug.Log(damage + " のダメージを受けた！");
        }

        // 【 int, float, boolなど 】：処理のあとに、その型のデータを返す関数
        private int CalculateAdd(int a, int b)
        {
            int sum = a + b;
            return sum; // return で計算結果を呼び出し元に返す
        }
    }


    // =========================================================
    // 7. 条件分岐(if) と 繰り返し(for / foreach)
    // =========================================================
    public class ControlFlow : MonoBehaviour
    {
        private void Start()
        {
            int hp = 0;

            // 【 if文 】：条件によって処理を分ける
            if (hp > 50)
            {
                Debug.Log("元気です");
            }
            else if (hp > 0)
            {
                Debug.Log("ピンチ！");
            }
            else
            {
                Debug.Log("ゲームオーバー");
            }

            // 【 for文 】：決まった回数だけ処理を繰り返す（例：3回ログを出す）
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(i + "回目の処理");
            }

            // 【 foreach文 】：配列やリストの中身をすべて取り出して処理する
            string[] names = { "スライム", "ゴブリン", "ドラゴン" };
            foreach (string enemyName in names)
            {
                Debug.Log(enemyName + " があらわれた！");
            }
        }
    }


    // =========================================================
    // 8. コンポーネントの取得 (GetComponent)
    // =========================================================
    public class ComponentRetrieval : MonoBehaviour
    {
        private Rigidbody myRb;

        private void Start()
        {
            // 【 GetComponent<型>() 】
            // このスクリプトがくっついているオブジェクトと同じ場所にある、
            // 別のコンポーネント（Rigidbodyなど）を探して取得します。超頻出です。
            myRb = GetComponent<Rigidbody>();

            if (myRb != null)
            {
                myRb.AddForce(0, 300, 0); // ジャンプさせる
            }
        }
    }


    // =========================================================
    // 9. 当たり判定 (Collision と Trigger)
    // =========================================================
    public class CollisionBasics : MonoBehaviour
    {
        // 【 OnCollisionEnter 】
        // 物理的な「衝突」が起きた瞬間に呼ばれます。（壁にぶつかる、ボールがバウンドするなど）
        // ※お互いに Collider と Rigidbody が必要で、Is Trigger はオフにします。
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("敵とぶつかった！");
            }
        }

        // 【 OnTriggerEnter 】
        // 物理的な衝突はせず、「すり抜けるエリア」に入った瞬間に呼ばれます。（アイテム取得、ゴール判定など）
        // ※対象か自分の Collider の Is Trigger をオンにします。
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                Debug.Log("アイテムをゲットした！");
                Destroy(other.gameObject); // アイテムを消す
            }
        }
    }
}