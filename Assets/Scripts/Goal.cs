using UnityEngine;

public class Goal : BaseObject
{
    // 触れたらゴールできる／できないの設定
    [SerializeField] protected bool isGoalObject = true;

    // オブジェクトを構成するパーツを表示しない
    private void OnValidate()
    {
        // 子オブジェクトをすべてループしてヒエラルキーから隠す
        foreach (Transform child in transform)
        {
            // ヒエラルキーに表示しない設定
            if (child.gameObject.name == "Aura")
            {
                child.gameObject.hideFlags = HideFlags.HideInHierarchy;
                //child.gameObject.hideFlags = HideFlags.None; //表示したい場合はこの行を使用
            }
        }
    }

    protected override void Start()
    {
        if (isGoalObject)
        {
            gameObject.tag = "Goal";
        }
        else
        {
            gameObject.tag = "Untagged";
        }

        base.Start();
    }
}
