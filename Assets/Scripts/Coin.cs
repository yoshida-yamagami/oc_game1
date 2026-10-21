using UnityEngine;

public class Coin : BaseObject
{
    // コインとして取得できる／できないを設定する必要ありけり
    [SerializeField] protected bool isCoinObject = true; // コインとして取得できるかどうか

    protected override void Start()
    {
        if (isCoinObject)
        {
            gameObject.tag = "Coin";
        }
        else
        {
            gameObject.tag = "Untagged";
        }

        base.Start();
    }
}
