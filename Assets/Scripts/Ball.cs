using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //    // ボールの跳ねる勢いを設定
        //    rb.sharedMaterial.bounciness = 反発力;

        //    // autoStart = trueの場合、ゲーム開始直後にすぐ動き出す
        //    // autoStart = falseの場合、ゲーム開始時は止まった状態
        //    if (ゲーム開始時に動き出す)
        //    {
        //        rb.linearVelocity = 動き始めにかける力;
        //    }
        //    else
        //    {
        //        rb.bodyType = RigidbodyType2D.Static;
        //    }

        //    // canTriggerMove = trueの場合、当たり判定内にプレイヤーが入ったら動き出す
        //    // canTriggerMove = falseの場合、その球は止まったまま
        //    if (!範囲内に入ったら動き出す)
        //    {
        //        // 動かないようにするためにBoxCollider2Dを無効化する
        //        GetComponent<BoxCollider2D>().enabled = false;
        //    }

        //    if (プレイヤーに当たるとダメージ)
        //    {
        //        // オブジェクトのタグをTrapに変える
        //        gameObject.tag = "Trap";
        //    }
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
         //   rb.linearVelocity = initialForce;
        }
    }
}
