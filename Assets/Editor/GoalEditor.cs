using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Goal), true)]
public class GoalEditor : ObjectEditor
{
    // グループの開閉状態を記憶する変数
    private static bool showGoalSettings = true;

    // ゴールのオブジェクトかどうかの設定
    private SerializedProperty goalProp;

    protected override void OnEnable()
    {
        // プロパティを取得
        goalProp = serializedObject.FindProperty("isGoalObject");

        // 親クラスの処理を実行
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        // データを最新の状態に更新
        serializedObject.Update();

        // -----------------------
        // グループ: ダメージのオンオフ
        // --------------------------------------------------
        showGoalSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showGoalSettings, "Goal固有設定");
        if (showGoalSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(goalProp, new GUIContent("ゴール可能かどうか", "ゴール可能かどうか\n\nチェックをつけると、このオブジェクトにプレイヤーが触れるとゴールできるようになる。\n\nシーン上に2つ以上ゴールがあっても問題ないが、1つもゴールがない場合はエラーになる。"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();

        // 親クラスの処理を実行
        base.OnInspectorGUI();
    }
}
