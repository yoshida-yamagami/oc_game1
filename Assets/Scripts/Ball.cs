using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Vector2 power;
    [SerializeField] float bounciness; // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる
    [SerializeField] bool autoStart = false; // ゲーム開始直後に球が動き出すか否か（trueなら動く、falseなら動かない）
    [SerializeField] bool canTriggerMove = true; // 球の始動用当たり判定にプレイヤーが入ってきたら球が動き出すか否か（trueなら動く、falseなら動かない）

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ボールの跳ねる勢いを設定
        rb.sharedMaterial.bounciness = bounciness;

        // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        // autoStart = falseの場合、ゲーム開始時は止まった状態
        if (autoStart)
        {
            rb.linearVelocity = power;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        // canTriggerMove = falseの場合、その球は止まったまま
        if (!canTriggerMove)
        {
            // 動かないようにするためにBoxCollider2Dを無効化する
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 範囲内にプレイヤーが入った場合
        if (collision.tag == "Player")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            // 指定方向に力をかける
            rb.linearVelocity = power;
        }
    }
}
