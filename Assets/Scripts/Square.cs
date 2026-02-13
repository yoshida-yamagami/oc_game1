using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
    /// <summary>
    /// 移動するスピード。□はスピードにしたがって一定方向に進む
    /// </summary>
    [SerializeField] Vector2 speed = Vector2.zero;
    [SerializeField] bool autoStart = false; // ゲーム開始直後に動き出すか否か（trueなら動く、falseなら動かない）
    [SerializeField] bool canTriggerMove = true; // 始動用当たり判定にプレイヤーが入ってきたら動き出すか否か（trueなら動く、falseなら動かない）
    [SerializeField] BoxCollider2D canMoveTrigger;

    private bool isMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            canMoveTrigger.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 動作フラグがオンの場合は指定方向に動く
        if (isMove)
        {
            gameObject.transform.position += new Vector3(speed.x / 100, speed.y / 100, 0);
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position += new Vector3(speed.x / 100, speed.y / 100, 0);
        }
    }
}
