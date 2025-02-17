using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Infp")]
    [SerializeField] float delay = 1f;
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] float power = 1f;
    public float Power { get => power; set => power = value; }

    [SerializeField] float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] bool onKnocback = false;
    public bool OnKnocback { get => onKnocback; set => onKnocback = value; }

    [SerializeField] float knocbackPower = 0.1f;
    public float KnockbackPower { get => knocbackPower; set => knocbackPower = value; }

    [SerializeField] float knocbackTime = 0.5f;
    public float KnockbackTime { get => knocbackTime; set => knocbackTime = value; }

    static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public BaseController Controller { get; private set; }
    Animator anim;
    SpriteRenderer spRenderer;

    public AudioClip attackSound;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        anim = GetComponentInChildren<Animator>();
        spRenderer = GetComponentInChildren<SpriteRenderer>();

        anim.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimation();

        if (attackSound != null)
            SoundManager.PlayClip(attackSound);
    }

    public void AttackAnimation()
    {
        anim.SetTrigger(IsAttack);
    }

    public virtual void Rotate(bool isL)
    {
        spRenderer.flipY = isL;
    }
}
