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

    [SerializeField]
    private float m_AlarmTime;

    [SerializeField]
    private float m_MaxAlarmScale;
    [SerializeField]
    private float m_AlarmSpeed;
    private bool m_AlarmStarted = false;
    private float m_CurrentScale = 1f;
    private Vector3 m_BaseScale;

    private bool m_MenuIsLoaded = false;

	private void Awake()
	{
		m_TimeRemaining = m_LevelTime;
        m_BaseScale = m_LevelTimeText.rectTransform.localScale;
	}

	private void Update()
	{
		m_TimeRemaining -= Time.deltaTime;

		m_LevelTimeText.text = "Time Remaining" + ((int)m_TimeRemaining).ToString();

        if(m_TimeRemaining <= m_AlarmTime && !m_AlarmStarted)
        {
            m_AlarmStarted = true;
            StartCoroutine(PlayAlarm());
        }

		if(!m_MenuIsLoaded && m_TimeRemaining <= 0f)
		{
			LevelManager.Instance.ChangeScene(EScenes.MainMenu);
			m_MenuIsLoaded = true;
		}
	}

    private IEnumerator PlayAlarm() //Scale up and down the text and set it red.
    {
        m_LevelTimeText.color = Color.red;

        while(true)
        {
            //Grow the text
            while(m_CurrentScale <= m_MaxAlarmScale)
            {
                m_CurrentScale += m_AlarmSpeed * Time.deltaTime * 0.5f;
                m_LevelTimeText.rectTransform.localScale = m_BaseScale * m_CurrentScale;
                yield return null;
            }

            //Shrink the text
            while (m_CurrentScale > 1f)
            {
                m_CurrentScale -= m_AlarmSpeed * Time.deltaTime * 0.5f;
                m_LevelTimeText.rectTransform.localScale = m_BaseScale * m_CurrentScale;
                yield return null;
            }
        }
    }
}
