using UnityEngine;

public class Square2 : Object
{
    [SerializeField] GameObject body;
    [SerializeField] SquareBody bodyScript;
    [SerializeField] Vector2 speed = Vector2.zero;
    [SerializeField] Vector2 size = new Vector2(1, 1);
    [SerializeField] bool autoStart = false; // ゲーム開始直後に動き出すか否か（trueなら動く、falseなら動かない）
    [SerializeField] bool canTriggerMove = true; // 始動用当たり判定にプレイヤーが入ってきたら動き出すか否か（trueなら動く、falseなら動かない）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 四角形オブジェクトの本体を認識する
        body = transform.Find("SquareBody").gameObject;
        bodyScript = body.GetComponent<SquareBody>();
    }

    // インスペクターで値を変えた時に呼ばれる
    private void OnValidate()
    {
        // 変更した値に基づいて値を更新
        body.gameObject.transform.localScale = new Vector3(size.x, size.y, 1);
        bodyScript.SetSpeed(speed);
        bodyScript.SetAutoStart(autoStart);
        bodyScript.SetCanTriggerMove(canTriggerMove);
    }

    public Vector2 GetSpeed()
    {
        return this.speed;
    }

    public Vector2 GetSize()
    {
        return this.size;
    }
}
