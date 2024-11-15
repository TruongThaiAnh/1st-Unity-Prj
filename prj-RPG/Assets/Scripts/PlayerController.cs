using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Tốc độ di chuyển của nhân vật

    private PlayerMovement playerMovement; // Tham chiếu tới lớp PlayerMovement, điều khiển input
    private Vector2 movement; // Vector để lưu hướng di chuyển từ input
    private Rigidbody2D rb; // Tham chiếu tới Rigidbody2D của nhân vật
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        playerMovement = new PlayerMovement(); // Khởi tạo lớp PlayerMovement để nhận input từ người chơi
        rb = GetComponent<Rigidbody2D>(); // Lấy tham chiếu tới Rigidbody2D
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerMovement.Enable(); // Kích hoạt hệ thống nhận input
    }

    private void Update()
    {
        PlayerInput(); // Lấy input từ người chơi trong mỗi khung hình
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move(); // Di chuyển nhân vật trong mỗi khung hình vật lý
    }

    private void PlayerInput()
    {
        movement = playerMovement.Movement.Move.ReadValue<Vector2>(); // Lấy vector di chuyển từ input

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);

    }
    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime)); // Di chuyển nhân vật dựa trên input và tốc độ
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
        }else
        {
            mySpriteRenderer.flipX = false;
        }
    }

}