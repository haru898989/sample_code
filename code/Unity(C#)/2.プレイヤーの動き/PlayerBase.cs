using UnityEngine;
using UnityEngine.InputSystem;

// 全キャラクターの土台となる親クラス
public class PlayerBase : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField] protected float playerSpeed = 7f; // 移動速度

    // 内部で使う変数
    protected Vector2 moveInput = Vector2.zero; // スティックの入力値
    protected Rigidbody rb;                     // 物理演算用

    // virtual: 子クラスで上書き(カスタマイズ)できるようにする魔法のキーワード
    protected virtual void Start()
    {
        // 必要なコンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        // 毎フレーム、入力に応じた移動を実行する
        PlayerMove();
    }

    // =========================================================
    // 🎮 コントローラー入力の受け取り (Input System)
    // =========================================================

    // 左スティック・十字キーの入力 (移動)
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        // スティックの傾き具合をVector2(X, Y)で受け取る
        moveInput = context.ReadValue<Vector2>();
    }

    // Aボタンの入力 (例：ジャンプや決定)
    public virtual void OnActionA(InputAction.CallbackContext context)
    {
        // context.performed は「ボタンが押し込まれた瞬間」を判定します
        if (context.performed)
        {
            Debug.Log("Aボタンが押されました！：ジャンプなどのアクションを実行");
            // ここにAボタン用のアクションを書く
        }
    }

    // Bボタンの入力 (例：攻撃やキャンセル)
    public virtual void OnActionB(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Bボタンが押されました！：爆弾設置などのアクションを実行");
            // ここにBボタン用のアクションを書く
        }
    }

    // =========================================================
    // 🏃 実際の動作処理
    // =========================================================
    protected void PlayerMove()
    {
        // 入力値(X, Y)を3D空間の動き(X, Z)に変換して速度を計算
        Vector3 moveVelocity = new Vector3(moveInput.x * playerSpeed, 0f, moveInput.y * playerSpeed);

        // Rigidbodyに速度を代入して移動させる
        rb.linearVelocity = moveVelocity;
    }
}