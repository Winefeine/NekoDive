using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator PlayerAnimator;
    [Header("Movement")]
    public bool CanMove;
    public float MoveSpeed = 100f;
    public float AccessModifier = 0.5f;
    public float MaxSpeedModifier = 2f;
    [Header("Jump")]
    public bool IsGrounded;
    public float JumpForce = 5f;
    public float JumpCost;
    public float DefaultGravity = -0.3f;
    
    [Header("Bubble")]
    public bool IsInsideBubble;     //trans
    public BubbleController curBubble;
    
    [Header("Oxygen")]
    public Oxygen Oxygen;
    public float OxygenMoveFrameCost = 5f;

    public bool IsDead;
    
    
    public GameObject GameOverPanel;
    public List<Vector3> Currents = new List<Vector3>();

    
    public float Weight = 2f;

    bool isFacingRight;
    bool isHovering;
    float hoverTime = 0f;
    PlayerInput input;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Collider2D collider2d;
    AnimatorStateInfo stateInfo;
    
    void Start()
    {
        input = GetComponent<PlayerInput>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();
        Oxygen = GetComponentInChildren<Oxygen>();
        PlayerAnimator = GetComponent<Animator>();

        CanMove = true;
    }

    void Update()
    {
        if(CanMove)
        {
            HandleMove();
        }
        if(isHovering)
        {
            hoverTime += Time.deltaTime;
            if(hoverTime > curBubble.HoverTime)
            {       
                //get in 
                if(!IsInsideBubble)
                {
                    GetInBubble(curBubble);
                }else
                {
                    //GetOutBubble(curBubble);
                }
                isHovering = false;
                
                
            }
        }
    }

    void LateUpdate()
    {
        

        
    }

#region  Basic
    void HandleMove()
    {
        float horizontalInput = input.GetHorizontalMove();
        float horizontalMove = horizontalInput * AccessModifier * MoveSpeed * Time.deltaTime;

        // float verticalInput = input.GetVerticalMove();
        // float verticalMove = verticalInput * SpeedModifier * MoveSpeed * Time.deltaTime;

        // Vector3 newPosition = transform.position + new Vector3(horizontalMove,0f,0f);
        // transform.position = newPosition;

        if(horizontalInput != 0)
        {
            Oxygen.OxygenFrameCost += OxygenMoveFrameCost;
        }


        rb.velocity += new Vector2(horizontalMove,0f);
        float vx = rb.velocity.x;
        float maxSpeed = MoveSpeed * MaxSpeedModifier;
        vx = Mathf.Clamp(vx,-maxSpeed,maxSpeed);
        rb.velocity = new Vector2(vx,rb.velocity.y);
        
        if(!IsGrounded)
        {
            //stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
            if(rb.velocity.y > 1.5f)
            {
                PlayerAnimator.Play("AirUp");
            }else if(rb.velocity.y < -1.5f)
            {
                PlayerAnimator.Play("AirDown");
            }else
            {
                PlayerAnimator.Play("In Air");
            }
        }else
        {
            if(horizontalMove != 0)
            {
                stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.normalizedTime >= 1)
                {
                    PlayerAnimator.Play("Move");
                }
            }else
            {
                stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.normalizedTime >= 1)
                {
                    PlayerAnimator.Play("Idle");
                }
            }

            //jump
            if(input.GetJumpDown() && IsGrounded)
            {
                StartCoroutine(Jump());
                // float t = 0.5f;
                // StartCoroutine(ProcessDelay(t));
                // rb.velocity = new Vector2(rb.velocity.x, 0); // 重置垂直速度
                // rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); // 施加跳跃力
                // CostOxygen(JumpCost);
                // PlayerAnimator.Play("Jump Start");
            }
        }


        
        
        //Current
        rb.AddForce(SumCurrent(),ForceMode2D.Impulse);
    }

    void UpdateFacing()
    {


    }

    public void Die()
    {
        IsDead = true;
        CanMove = false;
        GameOverPanel.SetActive(true);
        Debug.Log("似了！");
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        //EnterBubble
        if(col.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            isHovering = true;
            hoverTime = 0f;
            curBubble = col.gameObject.GetComponent<BubbleController>();
        }

    }

#endregion Basic

#region  Oxygen



#endregion Oxygen

#region  Bubble
    void GetInBubble(BubbleController bubble)
    {
        curBubble = bubble;
        Transform bTrans = curBubble.transform;
        bTrans.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = bTrans.position;
        transform.SetParent(bTrans);
        CanMove = false;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        IsInsideBubble = true;
        collider2d.enabled = false;
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        
        bubble.ObjectInBubble = this.gameObject;
        //bubble.SetGravity(0f);

        hoverTime = 0f;
        PlayerAnimator.Play("PlayerBubbleIdle");
    }
    
    public void GetOutBubble()
    {
        transform.SetParent(null);
        //TryGetOut();
        //EjectPlayer();
        if(curBubble != null)
        {
            
            curBubble.ObjectInBubble = null;
            curBubble.SetGravity(curBubble.DefaultGravity);
            curBubble = null;
        }

        CanMove = true;
        IsInsideBubble = false;
        
        rb.gravityScale = DefaultGravity;
        rb.velocity = Vector2.zero;
        collider2d.enabled = true;
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        isHovering = false;
        hoverTime = 0f;

        PlayerAnimator.Play("In Air");
        Debug.Log("Out");
    }

    public BubbleController GetCurBubble()
    {
        if(curBubble != null)
        {
            return curBubble;
        }

        return null;
    }

#endregion Bubble

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
    IEnumerator Jump()
    {
        PlayerAnimator.Play("Jump Start");
        stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitUntil(()=> stateInfo.normalizedTime >= 0.5f);
        rb.velocity = new Vector2(rb.velocity.x, 0); // 重置垂直速度
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); // 施加跳跃力
        Oxygen.CostOxygen(JumpCost);
    }

    // public void OnCollisionExit2D(Collision2D col)
    // {
    //     if(col.gameObject.layer == LayerMask.NameToLayer("Bubble"))
    //     {
    //         canMove = true;
    //         isHovering = false;
    //         hoverTime = 0f;
    //     }

    // }


    
    // IEnumerator GetOutBubble(Collider2D col)
    // {
    //     canMove = false;
    //     Physics2D.IgnoreCollision(collider2d, col, true);
    //     if(input.GetHorizontalMove() != 0 || input.GetVerticalMove()!= 0)
    //     {    
    //         rigid.velocity = curMoveDir * (SpeedModifier + GetOutAccessBuffer) * MoveSpeed;
    //     }else
    //     {
    //         rigid.velocity = curMoveDir * (SpeedModifier - GetOutAccessBuffer) * MoveSpeed;
    //     }
        
    //     // 等待指定时间
    //     yield return new WaitForSeconds(GetOutTime);

    //     // 恢复碰撞
    //     Physics2D.IgnoreCollision(collider2d, col, false);
    //     rigid.velocity = Vector2.zero;
    //     canMove = true;


    // }
    


    

    


}
