using UnityEngine;

public class Square : BaseObject
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
}
