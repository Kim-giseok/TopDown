using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    static BulletManager instance;
    public static BulletManager Instance { get => instance; }

    [SerializeField] GameObject[] bulletPrefebs;
    [SerializeField] ParticleSystem impactParticleSys;

    private void Awake()
    {
        instance = this;
    }

    public void ShotBullet(RangeWeaponHandler rwHandler, Vector2 startP, Vector2 dir)
    {
        GameObject origin = bulletPrefebs[rwHandler.BulletIdx];
        GameObject obj = Instantiate(origin, startP, Quaternion.identity);

        BulletController bulletController = obj.GetComponent<BulletController>();
        bulletController.Init(dir, rwHandler, this);
    }

    public void CreateImpactParticlesAtPostion(Vector3 position, RangeWeaponHandler weaponHandler)
    {
        impactParticleSys.transform.position = position;
        ParticleSystem.EmissionModule em = impactParticleSys.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));
        ParticleSystem.MainModule mainModule = impactParticleSys.main;
        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
        impactParticleSys.Play();
    }
}
