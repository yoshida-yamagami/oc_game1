using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object), true)] // trueにすることで子クラスにも適用されます
public class ObjectEditor : Editor
{
    // 各グループの開閉状態を記憶する変数
    private static bool showMovementControl = true;
    private static bool showDamageSettings = true;
    private static bool showMovementSettings = true;

    // シリアライズされたプロパティ（元の変数と紐付けるためのもの）
    private SerializedProperty forceProp;
    private SerializedProperty bounceProp;
    private SerializedProperty damageProp;
    private SerializedProperty startOnAwakeProp;
    private SerializedProperty startOnTriggerProp;

    private void OnEnable()
    {
        // 各変数のプロパティを取得
        forceProp = serializedObject.FindProperty("動き始めにかける力");
        bounceProp = serializedObject.FindProperty("反発力");
        damageProp = serializedObject.FindProperty("プレイヤーに当たるとダメージ");
        startOnAwakeProp = serializedObject.FindProperty("ゲーム開始時に動き出す");
        startOnTriggerProp = serializedObject.FindProperty("範囲内に入ったら動き出す");
    }

    public override void OnInspectorGUI()
    {
        // データを最新の状態に更新
        serializedObject.Update();

        // --------------------------------------------------
        // グループ1: 動き方の制御
        // --------------------------------------------------
        showMovementControl = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementControl, "動き方の制御");
        if (showMovementControl)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(forceProp);
            EditorGUILayout.PropertyField(bounceProp);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //EditorGUILayout.Space(); // 少し隙間を空ける

        // --------------------------------------------------
        // グループ2: ダメージのオンオフ
        // --------------------------------------------------
        showDamageSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDamageSettings, "ダメージのオンオフ");
        if (showDamageSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(damageProp);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //EditorGUILayout.Space();

        // --------------------------------------------------
        // グループ3: 動き方の設定
        // --------------------------------------------------
        showMovementSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementSettings, "動き方の設定");
        if (showMovementSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(startOnAwakeProp);
            EditorGUILayout.PropertyField(startOnTriggerProp);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();
    }
}