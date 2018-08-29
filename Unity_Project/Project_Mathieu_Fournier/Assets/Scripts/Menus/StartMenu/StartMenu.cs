using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	

	private void Awake()
	{
		for (int i = 1; i < m_StartTexts.Count; i++)
		{
			m_StartTexts[i].GetComponent<Animator>().enabled = false;
			m_StartImage[i].enabled = false;
		}
	}

	void Update () 
	{
		m_StartImage[0].enabled = m_PlayerOneReady && Input.GetButton("Action_PlayerOne") ? true : false;
		m_StartImage[1].enabled = m_PlayerTwoReady && Input.GetButton("Action_PlayerTwo") ? true : false;
		m_StartImage[2].enabled = m_PlayerThreeReady && Input.GetButton("Action_PlayerThree") ? true : false;
		m_StartImage[3].enabled = m_PlayerFourReady && Input.GetButton("Action_PlayerFour") ? true : false;

		if(!m_PlayerFourReady)
		{
			if(Input.GetButtonDown("Action_PlayerOne"))
			{
				m_PlayerOneReady = true;
				m_StartTexts[0].text = "Ready!";
				m_StartTexts[0].GetComponent<Animator>().enabled = false;
				m_StartTexts[1].GetComponent<Animator>().enabled = true;	
			}
			
			if(m_PlayerOneReady && Input.GetButtonDown("Action_PlayerTwo"))
			{			
				m_PlayerTwoReady = true;
				m_StartTexts[1].text = "Ready!";	
				m_StartTexts[1].GetComponent<Animator>().enabled = false;
				m_StartTexts[2].GetComponent<Animator>().enabled = true;							
			}
			
			if(m_PlayerTwoReady && Input.GetButtonDown("Action_PlayerThree"))
			{							
				m_PlayerThreeReady = true;
				m_StartTexts[2].text = "Ready!";
				m_StartTexts[2].GetComponent<Animator>().enabled = false;
				m_StartTexts[3].GetComponent<Animator>().enabled = true;	
			}
			
			if(m_PlayerThreeReady && Input.GetButtonDown("Action_PlayerFour"))
			{											
				m_PlayerFourReady = true;
				m_StartTexts[3].text = "Ready!";
				m_StartTexts[3].GetComponent<Animator>().enabled = false;
			}

			if(m_PlayerFourReady)
			{
				LevelManager.Instance.ChangeScene(EScenes.MainMenu);
			}
		}
	}
}