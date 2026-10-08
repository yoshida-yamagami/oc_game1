using System.Drawing;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // オブジェクトを構成するパーツを表示しない
    private void OnValidate()
    {
        // 子オブジェクトをすべてループしてヒエラルキーから隠す
        foreach (Transform child in transform)
        {
            // ヒエラルキーに表示しない設定
            if (child.gameObject.name == "Parts")
            {
                child.gameObject.hideFlags = HideFlags.HideInHierarchy;
                //child.gameObject.hideFlags = HideFlags.None; 表示したい場合はこの行を使用
            }
        }
    }
}
