using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    [SerializeField]
    private int m_TargetMashCount = 30;
    private int m_TeamOneMashCount = 0;
    private int m_TeamTwoMashCount = 0;

    [SerializeField]
    private GameObject m_MiniGamePanel;

    [SerializeField]
    private LevelTimer m_LevelTimer;

    [Header("Team 1 References")]
    [SerializeField]
    private Slider m_TeamOneSlider;
    [SerializeField]
    private BreakableWall m_TeamOneBreakableWall;

    [Header("Team 1 References")]
    [SerializeField]
    private Slider m_TeamTwoSlider;
    [SerializeField]
    private BreakableWall m_TeamTwoBreakableWall;

	private void Update ()
    {
#if KEYBOARD_TEST
        if(Input.GetKeyDown(KeyCode.F))
        {
            AddMashCount(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddMashCount(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddMashCount(0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddMashCount(1);
        }
#else
        //Get Team One Action Input
        if (m_TeamOneMashCount < m_TargetMashCount
        && ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerOne).GetControl(InputControlType.Action1).WasPressed)
        {
            AddMashCount(0);
        }
        if (m_TeamOneMashCount < m_TargetMashCount
        && ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerThree).GetControl(InputControlType.Action1).WasPressed)
        {
            AddMashCount(0);
        }

        //Get Team Two Action Input
        if (m_TeamTwoMashCount < m_TargetMashCount
        && ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerTwo).GetControl(InputControlType.Action1).WasPressed)
        {
            AddMashCount(1);
        }
        if (m_TeamTwoMashCount < m_TargetMashCount
        && ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerFour).GetControl(InputControlType.Action1).WasPressed)
        {
            AddMashCount(1);
        }
#endif

        if(m_TeamOneSlider == null && m_TeamTwoSlider == null)
        {
            if(AudioManager.Instance)
            {
                AudioManager.Instance.SetMusicVolume(0.5f);
            }

            Destroy(gameObject);
        }
    }
    
    private void AddMashCount(int a_Team)
    {
        if(AudioManager.Instance)
        {
            AudioManager.Instance.PlaySFX(0, "ButtonMash", transform.position);
        }

        if (a_Team == 0)
        {
            m_TeamOneMashCount++;
            if (m_TeamOneMashCount >= m_TargetMashCount)
            {
                ExplodeWall(0);
            }
            if (m_TeamOneSlider != null)
            {
                m_TeamOneSlider.value = (float)m_TeamOneMashCount / m_TargetMashCount;
                
            }
        }
        else if(a_Team == 1)
        {
            m_TeamTwoMashCount++;
            if (m_TeamTwoMashCount >= m_TargetMashCount)
            {
                ExplodeWall(1);
            }
            if (m_TeamTwoSlider != null)
            {
                m_TeamTwoSlider.value = (float)m_TeamTwoMashCount / m_TargetMashCount;
            }
        }
    }

    private void ExplodeWall(int a_Team)
    {
        if(m_MiniGamePanel != null)
        {
            Destroy(m_MiniGamePanel);
            m_LevelTimer.gameObject.SetActive(true);
            m_LevelTimer.StartTimer();
        }

        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySFX(0, "Explosion", transform.position);
        }

        if (a_Team == 0)
        {
            if(m_TeamOneBreakableWall != null)
            {
                m_TeamOneBreakableWall.BreakWall();
            }

            if(m_TeamOneSlider != null)
            {
                Destroy(m_TeamOneSlider.gameObject);
            }
        }
        else if(a_Team == 1)
        {
            if(m_TeamTwoBreakableWall != null)
            {
                m_TeamTwoBreakableWall.BreakWall();
            }
            if (m_TeamTwoSlider != null)
            {
                Destroy(m_TeamTwoSlider.gameObject);
            }
        }
    }
}
