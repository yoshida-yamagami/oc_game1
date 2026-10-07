using UnityEngine;

/// <summary>
/// いったん床判定はしっかりできた。
/// 負荷軽減として、動かない床はFixedUpdateがいらないかもね、という話。
/// その対応は後回しにして、とりあえず次はトラップの整理を優先。
/// </summary>


public class GroundCollider : MonoBehaviour
{

    // 親オブジェクトのメインスクリプトへの参照
    private Object parent;
    private BoxCollider2D collider; // 地面の当たり判定
    private LayerMask playerLayer; // Playerレイヤーを設定
    private bool hasPlayer = false;

    private void Start()
    {
        parent = GetComponentInParent<Object>();
        collider = GetComponent<BoxCollider2D>();
        playerLayer = LayerMask.GetMask("Player"); // Playerレイヤーをプレイヤーと認識
    }
    private void FixedUpdate()
    {
        // 自分のコライダーの少し上（0.1f）のエリアにプレイヤーがいるか検知
        // BoxCastはコライダーの形そのまま上方向にセンサーを飛ばすイメージです
        Vector2 size = collider.size;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0f, Vector2.up, 0.1f, playerLayer);

        if (hit.collider != null)
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

    ///// オブジェクトがプレイヤーと触れた時の処理
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (parent != null && collision.gameObject.tag == "Player")
    //    {
    //        // 親のスクリプトに「自分の役割」と「ぶつかった相手」を伝える
    //        parent.OnChildCollisionEnter("Ground", collision);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        parent.OnChildCollisionExit("Ground", collision);
    //    }
    //}
}
