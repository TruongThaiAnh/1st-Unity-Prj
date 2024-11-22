using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft
    {
        get { return facingLeft; }
    }


    public static PlayerController Instance;
    [SerializeField] private float moveSpeed = 1f; // Tốc độ di chuyển của nhân vật, có thể chỉnh sửa trong Inspector
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    private PlayerControls playerControl; // Đối tượng PlayerControls để nhận input từ người chơi
    private Vector2 movement; // Vector lưu hướng di chuyển của nhân vật
    private Rigidbody2D rb; // Tham chiếu tới thành phần Rigidbody2D của nhân vật để xử lý chuyển động vật lý

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;

    private void Awake()
    {
        Instance = this;
        playerControl = new PlayerControls(); // Khởi tạo lớp PlayerMovement để nhận input từ người chơi
        rb = GetComponent<Rigidbody2D>(); // Lấy tham chiếu tới Rigidbody2D của nhân vật
        animator = GetComponent<Animator>(); // Lấy tham chiếu tới Animator của nhân vật
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy tham chiếu tới SpriteRenderer của nhân vật
    }

    private void Start()
    {
        playerControl.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
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
            facingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);  // Đợi khi dash xong
        moveSpeed = startingMoveSpeed;  // Khôi phục lại tốc độ ban đầu
        myTrailRenderer.emitting = false;  // Tắt trail

        yield return new WaitForSeconds(dashCD);  // Chờ cooldown trước khi cho phép dash lại
        isDashing = false;  // Sau cooldown, cho phép dash lại
    }



    /*
 * NOTE:
 * 1. `IEnumerable`:
 *    - Định nghĩa một tập hợp có thể được lặp qua.
 *    - Chỉ có phương thức `GetEnumerator()`, trả về một `IEnumerator`.
 *    - Sử dụng trong các lớp/tập hợp để chỉ rằng nó hỗ trợ lặp.
 *    - Không duy trì trạng thái lặp (trạng thái do `IEnumerator` quản lý).
 *    - Dùng với `foreach` hoặc LINQ queries.
 *
 * 2. `IEnumerator`:
 *    - Định nghĩa cách duyệt qua tập hợp.
 *    - Có các phương thức:
 *        - `MoveNext()`: Di chuyển đến phần tử kế tiếp.
 *        - `Current`: Trả về phần tử hiện tại.
 *        - `Reset()`: Đưa con trỏ về trạng thái ban đầu (không phải lúc nào cũng được hỗ trợ).
 *    - Duy trì trạng thái lặp, được sử dụng trong nội bộ của `foreach`.
 *    - Dùng khi cần điều khiển cụ thể quá trình lặp (ví dụ: lặp ngược, dừng giữa chừng).
 *
 * 3. So sánh `IEnumerable` và `IEnumerator`:
 *    | Đặc điểm              | IEnumerable                           | IEnumerator                     |
 *    |-----------------------|---------------------------------------|---------------------------------|
 *    | Vai trò               | Định nghĩa một tập hợp có thể lặp qua | Điều khiển việc lặp qua tập hợp |
 *    | Trả về                | `IEnumerator`                        | Phần tử hiện tại (`Current`)    |
 *    | Phương thức chính     | `GetEnumerator()`                    | `MoveNext()`, `Reset()`, `Current` |
 *    | Trạng thái lặp        | Không quản lý trạng thái             | Duy trì trạng thái lặp          |
 *    | Sử dụng với `foreach` | Có                                   | Được gọi ngầm trong `foreach`  |
 *
 * 4. Khi nào dùng gì?
 *    - **`IEnumerable`**: Khi bạn muốn lớp/tập hợp có thể lặp qua một cách đơn giản.
 *    - **`IEnumerator`**: Khi bạn cần tùy chỉnh cách duyệt qua tập hợp hoặc kiểm soát trạng thái lặp.
 */

    // Trail Renderer trong Unity là gì?
    // Trail Renderer là một thành phần (Component) trong Unity dùng để tạo hiệu ứng vệt sáng 
    // hoặc vệt chuyển động phía sau một đối tượng khi nó di chuyển. 
    // Thường được sử dụng để tăng tính thẩm mỹ, ví dụ:
    // - Hiệu ứng kiếm vung trong game hành động.
    // - Vệt sáng phía sau đạn hoặc vật thể bay.
    // - Dấu vết chuyển động của nhân vật.

    // Cách thêm Trail Renderer:
    // 1. Chọn đối tượng trong Scene hoặc Hierarchy.
    // 2. Trong Inspector, nhấn Add Component > Chọn Trail Renderer.
    // 3. Cấu hình các thông số trong Inspector.

    // Các thuộc tính quan trọng của Trail Renderer:

    // 1. Time:
    // - Xác định thời gian (theo giây) mà vệt sáng tồn tại trước khi mờ dần và biến mất.
    // - Ví dụ: Time = 1 nghĩa là vệt sáng tồn tại trong 1 giây.

    // 2. Start Width / End Width:
    // - Start Width: Độ rộng của vệt sáng ở phần đầu.
    // - End Width: Độ rộng của vệt sáng ở phần cuối.
    // - Ví dụ: Start Width = 0.5, End Width = 0 sẽ tạo hiệu ứng vệt thon dần.

    // 3. Color Gradient:
    // - Xác định sự chuyển đổi màu sắc từ đầu đến cuối của vệt sáng.
    // - Có thể sử dụng gradient để tạo hiệu ứng chuyển đổi màu (ví dụ từ đỏ sang xanh).

    // 4. Material:
    // - Gán một Material để tạo hiệu ứng đặc biệt cho vệt sáng.
    // - Nên sử dụng Material với Shader phù hợp như Unlit/Transparent hoặc phát sáng (Glow Effect).

    // 5. Min Vertex Distance:
    // - Khoảng cách tối thiểu giữa các điểm của vệt sáng.
    // - Giá trị nhỏ hơn sẽ tạo vệt mượt hơn nhưng tốn tài nguyên hơn.

    // 6. Autodestruct:
    // - Nếu được bật, GameObject chứa Trail Renderer sẽ tự động bị hủy khi vệt sáng biến mất.

    // Ví dụ sử dụng Trail Renderer:
    // Gắn Trail Renderer vào kiếm để tạo hiệu ứng vệt sáng khi vung kiếm.
    // - Time: 0.5s
    // - Start Width: 0.2
    // - End Width: 0
    // - Gán Material có Shader phát sáng để làm nổi bật.

    // Lưu ý hiệu năng:
    // - Sử dụng nhiều Trail Renderer có thể ảnh hưởng đến hiệu suất.
    // - Tối ưu bằng cách điều chỉnh Time và Min Vertex Distance để cân bằng giữa chất lượng và hiệu năng.

}
