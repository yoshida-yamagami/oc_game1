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

    // オブジェクトの動作に関する選択肢の内容を設定
    private readonly string[] patternNames = new string[]
    {
        "動き出しのみ力をかける",
        "一定速度で動く"
    };

    // 各グループの開閉状態を記憶する変数
    private static bool showDamageSettings = true;
    private static bool showMovementSettings = true;
    private static bool showMovementPatternAControl = true; // 動き出しのみ力をかける場合の設定項目
    private static bool showMovementPatternBControl = true; // 一定速度で動く場合の設定項目

    // シリアライズされたプロパティ（元の変数と紐付けるためのもの）
    private SerializedProperty damageProp;              // ダメージを受けるオブジェクトかどうかの設定
    private SerializedProperty movementTriggerProp;     // 動き出すタイミング設定
    protected SerializedProperty triggerAreaSizeProp;     // トリガーの範囲設定
    protected SerializedProperty triggerAreaPositionProp; // トリガーの位置設定
    private SerializedProperty movementPatternProp;     // 動き方の設定
    private SerializedProperty forceProp;               // 初速度設定
    private SerializedProperty bounceProp;              // 反発力設定
    private SerializedProperty moveSpeedProp;           // 移動速度設定
    private SerializedProperty moveLengthProp;          // 移動距離設定
    private SerializedProperty turnProp;                // 折り返し設定

    protected virtual void OnEnable()
    {
        // 各変数のプロパティを取得
        damageProp = serializedObject.FindProperty("isDamageObject");
        // 【修正】新しく作った enum 変数の名前を指定（例: "movementTriggerType"）
        movementTriggerProp = serializedObject.FindProperty("movementTriggerType");
        triggerAreaPositionProp = serializedObject.FindProperty("triggerAreaPosition");
        triggerAreaSizeProp = serializedObject.FindProperty("triggerAreaSize");
        movementPatternProp = serializedObject.FindProperty("movementPatternType");
        forceProp = serializedObject.FindProperty("initialForce");
        bounceProp = serializedObject.FindProperty("bounciness");
        moveSpeedProp = serializedObject.FindProperty("moveSpeed");
        moveLengthProp = serializedObject.FindProperty("moveLength");
        turnProp = serializedObject.FindProperty("isTurn");
    }

    /// <summary>
    /// トリガー範囲がBoxCollider使用の場合
    /// CircleCollider使用の場合は子スクリプト側で実装
    /// </summary>
    protected virtual void SetAreaTriggerProp()
    {
        EditorGUILayout.PropertyField(triggerAreaPositionProp, new GUIContent("範囲位置", "範囲位置\n\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く"));
        EditorGUILayout.PropertyField(triggerAreaSizeProp, new GUIContent("範囲指定", "範囲指定\n\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く"));
    }

    public override void OnInspectorGUI()
    {
        // データを最新の状態に更新
        serializedObject.Update();

        bool isMovingEnabled = movementTriggerProp.enumValueIndex != 0; // 動くオブジェクトの場合true
        bool isMovingAreaTrigger = movementTriggerProp.enumValueIndex == 2; // 範囲内に入ったら動く場合true
        bool isMovingConstant = movementPatternProp.enumValueIndex == 1; // 一定速度で動く場合true

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
            // 選択肢を表示
            movementTriggerProp.enumValueIndex = EditorGUILayout.Popup(
                new GUIContent("動き出すタイミング", "動き出すタイミング\n\nオブジェクトが動くかどうか、\n動くならどのタイミングで動くのかを設定"), // 左側のラベル
                movementTriggerProp.enumValueIndex,   // 現在の選択値（インデックス）
                triggerNames                          // 表示する日本語の配列
            );

            // 範囲内に入ったら動き出す設定にした場合、ここで範囲指定をできるようにしたい
            if (isMovingAreaTrigger)
            {
                SetAreaTriggerProp();
            }

            // 選択肢を表示
            // 動く設定にした場合入力可能、それ以外は入力不可
            EditorGUI.BeginDisabledGroup(!isMovingEnabled);
            movementPatternProp.enumValueIndex = EditorGUILayout.Popup(
                new GUIContent("動き方", "動き方\n\n一定スピードで動くか、オブジェクトが動くかどうか、\n動くならどのタイミングで動くのかを設定"), // 左側のラベル
                movementPatternProp.enumValueIndex,   // 現在の選択値（インデックス）
                patternNames                          // 表示する日本語の配列
            );
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        //EditorGUILayout.Space();

        // --------------------------------------------------
        // グループ: 動き方A（動き出しのみ力をかける）の制御
        // --------------------------------------------------
        EditorGUI.BeginDisabledGroup(!isMovingEnabled || (isMovingEnabled && isMovingConstant));
        showMovementPatternAControl = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementPatternAControl, "動き出しのみ力を入れる場合の制御");
        if (showMovementPatternAControl)
        {
            EditorGUI.indentLevel++;

            // 動く設定かつ動き始めのみ力をかける場合のみ入力可能、それ以外は入力不可
            EditorGUILayout.PropertyField(forceProp, new GUIContent("動き始めにかける力", "動き始めにかける力\n\n力をかける方向と大きさを設定\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く"));
            EditorGUILayout.PropertyField(bounceProp, new GUIContent("反発力", "反発力\n\n何かにぶつかったときの動きに影響\n0：反発せず\n0～1：ぶつかった力より弱く反発\n1：ぶつかった力と同じだけ反発\n1以上：ぶつかった力より強く反発"));

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();

        // --------------------------------------------------
        // グループ: 動き方B（一定速度で動く）の制御
        // --------------------------------------------------
        EditorGUI.BeginDisabledGroup(!isMovingEnabled || (isMovingEnabled && !isMovingConstant));
        showMovementPatternBControl = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementPatternBControl, "一定速度で動く場合の制御");
        if (showMovementPatternBControl)
        {
            EditorGUI.indentLevel++;

            // 動く設定かつ一定速度で動く場合のみ入力可能、それ以外は入力不可
            EditorGUILayout.PropertyField(moveSpeedProp, new GUIContent("移動速度", "移動速度\n\n数値が大きいほど早く動く\nx：正なら右、負なら左に動く\ny：正なら上、負なら下に動く"));
            EditorGUILayout.PropertyField(moveLengthProp, new GUIContent("移動距離", "移動距離\n\n数値が大きいほど遠くまで動く"));
            EditorGUILayout.PropertyField(turnProp, new GUIContent("折り返しする", "折り返しする\n\nチェックを入れると、設定された移動距離を移動し終わった後元いた場所に戻る\nチェックがない場合、移動し終わった場所に留まる"));

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();

        // 変更を確定させ、もし値に変更があれば true が返る
        if (serializedObject.ApplyModifiedProperties())
        {
            // ターゲットとなっている BaseObject のインスタンスを取得して即座にサイズ更新を呼び出す
            BaseObject baseObj = (BaseObject)target;
            if (baseObj != null)
            {
                baseObj.UpdateColliderSizes();
                // Sceneビューの描画を即座に更新する
                EditorUtility.SetDirty(baseObj);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnSceneGUI()
    {
        BaseObject baseObj = (BaseObject)target;
        if (baseObj == null) return;

        // 自身およびすべての子オブジェクトから Collider2D を取得
        Collider2D[] colliders = baseObj.GetComponentsInChildren<Collider2D>();

        // Unity標準のコライダーに近い緑色で描画
        Handles.color = new Color(0.3f, 1.0f, 0.4f, 0.8f);

        foreach (var col in colliders)
        {
            // 1. PolygonCollider2D（三角形などの複雑な形状）
            if (col is PolygonCollider2D poly)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(poly.transform.position, poly.transform.rotation, poly.transform.lossyScale);
                using (new Handles.DrawingScope(matrix))
                {
                    Vector2[] points = poly.points;
                    for (int i = 0; i < points.Length; i++)
                    {
                        Vector3 p1 = (Vector3)points[i] + (Vector3)poly.offset;
                        Vector3 p2 = (Vector3)points[(i + 1) % points.Length] + (Vector3)poly.offset;
                        Handles.DrawLine(p1, p2);
                    }
                }
            }
            // 2. BoxCollider2D（四角形）
            else if (col is BoxCollider2D box)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(box.transform.position, box.transform.rotation, box.transform.lossyScale);
                using (new Handles.DrawingScope(matrix))
                {
                    Vector3 size = new Vector3(box.size.x, box.size.y, 0f);
                    Vector3 center = new Vector3(box.offset.x, box.offset.y, 0f);
                    Handles.DrawWireCube(center, size);
                }
            }
            // 3. CircleCollider2D（円形）
            else if (col is CircleCollider2D circle)
            {
                Handles.DrawWireDisc(
                    circle.transform.position + (Vector3)circle.offset,
                    Vector3.forward,
                    circle.radius * Mathf.Max(circle.transform.lossyScale.x, circle.transform.lossyScale.y)
                );
            }
        }
    }
}