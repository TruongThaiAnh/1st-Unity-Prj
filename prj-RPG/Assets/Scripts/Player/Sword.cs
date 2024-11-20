using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab; // Prefab cho hiệu ứng chém
    [SerializeField] private Transform slashAnimSpawnPoint; // Điểm xuất hiện hiệu ứng chém
    [SerializeField] private Transform weaponCollider; // Vùng va chạm của kiếm
    [SerializeField] private float swordAttackCD = .5f; // Thời gian hồi sau mỗi đòn tấn công

    private PlayerControls playerControls; // Hệ thống input cho nhân vật
    private Animator myAnimator; // Điều khiển hoạt ảnh của nhân vật
    private PlayerController playerController; // Tham chiếu đến PlayerController để lấy trạng thái nhân vật
    private ActiveWeapon activeWeapon; // Quản lý trạng thái vũ khí hiện tại
    private bool attackButtonDown, isAttacking = false; // Trạng thái tấn công: đang nhấn phím và đang thực hiện tấn công

    private GameObject slashAnim; // Biến lưu hiệu ứng chém được tạo ra

    private void Awake()
    {
        // Khởi tạo các thành phần khi GameObject được kích hoạt
        playerController = GetComponentInParent<PlayerController>(); // Lấy PlayerController từ đối tượng cha
        activeWeapon = GetComponentInParent<ActiveWeapon>(); // Lấy ActiveWeapon từ đối tượng cha
        myAnimator = GetComponent<Animator>(); // Lấy Animator trên GameObject hiện tại
        playerControls = new PlayerControls(); // Khởi tạo hệ thống input
    }

    private void OnEnable()
    {
        // Kích hoạt hệ thống input khi GameObject được bật
        playerControls.Enable();
    }

    void Start()
    {
        // Gán sự kiện input cho phím tấn công
        playerControls.Combat.Attack.started += _ => StartAttacking(); // Khi nhấn phím, gọi hàm StartAttacking()
        playerControls.Combat.Attack.canceled += _ => StopAttacking(); // Khi nhả phím, gọi hàm StopAttacking()
    }

    private void Update()
    {
        // Gọi liên tục mỗi frame
        MouseFollowWithOffset(); // Theo dõi vị trí chuột và xoay kiếm theo hướng chuột
        Attack(); // Kiểm tra trạng thái tấn công
    }

    private void StartAttacking()
    {
        // Kích hoạt trạng thái "đang nhấn phím tấn công"
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        // Hủy trạng thái "đang nhấn phím tấn công"
        attackButtonDown = false;
    }

    private void Attack()
    {
        // Kiểm tra nếu đang nhấn nút và chưa trong trạng thái tấn công
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true; // Đặt trạng thái đang tấn công
            myAnimator.SetTrigger("Attack"); // Kích hoạt hoạt ảnh tấn công
            weaponCollider.gameObject.SetActive(true); // Bật vùng va chạm để kiểm tra va chạm với kẻ địch
            // Tạo hiệu ứng chém tại vị trí chỉ định
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent; // Đặt hiệu ứng làm con của nhân vật
            StartCoroutine(AttackCDRoutine()); // Bắt đầu thời gian hồi để ngăn spam tấn công
        }
    }

    private IEnumerator AttackCDRoutine()
    {
        // Coroutine: tạm dừng thực thi trong khoảng thời gian hồi chiêu
        yield return new WaitForSeconds(swordAttackCD); // Chờ thời gian hồi
        isAttacking = false; // Cho phép tấn công lại
    }

    public void DoneAttackingAnimEvent()
    {
        // Sự kiện hoạt ảnh: tắt vùng va chạm khi hoạt ảnh kết thúc
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        // Xử lý hiệu ứng chém khi chém lên
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0); // Xoay hiệu ứng lên trên
        if (playerController.FacingLeft) // Nếu nhân vật quay mặt sang trái
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true; // Lật hiệu ứng theo trục X
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        // Xử lý hiệu ứng chém khi chém xuống
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Xoay hiệu ứng xuống dưới
        if (playerController.FacingLeft) // Nếu nhân vật quay mặt sang trái
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true; // Lật hiệu ứng theo trục X
        }
    }

    private void MouseFollowWithOffset()
    {
        // Lấy vị trí chuột trên màn hình
        Vector3 mousePos = Input.mousePosition;
        // Vị trí của nhân vật trong hệ tọa độ màn hình
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        // Tính toán góc giữa nhân vật và chuột
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // Xoay vũ khí dựa trên vị trí chuột
        if (mousePos.x < playerScreenPoint.x)
        {
            // Nếu chuột ở bên trái nhân vật
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle); // Xoay vũ khí theo hướng chuột
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0); // Xoay vùng va chạm
        }
        else
        {
            // Nếu chuột ở bên phải nhân vật
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle); // Xoay vũ khí theo hướng chuột
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0); // Xoay vùng va chạm
        }
    }
}
