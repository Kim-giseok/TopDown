using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] Transform bulletSpawnP;

    [SerializeField] int bulletIdx;
    public int BulletIdx { get => bulletIdx; }

    [SerializeField] float bulletSize = 1f;
    public float BulletSize { get => bulletSize; }

    [SerializeField] float duration;
    public float Duration { get => duration; }

    [SerializeField] float spread;
    public float Spread { get => spread; }

    [SerializeField] int bulletNumPerShot;
    public int BulletNumPerShot { get => bulletNumPerShot; }

    [SerializeField] float multiBulletAngle;
    public float MultiBulletAngle { get => multiBulletAngle; }

    [SerializeField] Color bulletColor;
    public Color BulletColor { get => bulletColor; }

    BulletManager bulletManager;

    private StatHandler statHandler;

    protected override void Start()
    {
        base.Start();
        bulletManager = BulletManager.Instance;
        statHandler = GetComponentInParent<StatHandler>();
    }

    public override void Attack()
    {
        base.Attack();

        float bulletSpace = multiBulletAngle;
        int bulletNumPerShot = this.bulletNumPerShot + (int)statHandler.GetStat(StatType.ProjectileCount);

        float minangle = -(bulletNumPerShot / 2f) * bulletSpace;

        for (int i = 0; i < bulletNumPerShot; i++)
        {
            float angle = minangle + bulletSpace * i;
            float randSpread = Random.Range(-spread, spread);
            angle += randSpread;
            CreateBullet(Controller.LookDir, angle);
        }
    }

    void CreateBullet(Vector2 _lookDir, float angle)
    {
        bulletManager.ShotBullet
        (
            this,
            bulletSpawnP.position,
            RotateVector2(_lookDir, angle)
        );
    }

    static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
