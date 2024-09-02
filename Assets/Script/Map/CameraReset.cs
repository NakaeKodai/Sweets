using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // カメラの初期回転を保存
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 毎フレーム、カメラの回転を初期回転にリセット
        transform.rotation = initialRotation;
    }
}
