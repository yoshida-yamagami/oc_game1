using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの動作を管理
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;

    private enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    private GameDirector gameDirectorScript;
    private Rigidbody2D rb;
    private Animator animator;
    private float speed = 0;
    private bool canMove = true; // 行動可否判定

    // 地面との当たり判定に使用
    Vector3 groundCheckOffset = new Vector3(0.3f, 0.3f, 0); // 左右の幅0.3、下方向へ0.3
    float groundCheckDistance = 0.3f; // デバッグ用の線の長さ指定
    private bool isGrounded; // 現在接地しているかどうかを保持する変数（Gizmosでの色分けに使用）

    void Start()
    {
        gameDirectorScript = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        // 行動できない場合は処理しない
        if (!canMove)
        {
            return;
        }

        // 方向を決定
        ReadMoveInput();

        // ジャンプ判定
        JudgeJump();
    }

    /// <summary>
    /// 一定間隔で呼ばれる
    /// </summary>
    private void FixedUpdate()
    {
        // 行動できない場合は処理しない
        if (!canMove)
        {
            return;
        }

        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
    }
    
    /// <summary>
    /// 水平方向の入力から、プレイヤーの方向を設定
    /// </summary>
    private void ReadMoveInput()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        if (inputVector.x == 0)
        {
            // 止まっている状態
            direction = DIRECTION_TYPE.STOP;
        }
        else if (inputVector.x > 0)
        {
            // 右に動く
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (inputVector.x < 0)
        {
            // 左に動く
            direction = DIRECTION_TYPE.LEFT;
        }
    }

    private void JudgeJump()
    {
        // ジャンプボタンを押されたら初速度を与える
        // ジャンプは地面に接している時のみ可能（2段ジャンプ防止）
        if (IsGround() && playerInput.actions["Jump"].WasPressedThisFrame())
        {
            float jumpForce = 7f; // ジャンプボタンを押されたときに上方向に与える力
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump"); // ジャンプアニメーションスタート
        }

        // 上昇中にボタンを離したら、速度を急減衰させる（小・中ジャンプになる）
        if (playerInput.actions["Jump"].WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            float cancelRate = 0.5f; // 離した時にどれくらい勢いを殺すか
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * cancelRate);
        }

        // 接地状態を常にAnimatorに伝える
        animator.SetBool("isGround", IsGround());
    }

    private bool IsGround()
    {
        Vector3 leftStart = transform.position + new Vector3(-groundCheckOffset.x, -groundCheckOffset.y, 0);
        Vector3 rightStart = transform.position + new Vector3(groundCheckOffset.x, -groundCheckOffset.y, 0);
        Vector3 leftEnd = leftStart - Vector3.up * groundCheckDistance;
        Vector3 rightEnd = rightStart - Vector3.up * groundCheckDistance;

        // Linecast実行
        RaycastHit2D hitL = Physics2D.Linecast(leftStart, leftEnd, groundLayer);
        RaycastHit2D hitR = Physics2D.Linecast(rightStart, rightEnd, groundLayer);

        // 接地判定
        bool grounded = hitL.collider != null || hitR.collider != null;

        // --- 1. 判定時のみ色を変える (Debug.DrawLine) ---
        // 接地していたら緑、浮いていたら赤
        Color lineColor = grounded ? Color.green : Color.red;
        Debug.DrawLine(leftStart, leftEnd, lineColor);
        Debug.DrawLine(rightStart, rightEnd, lineColor);

        // デバッグ用ログ：何に当たっているか表示
        if (hitL.collider != null) Debug.Log($"Left hit: {hitL.collider.name}");
        if (hitR.collider != null) Debug.Log($"Right hit: {hitR.collider.name}");

        return grounded;
    }

    // --- 2. Gizmos（ギズモ）を利用する ---
    // このメソッドはUnityエディタ上で常に（再生していなくても）呼ばれます
    private void OnDrawGizmos()
    {
        // アプリケーションが動いていない時は白、動いている時は判定結果に合わせる
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.white;
        }
        else
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
        }

        // 判定位置を計算（Update内の計算と同じにする）
        Vector3 leftStart = transform.position + new Vector3(-groundCheckOffset.x, -groundCheckOffset.y, 0);
        Vector3 rightStart = transform.position + new Vector3(groundCheckOffset.x, -groundCheckOffset.y, 0);

        // ギズモとして線を描画
        Gizmos.DrawLine(leftStart, leftStart - Vector3.up * groundCheckDistance);
        Gizmos.DrawLine(rightStart, rightStart - Vector3.up * groundCheckDistance);
    }

    // 他のオブジェクトとの衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // トラップとぶつかったらゲームオーバー
        if (collision.gameObject.tag == "Trap")
        {
            // ゲームオーバー処理
            GameOver();
        }
    }

    // 他のオブジェクトとの通過判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ゴールに接触したらゲームクリア
        if (collision.gameObject.tag == "Goal")
        {
            // ステージクリア処理
            GameClear();
        }

        // 奈落に接触したらゲームオーバー
        if (collision.gameObject.tag == "Chaos")
        {
            // ゲームオーバー処理
            GameOver();
        }

        // コインに接触したらコインを取得
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }
    }

    // ゲームオーバー時の処理
    private void GameClear()
    {
        gameDirectorScript.GameClear();

        // 行動できないように制御
        canMove = false;
    }

    // ゲームオーバー時の処理
    private void GameOver()
    {
        gameDirectorScript.GameOver();

        // 行動できないように制御
        canMove = false;
    }
}
