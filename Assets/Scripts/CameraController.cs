using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera MainCamera;  // 主摄像机
    public float FixedWidth = 1600f; // 固定宽度

    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        MainCamera.orthographicSize = FixedWidth / (2f * screenRatio);
    }
}