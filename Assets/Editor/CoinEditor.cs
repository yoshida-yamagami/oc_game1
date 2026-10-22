using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Coin), true)]
public class CoinEditor : ObjectEditor
{
    // グループの開閉状態を記憶する変数
    private static bool showCoinSettings = true;

    // コインのオブジェクトかどうかの設定
    private SerializedProperty coinProp;

    protected override void OnEnable()
    {
        // プロパティを取得
        coinProp = serializedObject.FindProperty("isCoinObject");

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
        showCoinSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showCoinSettings, "Coin固有設定");
        if (showCoinSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(coinProp, new GUIContent("コインとして取得可能", "コインとして取得可能\n\nチェックをつけると、このオブジェクトにプレイヤーが触れるとコインとして獲得できるようになる"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();

        // 親クラスの処理を実行
        base.OnInspectorGUI();
    }
}