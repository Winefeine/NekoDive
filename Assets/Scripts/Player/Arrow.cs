using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Distance = 2f;
    public float MaxScale = 1.5f;

    PlayerController playerController;
    BubbleController BubbleController;
    PlayerInput input;

    void Start()
    {
        input = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
        BubbleController = playerController.GetCurBubble();
    }

    void Update()
    {
        ArrowUpdate();
    }

    void ArrowUpdate()
    {
        Vector2 inputDir = new Vector2(input.GetHorizontalMove(),input.GetVerticalMove());
        if(inputDir == Vector2.zero)
        {
            inputDir = Vector2.right;
        }
        Vector3 offset = (Vector3)inputDir.normalized * Distance;
        transform.position = BubbleController.gameObject.transform.position + offset;

        float angle = Mathf.Atan2(inputDir.y,inputDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f,0f,angle - 90f + 49f);

        //scale
        float scale = MaxScale * input.EjectChargePercent;
        transform.localScale = new Vector3(scale,scale,1f);
    }
}
