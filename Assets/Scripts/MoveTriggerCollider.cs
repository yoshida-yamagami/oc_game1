using UnityEngine;

public class MoveTriggerCollider : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが当たり判定内に侵入したら発動
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<BaseObject>().SetMove();
        }
    }
}
