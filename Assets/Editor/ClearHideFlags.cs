// Editor/ClearHideFlags.cs
using UnityEditor;
using UnityEngine;

public class ClearHideFlags
{
    [MenuItem("Tools/Clear All HideFlags")]
    public static void ClearAll()
    {
        // シーン内のすべての GameObject を取得して HideFlags を解除する
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true);
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            if (go.hideFlags != HideFlags.None)
            {
                Undo.RegisterCompleteObjectUndo(go, "Clear HideFlags");
                go.hideFlags = HideFlags.None;
                EditorUtility.SetDirty(go);
                count++;
            }
        }

        // シーンを更新して変更を反映
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log($"[ClearHideFlags] {count} 個のオブジェクトの HideFlags をリセットしました。");
    }
}