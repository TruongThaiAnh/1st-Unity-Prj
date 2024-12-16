using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    // Chỉ số của slot đang được kích hoạt
    private int activeSlotIndexNum = 0;

    // Đối tượng quản lý điều khiển đầu vào của người chơi
    private PlayerControls playerControls;

    // Hàm được gọi khi đối tượng khởi tạo
    private void Awake()
    {
        // Khởi tạo playerControls
        playerControls = new PlayerControls();
    }

    // Hàm được gọi khi bắt đầu chạy
    private void Start()
    {
        // Kết nối sự kiện khi nhấn phím trong Inventory.Keyboards
        // ctx đại diện cho ngữ cảnh của hành động (phím được nhấn)
        playerControls.Inventory.Keyboards.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    // Hàm được gọi khi đối tượng được kích hoạt
    private void OnEnable()
    {
        // Kích hoạt xử lý điều khiển đầu vào
        playerControls.Enable();
    }

    // Chuyển đổi slot đang kích hoạt dựa trên số nhận từ đầu vào
    private void ToggleActiveSlot(int numValue)
    {
        // Tính toán chỉ số slot (trừ 1 vì chỉ số bắt đầu từ 0)
        int indexNum = numValue - 1;

        // Kiểm tra xem chỉ số có nằm trong phạm vi hợp lệ không
        if (indexNum < 0 || indexNum >= this.transform.childCount)
        {
            // Ghi cảnh báo nếu giá trị không hợp lệ
            Debug.LogWarning($"Invalid slot number: {numValue}. Ignoring input.");
            return;
        }

        // Chuyển tiếp đến phương thức bật/tắt highlight
        ToggleActiveHighlight(indexNum);
    }

    // Bật/tắt highlight cho slot được chọn
    private void ToggleActiveHighlight(int indexNum)
    {
        // Kiểm tra xem chỉ số có nằm trong phạm vi hợp lệ không
        if (indexNum < 0 || indexNum >= this.transform.childCount)
        {
            // Ghi lỗi nếu giá trị vượt phạm vi
            Debug.LogError($"Index out of bounds: {indexNum}. Total slots: {this.transform.childCount}");
            return;
        }

        // Cập nhật chỉ số của slot đang được kích hoạt
        activeSlotIndexNum = indexNum;

        // Vòng lặp qua tất cả các slot trong danh sách
        foreach (Transform inventorySlot in this.transform)
        {
            // Tắt đối tượng con đầu tiên của mỗi slot
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        // Bật đối tượng con đầu tiên của slot được chọn
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }
}
