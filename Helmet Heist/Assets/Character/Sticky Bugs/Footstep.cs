using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource; // ��Ч������
    public AudioClip footstepClip; // �Ų�����Ч����������ѭ����
    public float stepInterval = 0.5f; // ���νŲ���֮���ʱ����

    private bool isMoving; // ����Ƿ����ƶ�

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }

        // ȷ�� AudioSource �����Զ�����
        audioSource.loop = true; // ����Ϊѭ������
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
        // �������Ƿ����ƶ�����
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void PlayFootstep()
    {
        if (footstepClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = footstepClip;
            audioSource.Play(); // ��ʼѭ�����ŽŲ�����Ч
        }
    }

    private void StopFootstep()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // ֹͣ����
        }
    }
}
