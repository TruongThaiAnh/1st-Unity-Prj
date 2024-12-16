using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;

    private PlayerControls playerControls; 

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        Attack();
    }
    public void ToggleIsAttacking(bool isAttacking)
    {
        isAttacking = true;
    } 

    private void StartAttacking()
    {
        isAttacking = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking ) {
            isAttacking=true;
            (currentActiveWeapon as IWeapon).Attack();
        }
    }
    




    //// 1. Sử dụng GetComponent để lấy một thành phần đã gắn vào GameObject
    //private Rigidbody rb;

    //// 2. Sử dụng new để khởi tạo một đối tượng mới không phải là component của Unity
    //private PlayerControls playerControls;

    //private void Awake()
    //{
    //    // **GetComponent<T>()**
    //    // Dùng để lấy tham chiếu (reference) đến một component đã gắn trên GameObject.
    //    // Áp dụng với các class kế thừa từ MonoBehaviour và được quản lý bởi Unity.
    //    rb = GetComponent<Rigidbody>(); // Lấy Rigidbody đã gắn vào GameObject này.

    //    // **new Class**
    //    // Dùng để tạo mới một instance (thể hiện) độc lập.
    //    // Áp dụng với các class không kế thừa từ MonoBehaviour và không được Unity quản lý.
    //    playerControls = new PlayerControls(); // Khởi tạo một đối tượng PlayerControls mới.
    //}

    //private void Start()
    //{
    //    // **Lý do sử dụng từng phương pháp:**
    //    // - GetComponent: Để truy cập các thành phần Unity (như Rigidbody, Transform, Collider...).
    //    // - new Class: Để khởi tạo đối tượng tùy chỉnh, không phải component Unity (ví dụ: hệ thống logic, cấu trúc dữ liệu).

    //    if (rb != null)
    //    {
    //        Debug.Log("Rigidbody đã được tìm thấy bằng GetComponent!");
    //    }

    //    if (playerControls != null)
    //    {
    //        Debug.Log("PlayerControls đã được khởi tạo bằng new!");
    //    }
    //}
}