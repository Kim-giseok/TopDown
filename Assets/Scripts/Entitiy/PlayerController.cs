using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    GameManager gm;
    Camera mainCamera;
    // Start is called before the first frame update
    public void Init(GameManager gm)
    {
        this.gm = gm;
        mainCamera = Camera.main;
    }

    protected override void HandleAction()
    {

    }

    public override void Death()
    {
        base.Death();
        gm.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        moveDir = inputValue.Get<Vector2>();
        moveDir = moveDir.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePosition);
        lookDir = (worldPos - (Vector2)transform.position);

        if (lookDir.magnitude < .9f)
        {
            lookDir = Vector2.zero;
        }
        else
        {
            lookDir = lookDir.normalized;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        isAttacking = inputValue.isPressed;
    }

    public void UseItem(ItemData item)
    {
        foreach (StatEntry modifier in item.statModifiers)
        {
            statHandler.ModifyStat(modifier.statType, modifier.baseValue, !item.isTemporary, item.duration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ItemHandler>(out ItemHandler handler))
        {
            if (handler.ItemData == null)
                return;

            UseItem(handler.ItemData);
            Destroy(handler.gameObject);
        }
    }
}
