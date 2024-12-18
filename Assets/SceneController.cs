using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement; // 引用场景管理器

public class SceneController : MonoBehaviour
{
    // 函数用于处理按钮点击事件
    public void OnButtonClick() {
        // 获取点击按钮的名称
        string sceneName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        // 加载与按钮名称相同的场景
        SceneManager.LoadScene(sceneName);
    }
}