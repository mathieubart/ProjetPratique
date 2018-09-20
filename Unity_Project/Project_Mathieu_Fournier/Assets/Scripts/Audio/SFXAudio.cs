using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;

    private float m_LifeTime;
    private float m_Duration;

    public void SetUp(AudioClip i_Clip)
    {
        m_AudioSource.clip = i_Clip;
        m_Duration = i_Clip.length;
    }

    public void Play()
    {
        m_AudioSource.Play();
    }

    private void Update()
    {
        m_LifeTime += Time.deltaTime;
        if (m_LifeTime >= m_Duration)
        {
            Destroy(gameObject);
        }
    }

    public void SetSFXVolume(float a_VolumeMultiplier)
    {
        m_AudioSource.volume *= a_VolumeMultiplier;
    }
}
