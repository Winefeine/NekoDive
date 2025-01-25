using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float MoveSpeed;
    public float MaxSpeedModifier;
    public GameObject ObjectInBubble;
    public float Weight = 1f;
    
    [Header("Oxygen")]
    public Oxygen Oxygen;

    public float OxygenFrameProvide = 10f;
    //public float OxygenFrameProvide = 10f;

    // public bool IsConsuming;
    // public bool IsProviding;
    
    // public float OyxgenTank;
    // public float CurOxygen;
    // public float DefaultOxygenConsume = 0f; 
    // public float OxygenMoveIncrement = 1f;
    // public float OxygenProvide = -2f;
    [HideInInspector]
    public float OxygenIncrement;
    
    public float MinOxygen;     //Scale
    public float MaxOxygen;     //Scale
    public float MinScale;
    public float MaxScale;
    public float CurScale;
    


    public float DefaultGravity = -0.1f;
    public float CurGravity;
    
    [Header("Eject")]
    public float EjectForce;
    public float EjectCost;
    public GameObject ArrowPrefab;
    [Header("Inflate")]
    public float SpeedModifier;
    public bool IsInflated;
    public float InflateCost;

    public float HoverTime;
    
    public List<Vector3> Currents = new List<Vector3>();
    
 

    Rigidbody2D rb;
    PlayerInput input;
    PlayerController playerController;
    GameObject curArrow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
        Oxygen = GetComponentInChildren<Oxygen>();
        CurGravity = DefaultGravity;
        SetGravity(DefaultGravity);
    }

    void Update()
    {
        if(playerController != null)
        {
            Eject();
            Inflate();
        }
        if(IsInflated)
        {
            HandleMove();
        }
        if(ObjectInBubble != null)
        {
            ObjectInBubble.transform.position = transform.position;
        }

        rb.AddForce(SumCurrent(),ForceMode2D.Impulse);
    }
    
    void FixedUpdate()
    {
        if (rb != null)
        {
            // 检查当前速度是否超过最大速度
            if (rb.velocity.magnitude > MoveSpeed * MaxSpeedModifier)
            {
                // 限制速度到最大速度值
                rb.velocity = rb.velocity.normalized * MoveSpeed * MaxSpeedModifier;
            }
        }
    }

    void LateUpdate()
    {
        //Transfer Oxygen
        if(ObjectInBubble != null)
        {
            //Player
            if(ObjectInBubble.TryGetComponent<PlayerController>(out PlayerController p))
            {
                Oxygen oxy = p.Oxygen;
                if(oxy.CurOxygen < oxy.OxygenTank)
                {
                    //providing
                    this.Oxygen.OxygenFrameCost += OxygenFrameProvide;
                    oxy.OxygenFrameCost -= OxygenFrameProvide;
                }
            }
        }


        if(IsInflated && input.GetMoveInput())
        {
            //input
            playerController.PlayerAnimator.Play("Inflate Move");
        }

        BubbleUpdate();
    }

#region  Basic
    public void BubbleInit()
    {


    }
    public void HandleMove()
    {
        float horizontalInput = input.GetHorizontalMove();
        float horizontalMove = horizontalInput * SpeedModifier * MoveSpeed * Time.deltaTime;
        float verticalInput = input.GetVerticalMove();
        float verticalMove = verticalInput * SpeedModifier * MoveSpeed * Time.deltaTime;

        rb.velocity += new Vector2(horizontalMove,verticalMove);
        float vx = rb.velocity.x;
        float vy = rb.velocity.y;
        float maxSpeed = MoveSpeed * MaxSpeedModifier;
        vx = Mathf.Clamp(vx,-maxSpeed,maxSpeed);
        vy = Mathf.Clamp(vy,-maxSpeed,maxSpeed);
        rb.velocity = new Vector2(vx,vy);

        Vector3 newPosition = transform.position + new Vector3(horizontalMove,verticalMove,0f);
        transform.position = newPosition;

        AnimatorStateInfo stateInfo;
        stateInfo = playerController.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.normalizedTime < 1)
        {
            return;
        }
        if(horizontalInput != 0)
        {
            playerController.PlayerAnimator.Play("Inflate Move");
        }else
        {
            playerController.PlayerAnimator.Play("InflateIdle");
        }
    }

    public void BubbleUpdate()
    {
        //Scale
        if(Oxygen.CurOxygen <= MinOxygen)
        {
            CurScale = MinScale;
        }else if(Oxygen.CurOxygen >= MaxOxygen)
        {
            CurScale = MaxScale;
        }else
        {
            CurScale = MinScale + (MaxScale - MinScale) * (Oxygen.CurOxygen - MinOxygen) / (MaxOxygen - MinOxygen);
        }
        transform.localScale = new Vector3(CurScale,CurScale,1f);
    }


    public void SetGravity(float g)
    {
        CurGravity = g;
        rb.gravityScale = CurGravity;
    }

    public void BubbleDestroyed()
    {
        if(ObjectInBubble != null)
        {
            if(ObjectInBubble.GetComponent<PlayerController>())
            {
                PlayerController player = ObjectInBubble.GetComponent<PlayerController>();
                player.GetOutBubble();
            }
        }
        
        Destroy(this.gameObject);
    }

    public void DisableBubbleSprite()
    {

    }

    public void EnableBubbleSprite()
    {
        
    }

    IEnumerator DisableBubble(float time)
    {
        Physics2D.IgnoreCollision(playerController.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreCollision(playerController.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }
#endregion Basic

#region  Oxygen

#endregion Oxygen

#region  Ability
    public void Eject()
    {
        if(ObjectInBubble == null)
        {
            return;
        }
            
        if(input.GetEject() && curArrow == null)
        {
            //setArrow
            curArrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
        }

        if(input.GetEjectUp())
        {
            //TryShoot
            StartCoroutine(DisableBubble(1f));
            Vector2 dir = (Vector2)input.GetMoveDirection();
            if(dir == Vector2.zero)
            {
                dir = Vector2.right;
            }
            Vector3 force = dir * input.EjectChargePercent * EjectForce;
            playerController.GetOutBubble();
            playerController.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            

            if(curArrow != null)
            {
                Destroy(curArrow);
            }

            BubbleUpdate();
            OxygenIncrement = 0f;

            Oxygen.CostOxygen(EjectCost);
        }

    }

    public void Inflate()
    {
        if(input.GetInflateDown())
        {
            //startInflate
            //animation
            IsInflated = true;
            rb.gravityScale = 0f;
            playerController.CanMove = false;
            rb.velocity = Vector2.zero;

            playerController.PlayerAnimator.Play("Inflate");
        }
        if(input.GetInflate())
        {
            //
            playerController.Oxygen.CostOxygen(InflateCost);
        }
        if(input.GetInflateUp())
        {
            //EndInflate

            IsInflated = false;
            rb.gravityScale = DefaultGravity;
            playerController.PlayerAnimator.Play("PlayerBubbleIdle");
            //playerController.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
#endregion Ability

#region  Current
    public void AddCurrent(Vector3 vel)
    {
        Currents.Add(vel);
    }

    public void RemoveCurrent(Vector3 vel)
    {
        Currents.Clear();
    }

    public Vector3 SumCurrent()
    {
        Vector3 current = Vector3.zero;
        foreach(Vector3 cur in Currents)
        {
            current += cur;
        }

        return current;
    }

#endregion Current
    
    // public bool JudgeIsInBubble(Transform trans,float radius)
    // {
    //     float distance = Vector3.Distance(transform.position,trans.position);
    //     if(distance <= Radius - radius)
    //     {
    //         ObjectInBubble = trans.gameObject;
    //         return true;
    //     }
    //     return false;
    // }

    


}
