using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(!playerController.IsGrounded)
            {
                playerController.PlayerAnimator.Play("Land");
            }
            playerController.IsGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerController.IsGrounded = false;
        }
    }

    
}
