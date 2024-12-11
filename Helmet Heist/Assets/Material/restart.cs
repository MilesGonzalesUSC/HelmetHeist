using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject gameOverUI; // 失败界面
    public Vector3 startPosition; // 玩家初始位置

    private void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // 确保失败界面默认隐藏
        }

        // 记录玩家的初始位置
        startPosition = transform.position;
    }

    public void Damage()
    {
        // 假设玩家死亡逻辑在这里处理
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // 显示失败界面
            Time.timeScale = 0f; // 暂停游戏
        }
    }

    public void RestartGame()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // 隐藏失败界面
        }

        // 重置玩家位置
        transform.position = startPosition;

        // 恢复游戏时间和状态
        Time.timeScale = 1f;

        // 如果需要，重置其他游戏状态
        Debug.Log("Game restarted!");
    }
}
