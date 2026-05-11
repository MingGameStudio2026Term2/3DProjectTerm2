using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
    // 移动加速度，可以在 Inspector 里调整
    public float acceleration = 10f;

    // 水平阻力，可以在 Inspector 里调整
    public float horizontalDrag = 10f;

    // 跳跃力度，可以在 Inspector 里调整
    public float jumpForce = 5f;

    // 鼠标旋转灵敏度，可以在 Inspector 里调整
    public float rotationSpeed = 3f;

    // 记录 Player 是否正在地面上
    private bool isGrounded = false;

    // 获取 Rigidbody 组件的引用
    private Rigidbody rb;

    // PlayerInput 组件
    private PlayerInput playerInput;

    private Animator animator;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;

    void Start()
    {
        // 游戏开始时找到这个物体上的 Rigidbody
        rb = GetComponent<Rigidbody>();
        // 不设置内置阻力，手动控制水平阻力
        rb.drag = 0f;

        // 获取 PlayerInput 组件
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            lookAction = playerInput.actions["Look"];
        }

        // 获取 Animator 组件，用于同步 Speed 参数
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (lookAction != null)
        {
            // 读取鼠标左右移动，旋转 Player（绕 Y 轴）
            Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
            transform.Rotate(0f, mouseDelta.x * rotationSpeed * Time.deltaTime, 0f);
        }

        if (jumpAction != null && jumpAction.WasPressedThisFrame() && isGrounded)
        {
            // 向上施加一个瞬间冲力
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (moveAction != null)
        {
            // 读取移动输入
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            // 计算输入强度
            float inputMagnitude = moveInput.magnitude;

            if (inputMagnitude > 0)
            {
                // 归一化方向并应用加速度
                Vector3 direction = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
                rb.AddForce(direction * acceleration * Mathf.Min(1f, inputMagnitude), ForceMode.Acceleration);
            }
        }

        // 手动应用水平阻力，不影响垂直速度
        rb.velocity = new Vector3(
            rb.velocity.x * Mathf.Exp(-horizontalDrag * Time.fixedDeltaTime),
            rb.velocity.y,
            rb.velocity.z * Mathf.Exp(-horizontalDrag * Time.fixedDeltaTime)
        );

        if (animator != null)
        {
            float currentSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            animator.SetFloat("Speed", currentSpeed);
        }
    }

    // 当 Player 碰到地面时，标记为已落地
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }
}
