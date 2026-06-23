/* =========================================================
   Unity C# 基礎学習辞書 パート3 改訂版 (UI・最新入力・シーン遷移)
   

   10. UI操作（テキストの変更とボタン）
   11. 最新Input System: 直接読み取り方式
   12. 最新Input System: イベント（コールバック）方式
   13. シーンの切り替え


   ※このファイルは学習・閲覧用のチートシートです。
   ※入力処理を使うには Package Manager から「Input System」の
    インストールが必要です。
========================================================= */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ★重要：UIやシーン遷移、新しい入力システムを使う時は、専用の「おまじない(using)」が必要です！
using UnityEngine.UI;               // 古いUI（ImageやSliderなど）を使う時
using TMPro;                        // 新しくて綺麗な文字（TextMeshPro）を使う時
using UnityEngine.SceneManagement;  // シーンの切り替えを行う時
using UnityEngine.InputSystem;      // ★最新のInput Systemを使う時


namespace UnityBasicsDictionary
{
    // =========================================================
    // 10. UI操作（テキストの変更とボタン）
    // =========================================================
    public class UIBasics : MonoBehaviour
    {
        // 【 TextMeshProUGUI 】：画面に文字を表示するコンポーネント
        [SerializeField] private TextMeshProUGUI scoreText;

        // 【 Button 】：押せるボタンのコンポーネント
        [SerializeField] private Button startButton;

        private int score = 0;

        private void Start()
        {
            // プログラムからボタンに「押された時の処理」を登録する方法
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
            }
        }

        // ボタンが押された時に呼ばれる関数（publicにしておくとInspectorからも設定できます）
        public void OnStartButtonClicked()
        {
            score += 100;

            if (scoreText != null)
            {
                // .text に文字列を代入することで画面の表示が変わります
                scoreText.text = "SCORE: " + score.ToString();
            }

            Debug.Log("スタートボタンが押されました！");
        }
    }


    // =========================================================
    // 11. 最新Input System: 直接読み取り方式（一番簡単・直感的）
    // =========================================================
    public class NewInputDirect : MonoBehaviour
    {
        // 概要：旧 Input.GetKeyDown 等の代わりに、今すぐサクッと判定したい時に使います。
        // デバッグ作業や、ちょっとしたUIの操作に非常に便利です。

        private void Update()
        {
            // 【 キーボード (Keyboard) 】
            if (Keyboard.current != null)
            {
                // wasPressedThisFrame = 押した瞬間
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Debug.Log("スペースキーを押した！");
                }

                // isPressed = 押しっぱなし
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    Debug.Log("右矢印を押しっぱなし！");
                }
            }

            // 【 マウス (Mouse) 】
            if (Mouse.current != null)
            {
                // 左クリック（leftButton）の判定
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    // 画面のどこをクリックしたか、座標(X, Y)を取得
                    Vector2 clickPos = Mouse.current.position.ReadValue();
                    Debug.Log("クリック座標: " + clickPos);
                }
            }

            // 【 ゲームパッド / コントローラー (Gamepad) 】
            if (Gamepad.current != null)
            {
                // buttonSouth は PSコンの「×」や Xboxコンの「A」など、下側のボタンに相当します
                if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                {
                    Debug.Log("決定ボタンを押した！");
                }
            }

            // 【 タッチパネル (Touchscreen) 】
            if (Touchscreen.current != null)
            {
                // スマホ画面をタップした瞬間の判定
                if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                {
                    Debug.Log("画面をタッチした！");
                }
            }
        }
    }


    // =========================================================
    // 12. 最新Input System: イベント（コールバック）方式 ★本格ゲームの標準
    // =========================================================
    public class NewInputEvent : MonoBehaviour
    {
        // 概要：PlayerInputコンポーネントの「Invoke Unity Events」等と組み合わせて使います。
        // 「どのボタンが押されたか」をUnityエディタ側で設定・変更できるため、
        // スマホ、PC、ゲームパッドの全てに一括で対応できる最強の設計です。

        private Vector2 moveInput;

        // 【 移動の入力 】：スティックや十字キーの入力を受け取る
        // (InputAction.CallbackContext) という「入力の情報が詰まった箱」を受け取ります。
        public void OnMove(InputAction.CallbackContext context)
        {
            // スティックの傾きや入力値を Vector2(X, Y) として読み取る
            moveInput = context.ReadValue<Vector2>();
        }

        // 【 ボタンの入力 】：ジャンプや爆弾設置などのアクションを受け取る
        public void OnActionA(InputAction.CallbackContext context)
        {
            // context.performed = 「ボタンがしっかり押し込まれた瞬間」だけ true になります。
            // これを書かないと、離した時にも呼ばれてしまうため必須の安全装置です！
            if (context.performed)
            {
                Debug.Log("アクションA（ジャンプ等）を実行しました！");
            }
        }

        public void OnActionB(InputAction.CallbackContext context)
        {
            // canceled = 「ボタンを離した瞬間」の判定も簡単に作れます
            if (context.canceled)
            {
                Debug.Log("アクションBのボタンを離しました！タメ攻撃発射！");
            }
        }

        private void Update()
        {
            // 実際のキャラクターの移動処理は、UpdateやFixedUpdateで行うのが基本です
            if (moveInput != Vector2.zero)
            {
                // ここで Rigidbody.velocity 等に moveInput の値を渡して移動させます
                // Debug.Log("移動中: X=" + moveInput.x + " / Y=" + moveInput.y);
            }
        }
    }


    // =========================================================
    // 13. シーンの切り替え (SceneManagement)
    // =========================================================
    public class SceneBasics : MonoBehaviour
    {
        // 概要：タイトル画面からゲーム本編へ移動したり、リトライする時に使います。
        // ※File > Build Settings... で「Scenes In Build」にシーンを登録しておく必要があります。

        public void LoadGameScene()
        {
            // 【 LoadScene 】：指定した名前のシーンを読み込む
            SceneManager.LoadScene("MainGameScene");
        }

        public void ReloadCurrentScene()
        {
            // 【 応用 】：今のシーンの名前を自動で取得して読み込み直す（＝リトライ処理）
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}