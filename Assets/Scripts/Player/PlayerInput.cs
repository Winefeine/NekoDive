using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Eject")]
    public KeyCode EjectKey;
    public float MaxEjectChargeTime = 2f;
    public float MinChargePercent = 0.2f;
    public float EjectChargePercent;
    float ejectChargeTime = 0f;
    [Header("Inflate")]
    public KeyCode InflateKey;

    public bool canInput;

    SpriteRenderer sprite;
    

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        canInput = true;
        ejectChargeTime = 0f;
    }
    
    void Update()
    {
        if(Input.GetKey(EjectKey))
        {
            EjectChargePercent = 0f;
        }
        if(Input.GetKey(EjectKey))
        {
            ejectChargeTime += Time.deltaTime;
            EjectChargePercent = GetEjectCharge();
        }
        if(Input.GetKeyUp(EjectKey))
        {
            EjectChargePercent = GetEjectCharge();
            ejectChargeTime = 0f;
        }
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }


    }

#region Basic
    public bool GetMoveInput()
    {
        if(canInput)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                return true;
            }
        }
        return false;
    }
    public float GetHorizontalMove()
    {
        if (canInput)
        {
            float move = Input.GetAxisRaw("Horizontal");
            move = Math.Min(move, 1f);

            return move;
        }

        return 0f;
    }

    public float GetVerticalMove()
    {
        if(canInput)
        {
            float move = Input.GetAxisRaw("Vertical");
            move = Math.Min(move, 1f);

            return move;
        }

        return 0f;
    }

    public bool GetJumpDown()
    {
        if(canInput)
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    public bool GetEjectDown()
    {
        if(canInput)
        {
            return Input.GetKeyDown(EjectKey);
        }
        return false;
    }

    public bool GetEject()
    {
        if(canInput)
        {
            return Input.GetKey(EjectKey);
        }

        return false;
    }

    public bool GetEjectUp()
    {
        if(canInput)
        {
            return Input.GetKeyUp(EjectKey);
        }
        return false;
    }

    public float GetEjectCharge()
    {
        float percent = ejectChargeTime / MaxEjectChargeTime;
        percent = Mathf.Clamp(percent,MinChargePercent,1f);

        return percent;
    }

    public bool GetInflateDown()
    {
        if(canInput)
        {
            return Input.GetKeyDown(InflateKey);
        }
        return false;
    }

    public bool GetInflate()
    {
        if(canInput)
        {
            return Input.GetKey(InflateKey);
        }
        return false;
    }

    public bool GetInflateUp()
    {
        if(canInput)
        {
            return Input.GetKeyUp(InflateKey);
        }
        return false;
    }


    public Vector3 GetMoveDirection()
    {
        Vector3 dir =  new Vector3(GetHorizontalMove(),GetVerticalMove(),0f);
        return dir.normalized;
    }


    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

#endregion Basic


}
