using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    [SerializeField] LayerMask levelCollisionL;

    RangeWeaponHandler rwHandler;

    float currentDuration;
    Vector2 dir;
    bool isReady;
    Transform pivot;

    Rigidbody2D _rig;
    SpriteRenderer render;

    public bool fxOnDestroy = true;

    BulletManager bulletManager;

    private Action<GameObject> returnToPool;

    private void Awake()
    {
        render = GetComponentInChildren<SpriteRenderer>();
        _rig = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
            return;

        currentDuration += Time.deltaTime;

        if (currentDuration > rwHandler.Duration)
            DestroyBullet(transform.position, false);

        _rig.velocity = dir * rwHandler.Speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelCollisionL.value == (levelCollisionL.value | (1 << collision.gameObject.layer)))
        {
            DestroyBullet(collision.ClosestPoint(transform.position) - dir * 2f, fxOnDestroy);
        }
        else if (rwHandler.target.value == (rwHandler.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController rController = collision.GetComponent<ResourceController>();
            if(rController != null)
            {
                rController.ChangeHealth(-rwHandler.Power);
                if(rwHandler.OnKnocback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if (controller != null)
                        controller.ApplyKnockback(transform, rwHandler.KnockbackPower, rwHandler.KnockbackTime);
                }
            }

            DestroyBullet(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 dir, RangeWeaponHandler rwHandler, BulletManager bManager)
    {
        bulletManager = bManager;

        this.rwHandler = rwHandler;

        this.dir = dir;
        currentDuration = 0;
        transform.localScale = Vector3.one * rwHandler.BulletSize;
        render.color = rwHandler.BulletColor;

        transform.right = this.dir;

        if (dir.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    void DestroyBullet(Vector3 position, bool createFx)
    {
        if (createFx)
            bulletManager.CreateImpactParticlesAtPostion(position, rwHandler);

        //Destroy(gameObject);
        OnDespawn();
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }
}
