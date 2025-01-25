using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BackGround : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 虚拟相机
    public Transform targetTransform;             // 目标 Transform
    public float moveRatio = 0.5f;                // 移动比例（0~1）

    private float previousCameraX;                // 记录上一帧的相机 X 轴位置
    private Vector3 initialTargetPosition;        // 目标物体的初始位置

    void Start()
    {
        targetTransform = this.transform;
        // 初始化变量
        previousCameraX = virtualCamera.transform.position.x;
        initialTargetPosition = targetTransform.position;
    }

    void LateUpdate()
    {
        if (virtualCamera == null || targetTransform == null) return;

        // 当前相机的 X 轴位置
        float currentCameraX = virtualCamera.transform.position.x;

        // 计算相机 X 轴的位移
        float deltaX = currentCameraX - previousCameraX;

        // 按比例移动目标 Transform
        targetTransform.position += new Vector3(deltaX * moveRatio, 0, 0);

        // 更新上一帧的相机 X 位置
        previousCameraX = currentCameraX;
    }
}
