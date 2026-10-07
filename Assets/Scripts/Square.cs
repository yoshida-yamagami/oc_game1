using UnityEngine;

public class Square : Object
{
    // 他のスクリプトでやっているように、
    // わざわざ親を用意しないとプレイヤーの動きを同期できないかテストする

    // やりたいこと：親を作らないでも、
    // 自動で動く四角形の上にプレイヤーを乗せ、
    // かつプレイヤーが動かなくても四角形の動きに合わせてプレイヤーが動くようにしたい

    //// 自動で動くスピード
    //[SerializeField] float speed = 1.0f;
    //[SerializeField] bool isMove = false;

    // 有効にする場合、親スクリプトのStartを明示的に呼ぶ必要あり
    //void Start()
    //{
    //    //isMove = true;
    //}

    void FixedUpdate()
    {
        //// 動作フラグがオンの場合は指定方向に動く
        //if (isMove)
        //{
        //    gameObject.transform.position += new Vector3(speed / 100, 0, 0);
        //}
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        // プレイヤーが誰かの子である可能性を排除する
    //        collision.gameObject.transform.SetParent(null);

    //        // 親方にプレイヤーを追加することで一緒に動くようにする
    //        collision.gameObject.transform.SetParent(transform, false);

    //        // 親方との位置関係を調整
    //        collision.gameObject.transform.position -= transform.position;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        // 1. 親が存在するか？
    //        // 2. その親が「アクティブに変化中（消えかかっている）」ではないか？
    //        // 3. 自身も消えかかっていないか？
    //        if (gameObject.activeInHierarchy)
    //        {
    //            collision.gameObject.transform.SetParent(null);
    //        }
    //    }
    //}
}
