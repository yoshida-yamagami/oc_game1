using UnityEditor;
using UnityEngine;

public class Ball : BaseObject
{
    // サイズの設定
    [SerializeField] protected float ballSize = 1.0f;
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

        ballSize = ballSize < 0f ? 0f : ballSize;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.drawMode == SpriteDrawMode.Simple)
        {
            transform.localScale = new Vector3(ballSize, ballSize, 1f);
        }
    }
}
