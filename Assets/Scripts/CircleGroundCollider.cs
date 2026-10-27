using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// いったん床判定はしっかりできた。
/// 負荷軽減として、動かない床はFixedUpdateがいらないかもね、という話。
/// その対応は後回しにして、とりあえず次はトラップの整理を優先。
/// </summary>

public class CircleGroundCollider : MonoBehaviour
{
    private CircleCollider2D groundCollider; // 地面の当たり判定
    private LayerMask playerLayer; // Playerレイヤーを設定
    private bool hasPlayer = false;

    private void Start()
    {
        groundCollider = GetComponent<CircleCollider2D>();
        playerLayer = LayerMask.GetMask("Player"); // Playerレイヤーをプレイヤーと認識
    }

    private void FixedUpdate()
    {
        // 自分のコライダーの少し上（0.1f）のエリアにプレイヤーがいるか検知
        // BoxCastはコライダーの形そのまま上方向にセンサーを飛ばすイメージです
        //Vector2 size = collider.size;
        //RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0f, Vector2.up, 0.1f, playerLayer);

        // 1. コライダーの上面（Top）の位置を計算
        Bounds bounds = groundCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.max.y);

        // 2. センサーのサイズ（幅はコライダーと同じ、高さは薄い判定ラインにする）
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, 0.05f); // 横幅を少し狭めて端の誤検知を防ぐ

        // 3. コライダーの上面から上方向（Vector2.up）へ 0.1f だけ判定を飛ばす
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.up, 0.1f, playerLayer);
        if (hit.collider != null)
        {
            // プレイヤーが「オブジェクトの上面より上にいるか」を念のため確認
            // （プレイヤーの足元/中心が上面より上にある場合のみ乗ったと判定）
            if (hit.collider.transform.position.y >= bounds.max.y)
            {
                // プレイヤーが上に乗っている場合
                Player player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    // 親オブジェクト（Object.csがついている一番上の親）を取得してプレイヤーを子にする
                    player.SetPassengerOf(transform.parent);
                    hasPlayer = true;
                }
            }
        }
        else
        {
            // プレイヤーが上にいない場合
            if (hasPlayer)
            {
                // 直前まで乗っていたなら親子関係を解除
                // 念のため、現在の子が本当にプレイヤーの時だけ解除する
                Player player = transform.parent.GetComponentInChildren<Player>();
                if (player != null)
                {
                    player.ReleasePassenger();
                }

                hasPlayer = false;
            }
        }
    }
}
