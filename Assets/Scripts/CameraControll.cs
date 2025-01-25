using UnityEngine;
using Cinemachine;

public class CameraControll : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 绑定的虚拟相机
    
    public float leftScreenX = 0.8f; // 玩家向左时的屏幕位置
    public float rightScreenX = 0.2f; // 玩家向右时的屏幕位置
    public float transitionSpeed = 5f; // 平滑过渡速度

    PlayerController player; // 玩家 Transform

    CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // 获取 Framing Transposer
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer == null)
        {
            Debug.LogError("Framing Transposer is missing on the virtual camera.");
        }
    }

    void Update()
    {
        if (framingTransposer == null || player == null) return;

        // 获取玩家移动方向
        float horizontalVelocity = player.GetComponent<Rigidbody2D>().velocity.x;

        // 根据方向调整 Screen X
        float targetScreenX = horizontalVelocity > 0 ? rightScreenX : (horizontalVelocity < 0 ? leftScreenX : framingTransposer.m_ScreenX);
        framingTransposer.m_ScreenX = Mathf.Lerp(framingTransposer.m_ScreenX, targetScreenX, Time.deltaTime * transitionSpeed);
    }
}
