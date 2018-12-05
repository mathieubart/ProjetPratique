using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InControl;

public class StartMenu : MonoBehaviour 
{
	private bool m_PlayerOneReady = false;
	private bool m_PlayerTwoReady = false;
	private bool m_PlayerThreeReady = false;
	private bool m_PlayerFourReady = false;

	[SerializeField]
	private List<Text> m_StartTexts = new List<Text>();
	[SerializeField]
	private List<Image> m_StartImage = new List<Image>();

    InputDevice m_LastDeviceUsed;
    private int m_ActivatedDevices = 0;

	private void Awake()
	{
        TeamManager.Instance.DeleteTeams();
        ControllerManager.Instance.ResetPlayerDevice();

        for (int i = 1; i < m_StartTexts.Count; i++)
		{
			m_StartTexts[i].GetComponent<Animator>().enabled = false;
			m_StartImage[i].enabled = false;
		}
	}

	void Update () 
	{
#if KEYBOARD_TEST

        m_StartImage[0].enabled = Input.GetKey(KeyCode.F);
        m_StartImage[1].enabled = Input.GetKey(KeyCode.G);
        m_StartImage[2].enabled = Input.GetKey(KeyCode.H);
        m_StartImage[3].enabled = Input.GetKey(KeyCode.J);

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_PlayerOneReady = true;
            m_StartTexts[0].text = "Ready!";
            m_StartTexts[0].GetComponent<Animator>().enabled = false;
            m_StartTexts[1].GetComponent<Animator>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.G) && m_PlayerOneReady)
        {
            m_PlayerTwoReady = true;
            m_StartTexts[1].text = "Ready!";
            m_StartTexts[1].GetComponent<Animator>().enabled = false;
            m_StartTexts[2].GetComponent<Animator>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.H) && m_PlayerTwoReady)
        {
            m_PlayerThreeReady = true;
            m_StartTexts[2].text = "Ready!";
            m_StartTexts[2].GetComponent<Animator>().enabled = false;
            m_StartTexts[3].GetComponent<Animator>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.J) && m_PlayerThreeReady)
        {
            m_PlayerFourReady = true;
            m_StartTexts[3].text = "Ready!";
            m_StartTexts[3].GetComponent<Animator>().enabled = false;
        }

        if (m_PlayerOneReady && m_PlayerTwoReady && m_PlayerThreeReady && m_PlayerFourReady)
        {
            LevelManager.Instance.ChangeScene(EScenes.MainMenu);
        }

#else

        InputDevice controller = InputManager.ActiveDevice;

        if (controller.GetControl(InputControlType.Action1).WasPressed && !ControllerManager.Instance.ContainDevice(controller))
        {
            switch (m_ActivatedDevices)
            {
                case 0:
                    {
                        ControllerManager.Instance.SetPlayerDevice(PlayerID.PlayerOne, controller);
                        m_PlayerOneReady = true;
                        m_StartTexts[0].text = "Ready!";
                        m_StartTexts[0].GetComponent<Animator>().enabled = false;
                        m_StartTexts[1].GetComponent<Animator>().enabled = true;
                        break;
                    }
                case 1:
                    {
                        ControllerManager.Instance.SetPlayerDevice(PlayerID.PlayerTwo, controller);
                        m_PlayerTwoReady = true;
                        m_StartTexts[1].text = "Ready!";
                        m_StartTexts[1].GetComponent<Animator>().enabled = false;
                        m_StartTexts[2].GetComponent<Animator>().enabled = true;
                        break;
                    }
                case 2:
                    {
                        ControllerManager.Instance.SetPlayerDevice(PlayerID.PlayerThree, controller);
                        m_PlayerThreeReady = true;
                        m_StartTexts[2].text = "Ready!";
                        m_StartTexts[2].GetComponent<Animator>().enabled = false;
                        m_StartTexts[3].GetComponent<Animator>().enabled = true;
                        break;
                    }
                case 3:
                    {
                        ControllerManager.Instance.SetPlayerDevice(PlayerID.PlayerFour, controller);
                        m_PlayerFourReady = true;
                        m_StartTexts[3].text = "Ready!";
                        m_StartTexts[3].GetComponent<Animator>().enabled = false;
                        break;
                    }
            }

            m_ActivatedDevices++;
        }
        else
        {
            if (ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerOne) != null)
            {
                m_StartImage[0].enabled = ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerOne).GetControl(InputControlType.Action1);
            }
            if (ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerTwo) != null)
            {
                m_StartImage[1].enabled = ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerTwo).GetControl(InputControlType.Action1);
            }
            if (ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerThree) != null)
            {
                m_StartImage[2].enabled = ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerThree).GetControl(InputControlType.Action1);
            }
            if (ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerFour) != null)
            {
                m_StartImage[3].enabled = ControllerManager.Instance.GetPlayerDevice(PlayerID.PlayerFour).GetControl(InputControlType.Action1);
            }
        }

        if (m_ActivatedDevices >= 4)
        {
            LevelManager.Instance.ChangeScene(EScenes.MainMenu);
        }
#endif
    }
}