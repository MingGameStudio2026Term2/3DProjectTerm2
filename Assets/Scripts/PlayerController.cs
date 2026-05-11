using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移动加速度，可以在 Inspector 里调整
    public float acceleration = 10f;

    // 跳跃力度，可以在 Inspector 里调整
    public float jumpForce = 5f;

	// 鼠标旋转灵敏度，可以在 Inspector 里调整
	public float rotationSpeed = 3f;

	// 记录 Player 是否正在地面上
	private bool isGrounded = false;

    // 获取 Rigidbody 组件的引用
    private Rigidbody rb;

    void Start()
    {
        // 游戏开始时找到这个物体上的 Rigidbody
        rb = GetComponent<Rigidbody>();
        // 设置阻力以便在无输入时停止
        rb.drag = 10f;
    }

    void Update()
    {
        // 按下空格键且在地面上时才能跳跃（防止空中二段跳）
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 向上施加一个瞬间冲力
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

		// 读取鼠标左右移动，旋转 Player（绕 Y 轴）
		float mouseX = Input.GetAxis("Mouse X");
		transform.Rotate(0f, mouseX * rotationSpeed, 0f);
	}

    void FixedUpdate()
    {
        // 读取键盘输入（WASD 或方向键），返回 -1 到 1 之间的值
        float moveX = Input.GetAxis("Horizontal"); // 左右：A/D 或 ←/→
        float moveZ = Input.GetAxis("Vertical");   // 前后：W/S 或 ↑/↓

        // 计算输入强度
        Vector3 inputVector = new Vector3(moveX, 0f, moveZ);
        float inputMagnitude = inputVector.magnitude;

        if (inputMagnitude > 0)
        {
            // 归一化方向并应用加速度
            Vector3 direction = (transform.right * moveX + transform.forward * moveZ).normalized;
            rb.AddForce(direction * acceleration * Mathf.Min(1f, inputMagnitude), ForceMode.Acceleration);
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
