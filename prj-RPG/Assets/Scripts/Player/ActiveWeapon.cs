using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;


    private PlayerControls playerControls;

    private bool attackButton,isActtack = false ;

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

    public void ToggleIsAttacking(bool valus)
    {
        isActtack = valus;
    }

    private void StartAttacking()
    {
        attackButton = true;
    }

    private void StopAttacking()
    {
        attackButton = false;
    }

    private void Attack()
    {
        if (attackButton && !isActtack) {
            isActtack=true;
            (currentActiveWeapon as IWeapon).Attack();
        }
    }
}