using UnityEngine;
using UnityEngine.AI; // NavMeshAgentを使うための準備

// NPCの「体（移動とアクション）」を担当する子クラス
[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : NPCBase
{
    private NavMeshAgent agent;

    private void Start()
    {
        // 自動で障害物を避けて歩いてくれるコンポーネントを取得
        agent = GetComponent<NavMeshAgent>();
    }

    // FixedUpdateではなく、親に合わせてUpdateを使用（※用途に合わせて変更可）
    protected override void Update()
    {
        // 親クラス(NPCBase)のUpdateを呼び出して、頭脳を働かせる
        base.Update();

        // 頭脳が決めた「目的地」へ、NavMeshAgentを使って移動を開始する
        agent.SetDestination(targetPosition);

        // 状態（ステート）に応じた具体的なアクションを実行する
        ExecuteAction();
    }

    // =========================================================
    // 🏃 アクション：ステートに応じた行動をとる
    // =========================================================
    private void ExecuteAction()
    {
        // 目的地にほぼ到着しているかチェック
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // 現在のステート（状態）ごとにやることを変える
            switch (currentState)
            {
                case NPCState.Area:
                case NPCState.GetExp:
                    // エリア塗りや経験値集めモードなら、爆弾を置く
                    NPCBomb();
                    break;

                case NPCState.Chase:
                    // 追いかけている時は爆弾を置かない（例）
                    break;
            }
        }
    }

    private void NPCBomb()
    {
        Debug.Log("爆弾を設置しました！");
        // ここに爆弾を置く処理が入る
    }
}