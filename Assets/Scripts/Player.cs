using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// プレイヤーの動作を管理
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    private Rigidbody2D rigidbody2D;
    private float speed = 0;
    private float jumpPower = 400;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 方向を決定
        ReadMoveInput();

        // ジャンプ判定
        if (playerInput.actions["Jump"].WasPressedThisFrame())
        {
            Jump();
        }
    }

    /// <summary>
    /// 一定間隔で呼ばれる
    /// </summary>
    private void FixedUpdate()
    {
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

        rigidbody2D.linearVelocity = new Vector2(speed, rigidbody2D.linearVelocityY);
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

    private void Jump()
    {
        // 上に力を加える
        rigidbody2D.AddForce(Vector2.up * jumpPower);
    }
}
