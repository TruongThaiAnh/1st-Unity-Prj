using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Tốc độ di chuyển của nhân vật, có thể chỉnh sửa trong Inspector

    private PlayerControls playerControl; // Đối tượng PlayerControls để nhận input từ người chơi
    private Vector2 movement; // Vector lưu hướng di chuyển của nhân vật
    private Rigidbody2D rb; // Tham chiếu tới thành phần Rigidbody2D của nhân vật để xử lý chuyển động vật lý

    private void Awake()
    {
        // Khởi tạo PlayerControls để xử lý input
        playerControl = new PlayerControls();

        // Lấy tham chiếu tới thành phần Rigidbody2D của đối tượng để thực hiện các thao tác vật lý
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Kích hoạt khả năng nhận input của PlayerControls khi đối tượng được kích hoạt
        playerControl.Enable();
    }

    private void Update()
    {
        // Gọi hàm PlayerInput() mỗi frame để lấy input từ người chơi
        PlayerInput();
    }

    private void FixedUpdate()
    {
        // Gọi hàm Move() mỗi khung hình vật lý cố định để đảm bảo chuyển động trơn tru
        Move();
    }

    private void PlayerInput()
    {
        // Lấy vector di chuyển từ input của người chơi (vector 2D đại diện cho hướng di chuyển)
        movement = playerControl.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        // Tính toán vị trí mới bằng cách cộng vị trí hiện tại với hướng di chuyển nhân với tốc độ và thời gian
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}
