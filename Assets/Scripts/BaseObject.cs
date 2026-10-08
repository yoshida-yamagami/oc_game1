using UnityEditor;
using UnityEngine;

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

    /// <summary>
    /// このスクリプトをアタッチする全オブジェクト共通設定
    /// </summary>
    [SerializeField] protected bool isDamageObject = false; // プレイヤーに触れた時にダメージを受けるかどうか
    [SerializeField] protected MovementTrigger movementTriggerType; // 動き出すタイミングの設定
    [SerializeField] protected Vector2 initialForce; // 動き始めにかける力
    [SerializeField] protected float reboundForce; // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる

    // オブジェクトが動く設定にした場合に使用する
    private float speed = 1.0f;
    private bool isMove = false;
    
    protected Rigidbody2D rb;

    void Start()
    {
        // まずは各コンポーネントの情報を取得
        rb = GetComponent<Rigidbody2D>();
        isMove = true;

        // 反発力を設定
        rb.sharedMaterial.bounciness = reboundForce;

        // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        // autoStart = falseの場合、ゲーム開始時は止まった状態
        //if (ゲーム開始時に動き出す)
        //{
        //    rb.linearVelocity = 動き始めにかける力;
        //}
        //else
        //{
        //    // 動き出さない場合はその場に固定
        //    rb.bodyType = RigidbodyType2D.Static;
        //}

        // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        // canTriggerMove = falseの場合、その球は止まったまま
        //if (!範囲内に入ったら動き出す)
        //{
        //    // 動かないようにするためにBoxCollider2Dを無効化する
        //    //moveTriggerCollider.enabled = false;
        //    Destroy(transform.Find("MoveTriggerCollider").gameObject);
        //    //GetComponent<CircleCollider2D>().enabled = false;
        //}

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
}
