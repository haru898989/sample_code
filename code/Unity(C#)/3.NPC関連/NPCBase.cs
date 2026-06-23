using UnityEngine;

// NPCの「頭脳（思考とセンサー）」を担当する親クラス
public class NPCBase : MonoBehaviour
{
    // 1. 状態（ステート）の定義
    // 列挙型(enum)を使うことで、今の状態を分かりやすい名前で管理できます
    public enum NPCState
    {
        Chase,  // 敵を追いかける
        GetExp, // 経験値を拾う
        Area    // エリアを塗る（通常時）
    }
    public NPCState currentState; // 現在のステート

    [Header("センサーの設定")]
    public float searchRange = 5f; // 探索範囲
    public LayerMask enemyLayer;   // 敵のレイヤー
    public LayerMask expLayer;     // 経験値のレイヤー

    protected Vector3 targetPosition; // 最終的に向かうべき目的地の座標

    protected virtual void Update()
    {
        // 毎フレーム、AIの思考をアップデートする
        StateChange();
    }

    // =========================================================
    // 🧠 思考回路：状況に応じてステート（状態）を切り替える
    // =========================================================
    protected void StateChange()
    {
        // ① 周りに敵がいるか、経験値があるかをセンサー(透明な球体)でチェック
        bool hasEnemy = Physics.CheckSphere(transform.position, searchRange, enemyLayer);
        bool hasExp = Physics.CheckSphere(transform.position, searchRange, expLayer);

        // ② if ~ else if で「優先順位」をつけて状態を決める
        // （上にある条件ほど優先される！）
        if (hasEnemy)
        {
            currentState = NPCState.Chase;
            // 実際のコードではここで一番近い敵の座標を targetPosition に入れます
        }
        else if (hasExp)
        {
            currentState = NPCState.GetExp;
            // 実際のコードではここで一番近い経験値の座標を targetPosition に入れます
        }
        else
        {
            currentState = NPCState.Area;
            // 敵も経験値もなければ、エリアの座標を targetPosition に入れます
        }
    }
}