using UnityEngine;

public class SeaWeed : MonoBehaviour
{
    public float OxygenFrameProvide = 10f;


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = GameObject.FindObjectOfType<PlayerController>();
            player.PlayerAnimator.Play("SeaweedIdle");
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = GameObject.FindObjectOfType<PlayerController>();
            PlayerInput input = GameObject.FindObjectOfType<PlayerInput>();
            AnimatorStateInfo state = player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
            player.Oxygen.OxygenFrameCost -= OxygenFrameProvide;
            if(!player.IsGrounded)
            {
                return;
            }
            if(!input.GetMoveInput() && !state.IsName("SeaweedIdle"))
            {
                player.PlayerAnimator.Play("SeaweedIdle");
            }
        }

    }


}
