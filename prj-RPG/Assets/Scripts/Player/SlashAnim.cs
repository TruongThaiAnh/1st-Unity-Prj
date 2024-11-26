using UnityEngine;

public class SlashAnim : MonoBehaviour
{
    // Tham chiếu đến thành phần ParticleSystem trên GameObject này
    private ParticleSystem ps;

    private void Awake()
    {
        // Gán ParticleSystem từ chính GameObject hiện tại
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Kiểm tra nếu ParticleSystem (ps) đã tồn tại và:
        // - !ps.IsAlive() => ParticleSystem không còn "sống" (kết thúc phát hiệu ứng)
        if (ps && !ps.IsAlive())
        {
            // Nếu điều kiện trên đúng, gọi hàm DestroySelf() để hủy GameObject này
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        // Hủy GameObject này khỏi Scene
        Destroy(gameObject);
    }
}