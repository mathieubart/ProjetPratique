using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    public float m_EffectDuration = 3f;
    private float m_LifeSpan = 0f;

    protected virtual void Start()
    {
        PlayEffect();
    }

    private void Update()
    {
        if(m_LifeSpan > m_EffectDuration)
        {
            StopEffect();
            Destroy(this);
        }

        m_LifeSpan += Time.deltaTime;
    }

	public virtual void PlayEffect()
	{

	}

    protected virtual void StopEffect()
    {

    }

    public void AddTime(float a_TimeToAdd)
    {
        m_EffectDuration += a_TimeToAdd;
    }
}
