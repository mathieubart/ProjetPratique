using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxophoneEffect : BaseEffect 
{
	private List<Grabber> m_Grabbers = new List<Grabber>();
	private List<Runner> m_Runners = new List<Runner>();
	private List<float> m_BaseGrabbersSpeed = new List<float>();
	private List<float> m_BaseRunnersSpeed = new List<float>();

	private void Awake()
	{
		m_Type = PowerupType.Saxophone;
		m_EffectFeedback = GetComponent<Runner>().m_MusicFeedback;
	}

	public override void PlayEffect()
	{
		SetGrabbersSpeedToZero();
		SetRunnersSpeedToZero();
		m_EffectFeedback.SetActive(true);
        AudioManager.Instance.PlaySFX(0, "Saxophone_Effect", transform.position);
	}

    protected override void StopEffect()
    {
        ResetCharactersSpeed();
        m_EffectFeedback.SetActive(false);
        Destroy(this);
    }

	private void SetGrabbersSpeedToZero()
	{
		RaycastHit[] spherecastHifos;

		spherecastHifos = Physics.SphereCastAll(transform.position, 5f, transform.position, 0f, LayerMask.GetMask("PlayerGrab"));

		if(spherecastHifos.Length != 0)
		{
			m_Grabbers.Clear();
			m_BaseGrabbersSpeed.Clear();
			for (int i = 0; i < spherecastHifos.Length; i++)
			{
				if(spherecastHifos[i].collider.GetComponent<Grabber>())
				{
					if(!m_Grabbers.Contains(spherecastHifos[i].collider.GetComponent<Grabber>()))
					{
						m_Grabbers.Add(spherecastHifos[i].collider.GetComponent<Grabber>());
						if(m_Grabbers[i].Speed != 0)
						{
							m_BaseGrabbersSpeed.Add(m_Grabbers[i].Speed);
							m_Grabbers[i].SetSpeed(0f);
                            m_Grabbers[i].StartDance();
						}
						else
						{
							GetComponent<SaxophoneEffect>().m_EffectDuration += m_EffectDuration;
							Destroy(this);
						}
					}
				}
			}
		}
	}

	private void SetRunnersSpeedToZero()
	{
		RaycastHit[] spherecastHifos;

		spherecastHifos = Physics.SphereCastAll(transform.position, 5f, transform.position, 0f, LayerMask.GetMask("PlayerFlee"));

		if(spherecastHifos.Length != 0)
		{
			m_Runners.Clear();
			m_BaseRunnersSpeed.Clear();
			for (int i = 0; i < spherecastHifos.Length; i++)
			{
				if(spherecastHifos[i].collider.GetComponent<Runner>()) //Does the sphereCast detected a Runner
				{
					if(!m_Runners.Contains(spherecastHifos[i].collider.GetComponent<Runner>())) // Is The Runner already Frozen
					{
						m_Runners.Add(spherecastHifos[i].collider.GetComponent<Runner>());

						if(m_Runners[i].Speed != 0) // Freeze
						{
							m_BaseRunnersSpeed.Add(m_Runners[i].Speed);

							if(m_Runners[i] != gameObject.GetComponent<Runner>()) // Am I the Target Of My Own Saxophone
							{
								m_Runners[i].SetSpeed(0f);
                                m_Runners[i].StartDance();
							}
						}
						else // Already Frozen, Add Effect duration
						{
							GetComponent<SaxophoneEffect>().m_EffectDuration += m_EffectDuration;
							Destroy(this);
						}
						
					}					
				}
			}
		}
	}

	private void ResetCharactersSpeed()
	{
		for (int i = 0; i < m_Grabbers.Count; i++)
		{
			m_Grabbers[i].SetSpeed(m_BaseGrabbersSpeed[i]);
            m_Grabbers[i].OnDanceEnd();
		}

		for (int i = 0; i < m_Runners.Count; i++)
		{
			m_Runners[i].SetSpeed(m_BaseRunnersSpeed[i]);
            m_Runners[i].OnDanceEnd();
		}	
	}
}