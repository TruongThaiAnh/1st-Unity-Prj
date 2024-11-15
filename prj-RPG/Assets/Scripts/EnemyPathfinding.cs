﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Tốc độ di chuyển của Enemy

    private Rigidbody2D rb; // Tham chiếu tới Rigidbody2D của Enemy để xử lý vật lý
    private Vector2 moveDir; // Hướng di chuyển của Enemy

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy thành phần Rigidbody2D từ GameObject
    }

    private void FixedUpdate()
    {
        // Di chuyển Enemy theo hướng moveDir và tốc độ moveSpeed
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    // Phương thức được gọi bởi EnemyAI để thiết lập hướng di chuyển đến vị trí đích
    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition; // Đặt hướng di chuyển là vị trí đích truyền vào
    }
}
