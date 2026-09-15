using System.Drawing;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] BoxCollider2D canMoveCollider;
    [SerializeField] Vector2 speed = Vector2.zero;
    [SerializeField] bool canTriggerMove = false;

    private bool isMove = false; // 移動する／しないの制御

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMove = false;

        // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        // canTriggerMove = falseの場合は止まったまま
        if (!canTriggerMove)
        {
            // 動かないようにするために、始動用当たり判定を無効化する
            canMoveCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 動作フラグがオンの場合は指定方向に動く
        if (isMove)
        {
            transform.position += new Vector3(speed.x / 100, speed.y / 100, 0);
        }
    }

    // 当たり判定の範囲内にプレイヤーが侵入したら発動
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMove = true;
        }
    }
}
