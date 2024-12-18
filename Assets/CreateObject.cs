using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CreateObject : MonoBehaviour
{
    public GameObject objectPrefab;  // 用于实例化的物体预制体
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    void Start() {
        // 获取左右手控制器
        InitializeControllers();
    }

    void Update() {
        // 检测左右手按键状态并生成物体
        CheckButtonAndGenerateObject(XRNode.LeftHand, ref leftHandDevice);
        CheckButtonAndGenerateObject(XRNode.RightHand, ref rightHandDevice);
    }

    // 初始化设备
    void InitializeControllers() {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0) {
            leftHandDevice = devices[0];
        }

        devices.Clear();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0) {
            rightHandDevice = devices[0];
        }
    }

    // 检测按钮按下并生成物体
    void CheckButtonAndGenerateObject(XRNode handNode, ref InputDevice handDevice) {
        if (!handDevice.isValid) {
            return; // 如果设备无效，跳过
        }

        // 检查是否按下按钮（例如 Grip 按钮）
        if (handDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed) {
            // 获取当前手的位置
            if (handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition)) {
                Debug.Log($"{handNode} Trigger pressed at position: {handPosition}");
                CreateObjectAtPosition(handPosition);
            }
        }
    }

    // 在指定位置创建物体
    void CreateObjectAtPosition(Vector3 position) {
        // 生成一个新的物体并设置其位置
        if (objectPrefab != null) {
            Instantiate(objectPrefab, position, Quaternion.identity);
        } else {
            Debug.LogWarning("Object prefab is not assigned!");
        }
    }
}
