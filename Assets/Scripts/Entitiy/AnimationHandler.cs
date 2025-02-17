using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    static readonly int IsMoving = Animator.StringToHash("IsMove");
    static readonly int IsDamage = Animator.StringToHash("IsDamage");

    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        anim.SetBool(IsMoving, obj.magnitude > .5f);
    }

    public void Damage()
    {
        anim.SetBool(IsDamage, true);
    }

    public void EndInvin()
    {
        anim.SetBool(IsDamage, false);
    }
}
