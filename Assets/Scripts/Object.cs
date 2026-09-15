using UnityEditor;
using UnityEngine;

/// <summary>
/// ゲーム内オブジェクトの祖
/// </summary>
public class Object : MonoBehaviour
{
    /// <summary>
    /// 全オブジェクト共通設定
    /// </summary>
    [Tooltip("\n力をかける方向と大きさを設定\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く\n")]
    [SerializeField] protected Vector2 動き始めにかける力;
    [Tooltip("\n何かにぶつかったときの動きに影響\n0：反発せず\n0～1：ゆるく反発\n1：ぶつかった力と同じだけ反発\n1以上：ぶつかった力より強く反発\n")]
    [SerializeField] protected float 反発力; // 球が物体とぶつかったときに跳ねる勢い（0 = 跳ねない、0～1 = 跳ねる。徐々に跳ねる力が弱くなる 1 = 同じ高さまで跳ねる 1～ = 徐々に跳ねる高さが高くなる
    [SerializeField] protected bool プレイヤーに当たるとダメージ = false;
    [SerializeField] protected bool ゲーム開始時に動き出す = false; // ゲーム開始直後に球が動き出すか否か（trueなら動く、falseなら動かない）
    [SerializeField] protected bool 範囲内に入ったら動き出す = true; // 球の始動用当たり判定にプレイヤーが入ってきたら球が動き出すか否か（trueなら動く、falseなら動かない）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
