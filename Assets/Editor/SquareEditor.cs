using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Square), true)]
public class SquareEditor : ObjectEditor
{
    // グループの開閉状態を記憶する変数
    private static bool showSquareSettings = true;

    private SerializedProperty squareSizeProp;

    protected override void OnEnable()
    {
        // プロパティを取得
        squareSizeProp = serializedObject.FindProperty("squareSize");

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
        showSquareSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSquareSettings, "Square固有設定");
        if (showSquareSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(squareSizeProp, new GUIContent("オブジェクトサイズ", "オブジェクトサイズ\n\n四角形の場合、幅・高さの設定ができる\n\nX:幅, Y:高さ"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();

        // 親クラスの処理を実行
        base.OnInspectorGUI();
    }
}