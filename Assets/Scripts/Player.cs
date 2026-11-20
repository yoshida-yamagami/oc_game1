using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの動作を管理
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    // SE再生用
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clearSeClip;
    [SerializeField] private AudioClip damageSeClip;

    private enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;
    private const int NO_INPUT_TIME_COUNT = 3;
    private const float JUMP_FORCE = 7f; // ジャンプボタンを押した際にy軸方向にかかる力
    private const float MOVE_SPEED = 4.0f;

    private LayerMask groundLayer; // 床と認識するレイヤーを指定
    private GameDirector gameDirectorScript;
    private Rigidbody2D rb;
    private Animator animator;
    private float speed = 0;
    private bool canMove = true; // 行動可否判定

    // 地面との当たり判定に使用
    Vector3 groundCheckOffset = new Vector3(0.5f, 0.4f, 0);
    float groundCheckDistance = 0.3f; // デバッグ用の線の長さ指定
    private bool isGrounded; // 現在接地しているかどうかを保持する変数（Gizmosでの色分けに使用）
    private int noInputTime = 0; // ジャンプ直後の誤判定を防ぐため、入力受付不可時間を設定

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        gameDirectorScript = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;

        groundLayer = LayerMask.GetMask("Ground"); // Groundレイヤーを床と認識

        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // プレイヤーの向きから、プレイヤーの移動速度と画像の向きを設定
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = MOVE_SPEED;
                spriteRenderer.flipX = false; // 右を向く
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -MOVE_SPEED;
                spriteRenderer.flipX = true;  // 左を向く
                break;
        }

        // 床の上にいる場合、床の速度も加味してプレイヤーを動かす
        float parentVelocityX = 0f;
        if (transform.parent != null)
        {
            Rigidbody2D parentRb = transform.parent.GetComponent<Rigidbody2D>();
            if (parentRb != null)
            {
                parentVelocityX = parentRb.linearVelocity.x;
            }
        }

        // 自分の入力移動速度（speed）に床の移動速度（parentVelocityX）を加算
        rb.linearVelocity = new Vector2(speed + parentVelocityX, rb.linearVelocityY);
    }

    /// <summary>
    /// 被ダメージ時のSE再生
    /// </summary>
    public void PlayDamageSE()
    {
        if (audioSource != null && damageSeClip != null)
        {
            audioSource.PlayOneShot(damageSeClip);
        }
    }

    /// <summary>
    /// クリア時のSE再生
    /// </summary>
    public void PlayClearSE()
    {
        if (audioSource != null && clearSeClip != null)
        {
            audioSource.PlayOneShot(clearSeClip);
        }
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

    /// <summary>
    /// ジャンプの判定
    /// </summary>
    private void JudgeJump()
    {
        // 上昇中にボタンを離したら、速度を急減衰させる（小・中ジャンプになる）
        if (playerInput.actions["Jump"].WasReleasedThisFrame() && rb.linearVelocity.y > 0)
        {
            float cancelRate = 0.5f; // 離した時にどれくらい勢いを殺すか
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * cancelRate);
        }

        bool grounded = IsGround();

        // ジャンプボタンを押されたら初速度を与える
        // ジャンプは地面に接している時のみ可能（2段ジャンプ防止）
        if (grounded && playerInput.actions["Jump"].WasPressedThisFrame())
        {
            float jumpForce = JUMP_FORCE; // ジャンプボタンを押されたときに上方向に与える力
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            noInputTime = NO_INPUT_TIME_COUNT;
            return;
        }
    }

    /// <summary>
    /// プレイヤーが地面に接地しているかを判定
    /// </summary>
    /// <returns></returns>
    private bool IsGround()
    {
        Vector3 leftStart = transform.position + new Vector3(-groundCheckOffset.x, -groundCheckOffset.y, 0);
        Vector3 centerStart = transform.position + new Vector3(0, -groundCheckOffset.y, 0); ;
        Vector3 rightStart = transform.position + new Vector3(groundCheckOffset.x, -groundCheckOffset.y, 0);
        Vector3 leftEnd = leftStart - Vector3.up * groundCheckDistance;
        Vector3 centerEnd = centerStart - Vector3.up * groundCheckDistance;
        Vector3 rightEnd = rightStart - Vector3.up * groundCheckDistance;

        // Linecast実行
        RaycastHit2D hitL = Physics2D.Linecast(leftStart, leftEnd, groundLayer);
        RaycastHit2D hitC = Physics2D.Linecast(centerStart, centerEnd, groundLayer);
        RaycastHit2D hitR = Physics2D.Linecast(rightStart, rightEnd, groundLayer);

        bool grounded = false;
        if (noInputTime > 0)
        {
            // 入力不可時間が設定されている場合はカウントダウン&ジャンプ状態
            noInputTime--;
        }
        else
        {
            // 接地判定
            grounded = hitL.collider != null || hitC.collider != null || hitR.collider != null;
        }

        // 判定時のみ色を変える(接地：緑、非接地：赤)
        Color lineColor = grounded ? Color.green : Color.red;
        Debug.DrawLine(leftStart, leftEnd, lineColor);
        Debug.DrawLine(centerStart, centerEnd, lineColor);
        Debug.DrawLine(rightStart, rightEnd, lineColor);

        return grounded;
    }

    // Gizmos（ギズモ）表示操作
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
        Vector3 centerStart = transform.position + new Vector3(0, -groundCheckOffset.y, 0); ;
        Vector3 rightStart = transform.position + new Vector3(groundCheckOffset.x, -groundCheckOffset.y, 0);

        // ギズモとして線を描画
        Gizmos.DrawLine(leftStart, leftStart - Vector3.up * groundCheckDistance);
        Gizmos.DrawLine(centerStart, centerStart - Vector3.up * groundCheckDistance);
        Gizmos.DrawLine(rightStart, rightStart - Vector3.up * groundCheckDistance);
    }

    /// <summary>
    /// プレイヤーをfloorTransformの子にする(床の動きがプレイヤーに影響するようになる)
    /// </summary>
    /// <param name="floorTransform"></param>
    public void SetPassengerOf(Transform floorTransform)
    {
        if (transform.parent != floorTransform)
        {
            transform.SetParent(floorTransform);
        }
    }

    /// <summary>
    /// プレイヤーの親子関係をキャンセルする
    /// </summary>
    public void ReleasePassenger()
    {
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
    }

    // 外部（トゲなど）から呼び出してダメージを受ける窓口
    public void TakeDamage()
    {
        GameOver(); // 既存のGameOverメソッドを呼び出す
    }

    // 他のオブジェクトとの衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 動けない状態なら判定をしない
        if (!canMove)
        {
            return;
        }

        // トラップとぶつかったらゲームオーバー
        if (collision.collider.gameObject.CompareTag("Trap"))
        {
            // ゲームオーバー処理
            GameOver();
        }
    }

    // 他のオブジェクトとの通過判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 動けない状態なら判定をしない
        if (!canMove)
        {
            return;
        }

        // ゴールに接触したらゲームクリア
        if (collision.gameObject.tag == "Goal")
        {
            // ステージクリア処理
            PlayClearSE();
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
            // コインを取得する際、プレイヤーがコインの子オブジェクトになっているとプレイヤーも消されてしまう
            // なので、コインを取る前にプレイヤーの親情報をリセット
            transform.SetParent(null);
            Destroy(collision.gameObject);
        }
    }

    // ゲームクリア時の処理
    private void GameClear()
    {
        gameDirectorScript.GameClear();

        // 行動できないように制御
        canMove = false;

        // 座標を固定
        rb.linearVelocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // ゲームオーバー用のアニメーションを再生
        animator.SetBool("IsClear", true);
    }

    // ゲームオーバー時の処理
    private void GameOver()
    {
        gameDirectorScript.GameOver();

        // 行動できないように制御
        canMove = false;

        // 座標を固定
        rb.linearVelocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // ゲームクリア用のアニメーションを再生
        animator.SetBool("IsGameOver", true);
    }
}
