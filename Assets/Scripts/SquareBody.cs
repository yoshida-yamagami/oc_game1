using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class SquareBody : MonoBehaviour
{
    [SerializeField] BoxCollider2D canMoveCollider;

    private GameObject parent;
    private Square2 parentScript;

    private Vector2 speed; // 移動方向と速度の設定
    private Vector2 size; // サイズの設定
    private bool isMove = false; // 移動する／しないの制御
    private bool autoStart = false; // ゲーム開始直後に動き出すか否か（trueなら動く、falseなら動かない）
    private bool canTriggerMove = false; // 始動用当たり判定にプレイヤーが入ってきたら動き出すか否か（trueなら動く、falseなら動かない）

    void Start()
    {
        // 親オブジェクトを取得
        parent = transform.parent.gameObject;
        parentScript = parent.GetComponent<Square2>();

        // 親から与えられる情報を反映
        this.speed = parentScript.GetSpeed();
        this.size = parentScript.GetSize();
        UpdateSize();

        // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        // autoStart = falseの場合、ゲーム開始時は止まった状態
        if (autoStart)
        {
            isMove = true;
        }

        // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        // canTriggerMove = falseの場合、その球は止まったまま
        if (!canTriggerMove)
        {
            // 動かないようにするために、始動用当たり判定を無効化する
            canMoveCollider.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // 動作フラグがオンの場合は指定方向に動く
        if (isMove)
        {
            parent.transform.position += new Vector3(speed.x / 100, speed.y / 100, 0);
        }
    }

    /// <summary>
    /// 当たり判定範囲内にプレイヤーが侵入したら効果発動
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーが誰かの子である可能性を排除する
            collision.gameObject.transform.SetParent(null);

            // 親方にプレイヤーを追加することで一緒に動くようにする
            collision.gameObject.transform.SetParent(parent.transform, false);

            // 親方との位置関係を調整
            collision.gameObject.transform.position -= parent.transform.position;
        }
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 1. 親が存在するか？
            // 2. その親が「アクティブに変化中（消えかかっている）」ではないか？
            // 3. 自身も消えかかっていないか？
            if (parent != null && parent.gameObject.activeInHierarchy)
            {
                collision.gameObject.transform.SetParent(null);
            }
        }
    }

    public void SetSpeed(Vector2 speed)
    {
        this.speed = speed;
    }

    public void SetSize(Vector2 size)
    {
        this.size = size;
    }

    public void SetAutoStart(bool autoStart)
    {
        this.autoStart = autoStart;
    }

    public void SetCanTriggerMove(bool canTriggerMove)
    {
        this.canTriggerMove = canTriggerMove;
    }

    private void UpdateSize()
    {
        transform.localScale = new Vector3(size.x, size.y, 1);
    }
}
