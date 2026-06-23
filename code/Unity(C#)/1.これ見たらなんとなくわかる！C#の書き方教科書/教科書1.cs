/* =========================================================
   Unity C# 基礎学習辞書 (UnityBasicsDictionary.cs)
   

　　1. よく使うコンポーネントと変数の宣言
　　2. Unityの基本イベント関数
　　3. コルーチン
　　4. シングルトンパターン

　　
   ※このファイルは学習・閲覧用です。
========================================================= */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBasicsDictionary
{
    // =========================================================
    // 1. よく使うコンポーネントと変数の宣言
    // =========================================================
    public class VariablesAndComponents : MonoBehaviour
    {
        [Header("コンポーネント（型）の宣言")]

        // 【 GameObject 】：ゲームオブジェクトそのもの（プレハブの生成やオンオフに使用）
        [SerializeField] private GameObject playerPrefab;

        // 【 Transform 】：位置・回転・大きさ（座標データだけが欲しい時に使用）
        [SerializeField] private Transform spawnPoint;

        // 【 AudioSource 】：音を鳴らすスピーカー役
        [SerializeField] private AudioSource bgmSource;

        // 【 Rigidbody 】：物理演算（重力や速度の計算に使用）
        [SerializeField] private Rigidbody playerRb;

        // 【 Animator 】：アニメーションの切り替えに使用
        [SerializeField] private Animator characterAnimator;

        [Header("複数データの管理")]

        // 【 配列 (Array) 】：数が決まっているものを入れる（例：3種類の武器）
        [SerializeField] private GameObject[] weaponPrefabs;

        // 【 リスト (List) 】：ゲーム中に数が増減するものを入れる（例：今いる敵のリスト）
        [SerializeField] private List<GameObject> activeEnemies = new List<GameObject>();
    }


    // =========================================================
    // 2. Unityの基本イベント関数（ライフサイクル）
    // =========================================================
    public class LifecycleFunctions : MonoBehaviour
    {
        // 【 Awake 】
        // タイミング：スクリプトが読み込まれた瞬間（Startよりも前）。
        // 用途：自分自身の初期化。他のオブジェクトを必要としない設定など。
        private void Awake()
        {
            // 初期化処理をここに書く
        }

        // 【 Start 】
        // タイミング：ゲーム開始時（オブジェクトが有効になって最初のフレーム）。
        // 用途：他のオブジェクトの取得や、ゲーム開始時のステータス設定など。
        private void Start()
        {
            // 開始時の処理をここに書く
        }

        // 【 Update 】
        // タイミング：毎フレーム（常に呼ばれ続ける）。
        // 用途：プレイヤーの入力監視、タイマーのカウントなど。
        private void Update()
        {
            // 毎フレーム行う処理をここに書く
        }

        // 【 FixedUpdate 】
        // タイミング：一定時間ごと（デフォルトでは0.02秒ごと）。
        // 用途：Rigidbodyを使った物理演算（移動やジャンプなど）。
        private void FixedUpdate()
        {
            // 物理演算の処理をここに書く
        }
    }


    // =========================================================
    // 3. コルーチン (Coroutine)
    // =========================================================
    public class CoroutineExample : MonoBehaviour
    {
        // 概要：時間の経過（待機）を伴う処理を作るための機能。

        private void Start()
        {
            // コルーチンを呼び出す時は StartCoroutine() を使う
            StartCoroutine(WaitAndExecuteRoutine());
        }

        // 宣言の仕方：IEnumerator（アイエニュメレーター）を使う
        private IEnumerator WaitAndExecuteRoutine()
        {
            // 1. すぐに実行される処理
            Debug.Log("処理を開始します。");

            // 2. ここで3秒間待機する（Update等は裏で動き続けます）
            yield return new WaitForSeconds(3.0f);

            // 3. 3秒後に再開される処理
            Debug.Log("3秒経過しました！");
        }
    }


    // =========================================================
    // 4. シングルトンパターン (Singleton Pattern)
    // =========================================================
    public class GameManagerSingleton : MonoBehaviour
    {
        // 概要：ゲーム内に「絶対に1つしか存在しない管理者」を作るテクニック。
        // これを作ると、他から「GameManagerSingleton.Instance.〇〇」でアクセス可能になる。

        // 宣言：public static にして、どこからでもアクセスできるようにする
        public static GameManagerSingleton Instance { get; private set; }

        private void Awake()
        {
            // 自分が最初の1人目なら、Instanceに自分を登録する
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // シーン遷移で消えないようにする
            }
            // 既に他のInstanceが存在している（＝自分が2人目）なら、自分自身を消す
            else
            {
                Destroy(gameObject);
            }
        }
    }
}