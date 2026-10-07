using UnityEditor;
using UnityEngine;

/// <summary>
/// ゲーム内オブジェクトの祖
/// </summary>
public class Object : MonoBehaviour
{
    /// <summary>
    /// 全オブジェクト共通設定
    /// </summary>
    [Tooltip("\n力をかける方向と大きさを設定\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く\n")]
    [SerializeField] protected Vector2 動き始めにかける力;
    [Tooltip("\n何かにぶつかったときの動きに影響\n0：反発せず\n0～1：ゆるく反発\n1：ぶつかった力と同じだけ反発\n1以上：ぶつかった力より強く反発\n")]
    [SerializeField] protected float 反発力; // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる
    [Tooltip("\nチェックをつけるとプレイヤーがダメージを受けるようになる\n")]
    [SerializeField] protected bool プレイヤーに当たるとダメージ = false;
    [Tooltip("\nチェックをつけるとゲーム開始時に動き出す\n")]
    [SerializeField] protected bool ゲーム開始時に動き出す = false; // ゲーム開始直後に球が動き出すか否か（trueなら動く、falseなら動かない）
    [Tooltip("\nチェックをつけるとプレイヤーが某Colliderの範囲内に入った時に動き出す\n")]
    [SerializeField] protected bool 範囲内に入ったら動き出す = true; // 球の始動用当たり判定にプレイヤーが入ってきたら球が動き出すか否か（trueなら動く、falseなら動かない）

    // ややこしくなるので、四角なら四角、円なら円の当たり判定を使用する前提にする

    [Tooltip("\nチェックをつけるとプレイヤーが某Colliderの範囲内に入った時に動き出す\n")]
    [SerializeField] float speed = 1.0f;
    [SerializeField] bool isMove = false;
    
    protected Rigidbody2D rb;

    //private BoxCollider2D groundCollider; // 床判定用の当たり判定
    //private BoxCollider2D attackCollider; // 攻撃判定用の当たり判定
    //private BoxCollider2D moveTriggerCollider; // 範囲内に入ったら動き出す当たり判定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // まずは各コンポーネントの情報を取得
        rb = GetComponent<Rigidbody2D>();
        //groundCollider = transform.Find("GroundCollider").GetComponent<BoxCollider2D>();
        //attackCollider = transform.Find("AttackCollider").GetComponent<BoxCollider2D>();
        //moveTriggerCollider = transform.Find("MoveTriggerCollider").GetComponent<BoxCollider2D>();

        isMove = true;

        // 反発力を設定
        rb.sharedMaterial.bounciness = 反発力;

        // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        // autoStart = falseの場合、ゲーム開始時は止まった状態
        if (ゲーム開始時に動き出す)
        {
            rb.linearVelocity = 動き始めにかける力;
        }
        else
        {
            // 動き出さない場合はその場に固定
            rb.bodyType = RigidbodyType2D.Static;
        }

        // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        // canTriggerMove = falseの場合、その球は止まったまま
        if (!範囲内に入ったら動き出す)
        {
            // 動かないようにするためにBoxCollider2Dを無効化する
            //moveTriggerCollider.enabled = false;
            Destroy(transform.Find("MoveTriggerCollider").gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (プレイヤーに当たるとダメージ)
        {
            // 当たり判定用のオブジェクトのタグをTrapに変える
            transform.Find("AttackCollider").tag = "Trap";
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // roleで地面判定なのかダメージ判定なのかを見極める
    public void OnChildCollisionEnter(string role, Collision2D collision)
    {
        if (role == "Ground")
        {
            //// プレイヤーが誰かの子である可能性を排除する
            //collision.gameObject.transform.SetParent(null);

            //// プレイヤーを子にすることで、Ground側の動きと同期する
            //collision.gameObject.transform.SetParent(transform, false);

            //// 位置関係を調整
            //collision.gameObject.transform.position -= transform.position;


            // プレイヤーを子にすることで、Ground側の動きと同期する
            collision.gameObject.transform.SetParent(transform);
        }
    }

    // roleで地面判定なのかダメージ判定なのかを見極める
    public void OnChildCollisionExit(string role, Collision2D collision)
    {
        // 地面から離れた時に、プレイヤーとの同期を解除する
        if (role == "Ground" && gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.SetParent(null);
        }

        // 新しいスクリプトを作って、当たり判定はそちらに任せる
    }
}
