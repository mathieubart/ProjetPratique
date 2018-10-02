using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
	protected PowerupType m_Type;
    public float m_EffectDuration = 3f;
    private float m_LifeSpan = 0f;

    protected GameObject m_EffectFeedback;

    private void Start()
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
}
