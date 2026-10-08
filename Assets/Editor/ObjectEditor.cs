using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseObject), true)]
public class ObjectEditor : Editor
{
    // オブジェクトの動作に関する選択肢の内容を設定
    private readonly string[] triggerNames = new string[]
    {
        "動かない",
        "ゲーム開始時に動き出す",
        "範囲内に入ったら動き出す"
    };

    // 各グループの開閉状態を記憶する変数
    private static bool showDamageSettings = true;
    private static bool showMovementSettings = true;
    private static bool showMovementControl = true;

    // シリアライズされたプロパティ（元の変数と紐付けるためのもの）
    private SerializedProperty damageProp;
    // 【修正】2つのboolプロパティを削除し、enum用のプロパティを1つ定義
    private SerializedProperty movementTriggerProp;

    //private SerializedProperty startOnAwakeProp;
    //private SerializedProperty startOnTriggerProp;
    private SerializedProperty forceProp;
    private SerializedProperty bounceProp;

    private void OnEnable()
    {
        // 各変数のプロパティを取得
        damageProp = serializedObject.FindProperty("isDamageObject");
        // 【修正】新しく作った enum 変数の名前を指定（例: "movementTriggerType"）
        movementTriggerProp = serializedObject.FindProperty("movementTriggerType");

        //startOnAwakeProp = serializedObject.FindProperty("ゲーム開始時に動き出す");
        //startOnTriggerProp = serializedObject.FindProperty("範囲内に入ったら動き出す");
        forceProp = serializedObject.FindProperty("initialForce");
        bounceProp = serializedObject.FindProperty("reboundForce");
    }

    public override void OnInspectorGUI()
    {
        // データを最新の状態に更新
        serializedObject.Update();

        // --------------------------------------------------
        // グループ: ダメージのオンオフ
        // --------------------------------------------------
        showDamageSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDamageSettings, "ダメージのオンオフ");
        if (showDamageSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(damageProp, new GUIContent("プレイヤーに当たるとダメージ", "プレイヤーに当たるとダメージ\n\nチェックをつけると、このオブジェクトにプレイヤーがぶつかるとダメージを受けるようになる"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        //EditorGUILayout.Space(); // 少し隙間を空ける

        // --------------------------------------------------
        // グループ: 動き方の設定
        // --------------------------------------------------
        showMovementSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementSettings, "動き方の設定");
        if (showMovementSettings)
        {
            EditorGUI.indentLevel++;
            // プロパティを表示するだけで、自動的にセレクトボックス（プルダウン）になります
            // 第2引数でインスペクター上の表示名を日本語にカスタムできます
            //EditorGUILayout.PropertyField(movementTriggerProp, new GUIContent("動き出すタイミング"));
            // 選択肢を表示
            movementTriggerProp.enumValueIndex = EditorGUILayout.Popup(
                new GUIContent("動き出すタイミング", "動き出すタイミング\n\nオブジェクトが動くかどうか、\n動くならどのタイミングで動くのかを設定"), // 左側のラベル
                movementTriggerProp.enumValueIndex,   // 現在の選択値（インデックス）
                triggerNames                          // 表示する日本語の配列
            );
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        //EditorGUILayout.Space();

        // --------------------------------------------------
        // グループ: 動き方の制御
        // --------------------------------------------------
        showMovementControl = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementControl, "動き方の制御");
        if (showMovementControl)
        {
            EditorGUI.indentLevel++;

            // 動く設定にした場合true
            bool isMovingEnabled = movementTriggerProp.enumValueIndex != 0;

            // 動く設定にした場合入力可能、それ以外は入力不可
            EditorGUI.BeginDisabledGroup(!isMovingEnabled);
            EditorGUILayout.PropertyField(forceProp, new GUIContent("動き始めにかける力", "動き始めにかける力\n\n力をかける方向と大きさを設定\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く"));
            EditorGUILayout.PropertyField(bounceProp, new GUIContent("反発力", "反発力\n\n何かにぶつかったときの動きに影響\n0：反発せず\n0～1：ぶつかった力より弱く反発\n1：ぶつかった力と同じだけ反発\n1以上：ぶつかった力より強く反発"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定して適用
        serializedObject.ApplyModifiedProperties();
    }
}