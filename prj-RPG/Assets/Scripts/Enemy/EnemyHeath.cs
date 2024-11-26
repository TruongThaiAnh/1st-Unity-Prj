using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Số máu khởi đầu của kẻ địch, có thể điều chỉnh từ Inspector
    [SerializeField] private int startingHealth = 3;

    // Prefab hiển thị hiệu ứng khi kẻ địch chết (có thể là nổ, ánh sáng, hoặc hiệu ứng đặc biệt)
    [SerializeField] private GameObject deathVFXPrefabs;

    [SerializeField] private float knockBackThrust = 15f;

    // Biến lưu trữ số máu hiện tại của kẻ địch
    private int currentHealth;

    // Các thành phần được gắn trên kẻ địch để hỗ trợ knockback và hiệu ứng flash
    private KnockBack knockBack;

    private Flash flash;

    private void Awake()
    {
        // Lấy thành phần Flash từ GameObject hiện tại
        flash = GetComponent<Flash>();

        // Lấy thành phần KnockBack từ GameObject hiện tại
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        // Khởi tạo máu hiện tại bằng giá trị máu khởi đầu
        currentHealth = startingHealth;
    }

    // Hàm để kẻ địch nhận sát thương
    public void TakeDamage(int damage)
    {
        // Trừ đi số máu tương ứng với sát thương nhận vào
        currentHealth -= damage;

        // Kẻ địch bị đẩy lùi (knockback), sử dụng thành phần KnockBack
        // PlayerController.Instance đại diện cho vị trí người chơi tấn công
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);

        // Bắt đầu hiệu ứng flash để hiển thị rằng kẻ địch đã bị tấn công
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        // Kiểm tra nếu máu <= 0, gọi hàm DetectDeath
        DetectDeath();
    }

    // Hàm kiểm tra và xử lý cái chết của kẻ địch
    public void DetectDeath()
    {
        // Hiển thị thông báo trên Console để theo dõi logic
        Debug.Log($"DetectDeath called. Current Health: {currentHealth}");

        // Nếu máu nhỏ hơn hoặc bằng 0, tiến hành xử lý cái chết
        if (currentHealth <= 0)
        {
            // Hiển thị hiệu ứng cái chết tại vị trí của kẻ địch
            Instantiate(deathVFXPrefabs, transform.position, Quaternion.identity);

            // Hủy kẻ địch khỏi Scene
            Destroy(gameObject);
        }
    }
}