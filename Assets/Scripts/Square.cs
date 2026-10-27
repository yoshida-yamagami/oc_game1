using UnityEditor;
using UnityEngine;

public class Square : BaseObject
{
    // サイズ設定
    [SerializeField] protected Vector2 squareSize = new Vector2(1f, 1f);

    // いろんな形のオブジェクトに、各個特有の処理が必要なさそうなら
    // いっそBaseObjectのみ生かす形でOK。
    // いったん形ごとのスクリプトを用意する形で進める。
    // なお、四角は特有の処理は必要なさそう。

    // インスペクター上で値が変更されたときに自動実行されるメソッド（エディタ専用）
    private void OnValidate()
    {
#if UNITY_EDITOR
        // OnValidate 内で直接呼ばず、エディタの更新処理が終わった直後に実行する
        EditorApplication.delayCall -= UpdateColliderSizes; // 重複登録の防止
        EditorApplication.delayCall += UpdateColliderSizes;
#endif
    }

    /// <summary>
    /// SpriteRendererのサイズに合わせて子オブジェクトのコライダーサイズを同期する
    /// </summary>
    public override void UpdateColliderSizes()
    {
        // オブジェクトが破棄されている、または非アクティブな場合は処理を中断
        if (this == null) return;

        if (squareSize.x < 0f || squareSize.y < 0f)
        {
            squareSize.x = squareSize.x < 0f ? 0f : squareSize.x;
            squareSize.y = squareSize.y < 0f ? 0f : squareSize.y;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.drawMode == SpriteDrawMode.Sliced)
        {
            sr.size = squareSize; // ★SpriteRenderer の size に反映
        }

        Vector2 newSize = sr.size; // SpriteRendererのWidth, Heightを取得

        // 2. 子オブジェクト（GroundCollider や DamageCollider）の BoxCollider2D を同期
        // 地面判定コライダーの同期
        Transform groundChild = transform.Find("GroundCollider");
        if (groundChild != null)
        {
            BoxCollider2D groundCollider = groundChild.GetComponent<BoxCollider2D>();
            if (groundCollider != null)
            {
                groundCollider.size = newSize;
            }
        }

        // ダメージ判定コライダーの同期
        Transform damageChild = transform.Find("DamageCollider");
        if (damageChild != null)
        {
            BoxCollider2D damageCollider = damageChild.GetComponent<BoxCollider2D>();
            if (damageCollider != null)
            {
                damageCollider.size = newSize;
            }
        }

        base.UpdateColliderSizes();
    }
}
