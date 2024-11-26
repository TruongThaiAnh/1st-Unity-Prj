using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TransparencyTrigger : MonoBehaviour
{
    // Thanh trượt trong Unity Inspector (giá trị từ 0 đến 1) để đặt độ trong suốt mục tiêu
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f; // Độ trong suốt khi làm mờ (0 = hoàn toàn trong suốt, 1 = không trong suốt)
    [SerializeField] private float fadeTime = 0.4f; // Thời gian (giây) để hoàn thành hiệu ứng mờ

    // Các thành phần cần để thay đổi alpha của đối tượng
    private SpriteRenderer spriteRenderer; // Thành phần SpriteRenderer nếu đối tượng là Sprite
    private Tilemap tilemap; // Thành phần Tilemap nếu đối tượng là một Tilemap

    private void Awake()
    {
        // Gán các thành phần của đối tượng hiện tại
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi hay không
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Nếu đối tượng có SpriteRenderer, bắt đầu hiệu ứng mờ
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            // Nếu đối tượng có Tilemap, bắt đầu hiệu ứng mờ
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng rời khỏi va chạm có phải là người chơi hay không
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Nếu đối tượng có SpriteRenderer, khôi phục độ trong suốt về trạng thái ban đầu
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            }
            // Nếu đối tượng có Tilemap, khôi phục độ trong suốt về trạng thái ban đầu
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    // Coroutine làm mờ cho SpriteRenderer
    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0; // Thời gian đã trôi qua

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua
            // Nội suy giá trị alpha từ startValue đến targetTransparency
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            // Cập nhật màu sắc của SpriteRenderer
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null; // Chờ đến khung hình tiếp theo
        }
    }

    // Coroutine làm mờ cho Tilemap
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0; // Thời gian đã trôi qua

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua
            // Nội suy giá trị alpha từ startValue đến targetTransparency
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            // Cập nhật màu sắc của Tilemap
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null; // Chờ đến khung hình tiếp theo
        }
    }
}
