using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootEffect : BaseEffect
{
    [SerializeField]
    private float m_SpeedMultiplier = 1.5f;
    private Runner m_Runner;
    private float m_BaseRunnerSpeed;

    private GameObject m_EffectFeedback;

    private void Awake()
    {
        m_Runner = GetComponent<Runner>();
        m_EffectFeedback = m_Runner.m_BootsFeedback;
    }

    public override void PlayEffect()
    {
        if(m_Runner.m_HisHeld)
        {
            Grabber grabber = m_Runner.GetParent().GetComponent<Grabber>();
            grabber.Drop();
        }

        SetRunnerSpeed();
        AudioManager.Instance.PlaySFX(0, "Boots_Effect", transform.position);
    }

    protected override void StopEffect()
    {
        ResetRunnerSpeed();
        Destroy(this);
    }

    private void SetRunnerSpeed()
    {
        if (m_EffectFeedback.activeSelf) // Effect already started, Add Effect Duration.
        {
            BootEffect[] bootEffects = m_Runner.GetComponents<BootEffect>();

            for (int i = 0; i < bootEffects.Length; i++)
            {
                if (bootEffects[i] != this)
                {
                    bootEffects[i].m_EffectDuration += m_EffectDuration;
                    Destroy(this);
                }
            }
        }
        else // Start Effect
        {
            m_BaseRunnerSpeed = m_Runner.Speed;
            m_Runner.Speed = (m_BaseRunnerSpeed * m_SpeedMultiplier);
            m_EffectFeedback.SetActive(true);
        }
    }

    private void ResetRunnerSpeed()
    {
        m_EffectFeedback.SetActive(false);
        m_Runner.Speed = m_BaseRunnerSpeed;
    }
}
