using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;

    // Enum để xác định trạng thái của Enemy
    private enum State
    {
        Roaming // Trạng thái di chuyển ngẫu nhiên
    }

    private State state; // Biến lưu trạng thái hiện tại của Enemy
    private EnemyPathfinding enemyPathfinding; // Tham chiếu tới thành phần EnemyPathfinding

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>(); // Lấy thành phần EnemyPathfinding từ GameObject
        state = State.Roaming; // Đặt trạng thái ban đầu là Roaming
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine()); // Bắt đầu coroutine để thực hiện hành vi di chuyển ngẫu nhiên
    }

    // Coroutine để thực hiện hành vi di chuyển ngẫu nhiên
    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming) // Lặp lại khi Enemy ở trạng thái Roaming
        {
            Vector2 roamPosition = GetRoamingPosition(); // Lấy vị trí ngẫu nhiên để di chuyển tới
            enemyPathfinding.MoveTo(roamPosition); // Gọi phương thức MoveTo của EnemyPathfinding để di chuyển tới vị trí
            yield return new WaitForSeconds(roamChangeDirFloat); // Đợi 2 giây trước khi chọn vị trí mới
        }
    }

    // Hàm để lấy vị trí ngẫu nhiên xung quanh Enemy
    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Trả về một vector ngẫu nhiên trong bán kính từ (-1, -1) đến (1, 1)
    }
}