using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour 
{
	[SerializeField]
	private TextMeshProUGUI m_LevelTimeText;
	
	[SerializeField]
	private float m_LevelTime;
	private float m_TimeRemaining;

	private bool m_MenuIsLoaded = false;

	private void Awake()
	{
		m_TimeRemaining = m_LevelTime;
	}

	private void Update()
	{
		m_TimeRemaining -= Time.deltaTime;

		m_LevelTimeText.text = "Time Remaining" + ((int)m_TimeRemaining).ToString();

		if(!m_MenuIsLoaded && m_TimeRemaining <= 0f)
		{
			LevelManager.Instance.ChangeScene(EScenes.MainMenu);
			m_MenuIsLoaded = true;
		}
	}
}
