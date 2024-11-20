using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;


    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

     void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();    
    }


    private void Update()
    {
        MouseFollowWithOffset();
    }


    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab,slashAnimSpawnPoint.position,Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    private void DoneAttackingAnimeEvent()
    {
        weaponCollider.gameObject.SetActive(false);
      
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition; // Lấy vị trí chuột trên màn hình
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position); // Chuyển vị trí của nhân vật sang hệ tọa độ màn hình

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; // Tính góc quay để kiếm hướng về chuột

        // Nếu chuột ở bên trái nhân vật, xoay kiếm theo trục X
        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle); // Nếu chuột ở bên phải, kiếm giữ nguyên trục X
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }


}
