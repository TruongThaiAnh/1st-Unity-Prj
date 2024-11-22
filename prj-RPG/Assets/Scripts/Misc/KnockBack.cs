using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    // Biến trạng thái: Kiểm tra xem nhân vật có đang bị knockback hay không
    public bool GettingKnockedBack { get; private set; }

    // Thời gian nhân vật sẽ bị knockback (đẩy lùi)
    [SerializeField] private float knockBackTime = 0.2f;

    // Thành phần Rigidbody2D để điều khiển vật lý
    private Rigidbody2D rb;

    private void Awake()
    {
        // Gán tham chiếu Rigidbody2D của đối tượng
        rb = GetComponent<Rigidbody2D>();
    }

    // Hàm xử lý knockback (gọi khi nhân vật bị sát thương)
    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        // Đặt trạng thái knockback thành true
        GettingKnockedBack = true;

        // Hệ số giảm lực (giảm 50% knockback)
        float dampingFactor = 0.5f;

        // Tính toán lực knockback:
        // Lực được tính theo hướng từ nguồn sát thương đến nhân vật (đẩy lùi nhân vật).
        Vector2 difference = (transform.position - damageSource.position).normalized
                             * knockBackThrust // Cường độ lực knockback
                             * rb.mass // Điều chỉnh lực theo khối lượng
                             * dampingFactor;

        // Áp dụng lực lên Rigidbody2D với ForceMode2D.Impulse (tác động tức thời)
        rb.AddForce(difference, ForceMode2D.Impulse);

        // Bắt đầu Coroutine để kiểm soát thời gian và dừng knockback sau đó
        StartCoroutine(KnockRoutine());
    }

    // Coroutine để dừng knockback sau một khoảng thời gian
    private IEnumerator KnockRoutine()
    {
        // Dừng thực thi trong knockBackTime giây
        yield return new WaitForSeconds(knockBackTime);

        // Dừng vận tốc của Rigidbody2D (ngăn đối tượng tiếp tục trôi)
        rb.velocity = Vector2.zero;

        // Đặt trạng thái knockback thành false (không còn bị knockback)
        GettingKnockedBack = false;
    }
}

/*
    --- Ghi chú chi tiết ---
    
    1. **Knockback là gì?**
       - Knockback là hiệu ứng đẩy lùi nhân vật khi bị tấn công, tạo cảm giác tương tác vật lý thực tế trong trò chơi.

    2. **`rb.AddForce(difference, ForceMode2D.Impulse)`**
       - Hàm này áp dụng một lực vật lý tức thời lên nhân vật.
       - Lực được tính bằng cách:
         - Xác định hướng đẩy lùi dựa trên vị trí nguồn sát thương (`damageSource`) và nhân vật (`transform.position`).
         - Chuẩn hóa vector hướng (`.normalized`) để giữ nguyên hướng nhưng không thay đổi cường độ.
         - Nhân với cường độ knockback (`knockBackThrust`) và khối lượng (`rb.mass`) để tạo hiệu ứng chân thực hơn.
       - **ForceMode2D.Impulse**: Áp dụng lực ngay lập tức thay vì chia đều qua nhiều khung hình.

    3. **`StartCoroutine(KnockRoutine());`**
       - Gọi một quy trình (Coroutine) cho phép xử lý các hành động diễn ra theo thời gian (như đợi vài giây).

    4. **`IEnumerator KnockRoutine()`**
       - Là một Coroutine, giúp tạm dừng thực thi mã:
         - **`yield return new WaitForSeconds(knockBackTime);`**: Tạm dừng trong khoảng thời gian đã định.
         - Sau đó:
           - **`rb.velocity = Vector2.zero;`**: Dừng chuyển động của nhân vật.
           - **`gettingKnockedBack = false;`**: Cập nhật trạng thái để nhân vật có thể di chuyển lại bình thường.
*/
