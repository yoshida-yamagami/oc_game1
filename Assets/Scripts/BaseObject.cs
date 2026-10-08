using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ゲーム内オブジェクトの祖
/// </summary>
public class BaseObject : MonoBehaviour
{
    // オブジェクトが動き出すタイミングを定義
    public enum MovementTrigger
    {
        None,
        GameStart,
        AreaTrigger
    }

    // オブジェクトの動き方を定義
    public enum MovementPattern
    {
        FirstForceOnly, // 動き出しのみ力をかける
        ConstantSpeed   // 一定速度で移動
    }

    /// <summary>
    /// このスクリプトをアタッチする全オブジェクト共通設定
    /// </summary>
    [SerializeField] protected bool isDamageObject = false; // プレイヤーに触れた時にダメージを受けるかどうか
    [SerializeField] protected MovementTrigger movementTriggerType; // 動き出すタイミングの設定
    [SerializeField] protected MovementPattern movementPatternType; // 動き方の設定
    // 動き始めのみ力をかける場合の設定
    [SerializeField] protected Vector2 initialForce; // 動き始めにかける力
    [SerializeField] protected float reboundForce;   // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる
    // 一定速度で移動する場合の設定
    [SerializeField] protected Vector2 moveSpeed;    // 移動速度
    [SerializeField] protected float moveLength;     // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる
    [SerializeField] protected bool isTurn;          // 

    // オブジェクトが動く設定にした場合に使用する
    // これらは後々削除する
    private float speed = 1.0f;

    // この下は使用する
    private bool isMove = false;
    private float moveDistance = 0f;
    
    protected Rigidbody2D rb;

    /// <summary>
    /// オブジェクトを移動しないように設定
    /// </summary>
    private void SetNoMove()
    {
        // 動き出さない場合はその場に固定
        rb.bodyType = RigidbodyType2D.Static;
    }

    /// <summary>
    /// 動き始めのみ力を与える設定
    /// </summary>
    private void SetFirstForce()
    {
        isMove = true;
        rb.linearVelocity = initialForce;
        rb.sharedMaterial.bounciness = reboundForce; // 反発力を設定
    }

    /// <summary>
    /// 一定速度で移動する設定
    /// </summary>
    private void SetConstantSpeed()
    {
        isMove = true;
        moveDirection = moveSpeed.normalized;
        rb.linearVelocity = moveSpeed;
        rb.sharedMaterial.bounciness = 0f; // 反発力を設定
    }

    private Vector3 startPosition;
    private Vector2 moveDirection;
    
    void Start()
    {
        startPosition = transform.position; // 移動開始時の位置を記憶

        // まずは各コンポーネントの情報を取得
        rb = GetComponent<Rigidbody2D>();

        // 範囲内に入ったら動き出す場合はStart関数内でやることは特になし
        // movementTriggerType.HasFlag(MovementTrigger.AreaTrigger)

        // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        // autoStart = falseの場合、ゲーム開始時は止まった状態
        if (movementTriggerType.HasFlag(MovementTrigger.GameStart))
        {
            if (movementPatternType == MovementPattern.FirstForceOnly)
            {
                SetFirstForce();
            }
            else if (movementPatternType == MovementPattern.ConstantSpeed)
            {
                SetConstantSpeed();
            }
        }

        // 範囲内に入ったら動く設定でなければ、無駄なオブジェクトを取り除く
        if (movementTriggerType.HasFlag(MovementTrigger.AreaTrigger))
        {
            Destroy(transform.Find("MoveTriggerCollider").gameObject);
        }

        // ダメージ判定はPlayer側で行い、Trapタグで判定する
        // Trapタグをつけたつけないの混乱がないよう、スクリプト側で操作する
        if (isDamageObject)
        {
            transform.Find("DamageCollider").tag = "Trap";
        }
        else
        {
            transform.Find("DamageCollider").tag = "Untagged";
        }
    }

    private void FixedUpdate()
    {
        // 移動状態でなければ処理しない
        if (!isMove)
        {
            return;
        }
        
        float movedDistance = Vector3.Distance(startPosition, transform.position); // 現在の移動距離を計算
        if (movedDistance >= moveLength)
        {
            // 指定距離に達したら停止
            rb.linearVelocity = Vector2.zero;
            isMove = false;
        }
        else
        {
            // Kinematic な Rigidbody2D の位置更新
            Vector2 nextPosition = rb.position + (moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(nextPosition);
        }
    }
}
