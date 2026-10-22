using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ball), true)]
public class BallEditor : ObjectEditor
{
    // グループの開閉状態を記憶する変数
    private static bool showBallSettings = true;

    // コインのオブジェクトかどうかの設定
    private SerializedProperty ballSizeProp;

    protected override void OnEnable()
    {
        // プロパティを取得
        ballSizeProp = serializedObject.FindProperty("ballSize");

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
        showBallSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showBallSettings, "Ball固有設定");
        if (showBallSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(ballSizeProp, new GUIContent("オブジェクトサイズ", "オブジェクトサイズ\n\nボールの場合、半径の設定ができる"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();

        // 親クラスの処理を実行
        base.OnInspectorGUI();
    }
}
