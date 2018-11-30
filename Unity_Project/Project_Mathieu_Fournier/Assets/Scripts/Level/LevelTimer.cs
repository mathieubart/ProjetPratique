using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour 
{
	[SerializeField]
	private TextMeshProUGUI m_LevelTimeText;

#if CHEATS_ACTIVATED
    [SerializeField]
    private TextMeshProUGUI m_CheatText;

    private bool m_IsTimePaused;
    private float m_PausedTime;
#endif

    [SerializeField]
	private float m_LevelTime;
	private float m_TimeRemaining;
    private bool m_IsStarted = false;

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

#if CHEATS_ACTIVATED
        if (CheatManager.Instance)
        {
            CheatManager.Instance.m_UIText = m_CheatText;
            CheatManager.Instance.AddText("Press 0 to End the level. \n");
            CheatManager.Instance.AddText("Press 1 to Stop/Resume Level Timer. \n");
            CheatManager.Instance.AddText("Press 9 to Remove 20 seconds to the Level Timer. \n");
        }
#endif
	}

	private void Update()
	{
        if (m_IsStarted)
        {
            m_TimeRemaining -= Time.deltaTime;

            m_LevelTimeText.text = "Time Remaining" + ((int)m_TimeRemaining).ToString();
        }

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

#if CHEATS_ACTIVATED
        Cheats();
#endif

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

    public void StartTimer()
    {
        m_IsStarted = true;
    }

#if CHEATS_ACTIVATED
    public void CheatEndTurn()
    {
        m_TimeRemaining = 0f;
    }

    private void Cheats()
    {
        //Stop/Repaly the Level Time
        if (Input.GetKeyDown(KeyCode.Alpha1) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            if (!m_IsTimePaused)
            {
                m_PausedTime = m_TimeRemaining;
                m_IsTimePaused = true;
            }
            else
            {
                m_IsTimePaused = false;
            }
        }

        //Lock the time if time paused
        if (m_IsTimePaused)
        {
            m_TimeRemaining = m_PausedTime;
        }

        //End the level
        if (Input.GetKeyDown(KeyCode.Alpha0) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            CheatEndTurn();
        }

        //End the level
        if (Input.GetKeyDown(KeyCode.Alpha9) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            m_TimeRemaining -= 20f;
        }
    }
#endif
}
