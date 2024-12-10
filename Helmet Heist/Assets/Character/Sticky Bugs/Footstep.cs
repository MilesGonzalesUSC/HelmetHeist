using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource; // 音效播放器
    public AudioClip footstepClip; // 脚步声音效（单个，可循环）
    public float stepInterval = 0.5f; // 两次脚步声之间的时间间隔

    private bool isMoving; // 玩家是否在移动

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }

        // 确保 AudioSource 不会自动播放
        audioSource.loop = true; // 设置为循环播放
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (IsPlayerMoving())
        {
            if (!isMoving)
            {
                isMoving = true;
                PlayFootstep();
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                StopFootstep();
            }
        }
    }

    private bool IsPlayerMoving()
    {
        // 检测玩家是否按下移动按键
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void PlayFootstep()
    {
        if (footstepClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = footstepClip;
            audioSource.Play(); // 开始循环播放脚步声音效
        }
    }

    private void StopFootstep()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // 停止播放
        }
    }
}
