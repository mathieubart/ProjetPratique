using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarStunEffect : MonoBehaviour 
{
	private float m_EffectDuration = 3f;
	private float m_BaseSpeed;
	private Grabber m_PlayerGrab;

	private void Awake()
	{
		if(gameObject.GetComponent<JarStunEffect>() != null)
		{
			if(gameObject.GetComponent<JarStunEffect>() != this)
			{
				Destroy(this);
			}
		}
	}

	private void Start()
	{
		m_PlayerGrab = gameObject.GetComponent<Grabber>();

		if(m_PlayerGrab != null)
		{
			m_BaseSpeed = m_PlayerGrab.Speed;
			m_PlayerGrab.Speed = 0f; 
			StartCoroutine("EffectTimer");
		}
		else
		{
			Destroy(this);
		}
	}

	private IEnumerator EffectTimer()
	{
		yield return new WaitForSeconds(m_EffectDuration);
		m_PlayerGrab.Speed = m_BaseSpeed;
        m_PlayerGrab.OnStunnedEnd();
		yield return new WaitForSeconds(0.1f);
		Destroy(this);	
	}
}
