using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private PlayerID m_ID;
	public int ID
	{
		get {return (int)m_ID;}
	}
	[SerializeField]
	private GameObject m_Runner;
	public GameObject Runner
	{
		get {return m_Runner;}
	}
	[SerializeField]
	private GameObject m_Grabber;
	public GameObject Grabber
	{
		get {return m_Grabber;}
	}

	public void SetActiveCharacter(bool a_IsRunnerActive)
	{
		m_Runner.SetActive(a_IsRunnerActive);
		m_Grabber.SetActive(!a_IsRunnerActive);
	}

	public void SwitchActiveCharacter()
	{
		m_Runner.SetActive(!m_Runner.activeSelf);
		m_Grabber.SetActive(!m_Grabber.activeSelf);			
	}

	public Runner GetRunner()
	{
		return m_Runner.GetComponent<Runner>();
	}
}