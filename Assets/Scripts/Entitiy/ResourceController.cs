using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] float damageDelay = 0.5f;

    BaseController baseController;
    StatHandler statHandler;
    AnimationHandler animationHanddler;

    float timeSinceLastChange = float.MaxValue;

    public float CurHealth { get; private set; }
    public float MaxHealth => statHandler.GetStat(StatType.Health);
    public AudioClip damageClip;

    private Action<float, float> OnChangeHealth;

    void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHanddler = GetComponent<AnimationHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurHealth = statHandler.GetStat(StatType.Health);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastChange < damageDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= damageDelay)
                animationHanddler.EndInvin();
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < damageDelay)
            return false;

        timeSinceLastChange = 0;
        CurHealth += change;
        CurHealth = CurHealth > MaxHealth ? MaxHealth : CurHealth;
        CurHealth = CurHealth < 0 ? 0 : CurHealth;

        OnChangeHealth?.Invoke(CurHealth, MaxHealth);

        if (change < 0)
        {
            animationHanddler.Damage();

            if (damageClip != null)
                SoundManager.PlayClip(damageClip);
        }
        if (CurHealth <= 0f)
            Death();

        return true;
    }

    void Death()
    {
        baseController.Death();
    }

    public void AddHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth += action;
    }

    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;
    }
}
