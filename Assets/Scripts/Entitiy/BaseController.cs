using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rig;

    [SerializeField] SpriteRenderer characterRenderer;
    [SerializeField] Transform weaponPivot;

    protected Vector2 moveDir = Vector2.zero;
    public Vector2 MoveDir => moveDir;

    protected Vector2 lookDir = Vector2.zero;
    public Vector2 LookDir => lookDir;

    Vector2 knockback = Vector2.zero;
    float knockbackDuration = 0.0f;

    protected AnimationHandler animHandller;
    protected StatHandler statHanddler;

    [SerializeField] public WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    protected virtual void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        animHandller = GetComponent<AnimationHandler>();
        statHanddler = GetComponent<StatHandler>();

        if (weaponPrefab != null)
            weaponHandler = Instantiate(weaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDir);
        HandleAttackDelay();
    }
    protected virtual void FixedUpdate()
    {
        Movement(moveDir);
        if (knockbackDuration > 0f)
            knockbackDuration -= Time.deltaTime;
    }

    protected virtual void HandleAction()
    {

    }

    protected virtual void Movement(Vector2 dir)
    {
        dir *= statHanddler.Speed;
        if (knockbackDuration > 0f)
        {
            dir *= 0.2f;
            dir += knockback;
        }

        _rig.velocity = dir;
        animHandller.Move(dir);
    }

    void Rotate(Vector2 dir)
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bool isL = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isL;

        if (weaponPivot != null)
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);

        weaponHandler?.Rotate(isL);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (lookDir != Vector2.zero)
            weaponHandler.Attack();
    }

    public virtual void Death()
    {
        _rig.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
            component.enabled = false;

        Destroy(gameObject, 2f);
    }
}
