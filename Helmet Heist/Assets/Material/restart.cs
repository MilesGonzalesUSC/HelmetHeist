using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject gameOverUI; // ʧ�ܽ���
    public Vector3 startPosition; // ��ҳ�ʼλ��

    private void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // ȷ��ʧ�ܽ���Ĭ������
        }

        // ��¼��ҵĳ�ʼλ��
        startPosition = transform.position;
    }

    public void Damage()
    {
        // ������������߼������ﴦ��
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // ��ʾʧ�ܽ���
            Time.timeScale = 0f; // ��ͣ��Ϸ
        }
    }

    public void RestartGame()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // ����ʧ�ܽ���
        }

        // �������λ��
        transform.position = startPosition;

        // �ָ���Ϸʱ���״̬
        Time.timeScale = 1f;

        // �����Ҫ������������Ϸ״̬
        Debug.Log("Game restarted!");
    }
}
