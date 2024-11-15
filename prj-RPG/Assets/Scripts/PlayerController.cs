using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public bool FacingLeft
    //{
    //    get { return facingLeft; }
    //    set { facingLeft = value; }
    //}
    [SerializeField] private float moveSpeed = 1f; // Tốc độ di chuyển của nhân vật, có thể chỉnh sửa trong Inspector

    private PlayerControls playerControl; // Đối tượng PlayerControls để nhận input từ người chơi
    private Vector2 movement; // Vector lưu hướng di chuyển của nhân vật
    private Rigidbody2D rb; // Tham chiếu tới thành phần Rigidbody2D của nhân vật để xử lý chuyển động vật lý

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerControl = new PlayerControls(); // Khởi tạo lớp PlayerMovement để nhận input từ người chơi
        rb = GetComponent<Rigidbody2D>(); // Lấy tham chiếu tới Rigidbody2D của nhân vật
        animator = GetComponent<Animator>(); // Lấy tham chiếu tới Animator của nhân vật
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy tham chiếu tới SpriteRenderer của nhân vật
    }

    private void OnEnable()
    {
       playerControl.Enable(); // Kích hoạt hệ thống nhận input
    }

    private void Update()
    {
        PlayerInput(); // Xử lý input của người chơi mỗi khung hình
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection(); // Điều chỉnh hướng nhân vật dựa trên vị trí của chuột
        Move(); // Di chuyển nhân vật mỗi khung hình vật lý
    }

    private void PlayerInput()
    {
        // Lấy vector di chuyển từ input
        movement = playerControl.Movement.Move.ReadValue<Vector2>();

        // Cập nhật giá trị cho hoạt ảnh di chuyển, `moveX` và `moveY` là tham số trong Animator
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        // Di chuyển nhân vật theo hướng `movement`, tốc độ `moveSpeed`, và thời gian vật lý cố định
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        // Lấy vị trí chuột trên màn hình
        Vector3 mousePos = Input.mousePosition;
        // Chuyển vị trí của nhân vật sang hệ tọa độ màn hình
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Kiểm tra nếu chuột nằm bên trái nhân vật thì lật hình ảnh nhân vật quay trái, ngược lại quay phải
        if (mousePos.x < playerScreenPoint.x)
        {
            spriteRenderer.flipX = true;
            //FacingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            //FacingLeft = false;
        }
    }
}
