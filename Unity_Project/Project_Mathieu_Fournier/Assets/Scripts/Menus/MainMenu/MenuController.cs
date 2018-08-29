using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
	[SerializeField]
	private float m_WinningGameScore = 50f;
	[SerializeField]
	private Slider m_ScoreSliderTeam01;
	[SerializeField]
	private Slider m_ScoreSliderTeam02;

	[SerializeField]
	private TextMeshProUGUI m_PressStartText;

	[SerializeField]
	private float m_DistributionTime = 5f;
	[SerializeField]
	private ParticleSystem m_ParticleST01; //Particle System Team 01.
	[SerializeField]
	private ParticleSystem m_ParticleST02; //Particle System Team 02.

	[SerializeField]
	private GameObject m_WinTeam01;
	[SerializeField]
	private GameObject m_WinTeam02;
	[SerializeField]
	private GameObject m_LooseTeam01;
	[SerializeField]
	private GameObject m_LooseTeam02;

	private Coroutine m_DistributionRoutine;

	private List<int> m_LevelScores = new List<int>();


	private void Awake()
	{
		m_PressStartText.enabled = false;
		m_WinTeam01.SetActive(false);
		m_WinTeam02.SetActive(false);
		m_LooseTeam01.SetActive(false);
		m_LooseTeam02.SetActive(false);
		m_ParticleST01.Stop();
		m_ParticleST02.Stop();
		m_ScoreSliderTeam01.maxValue = m_WinningGameScore;
		m_ScoreSliderTeam02.maxValue = m_WinningGameScore;
	}

	private void Start()
	{
		GetTeamsPoint();
		m_ScoreSliderTeam01.value = TeamManager.Instance.GetGameScore(0);
		m_ScoreSliderTeam02.value = TeamManager.Instance.GetGameScore(1);

		if(m_LevelScores[0] == 0 && m_LevelScores[1] == 0)
		{
			ShowText();
		}
		else
		{		
			TeamManager.Instance.ResetLevelScores();
			m_DistributionRoutine = StartCoroutine(DistributePoints());
		}
	}

	private void Update()
	{
		if(m_PressStartText.enabled && Input.GetButtonDown("Action_PlayerOne"))
		{
			LevelManager.Instance.ChangeScene(EScenes.Levels);
		}

		if(TeamManager.Instance.GetGameScore(0) >= m_WinningGameScore)
		{
			if(m_DistributionRoutine != null)
			{
				StopCoroutine(m_DistributionRoutine);
				m_DistributionRoutine = null;

				m_ParticleST01.Stop();
				m_ParticleST02.Stop();
			}
			m_WinTeam01.SetActive(true);
			m_LooseTeam02.SetActive(true);
		}	
		else if(TeamManager.Instance.GetGameScore(1) >= m_WinningGameScore)
		{
			if(m_DistributionRoutine != null)
			{
				StopCoroutine(m_DistributionRoutine);
				m_DistributionRoutine = null;

				m_ParticleST01.Stop();
				m_ParticleST02.Stop();
			}
			m_WinTeam02.SetActive(true);
			m_LooseTeam01.SetActive(true);			
		}
	}

	private void ShowText()
	{
		m_PressStartText.enabled = true;
	}

	private void GetTeamsPoint()
	{
		m_LevelScores.Add(TeamManager.Instance.GetLevelScore(0));
		m_LevelScores.Add(TeamManager.Instance.GetLevelScore(1));
	}

	private IEnumerator DistributePoints()
	{
		//Value used to Lerp and Stop the SFX of the Teams at the End of the Distribution Time.
		float team01Value = m_LevelScores[0]; 
		float team02Value = m_LevelScores[1]; 
		float highestScore = team01Value >= team02Value ? team01Value : team02Value;

		//Change The Teams Value To a 0 -> 1 base. 1 = the highestScore.
		if(team01Value != 0 && team02Value != 0)
		{
			team01Value = ((highestScore - team01Value) / highestScore);
			team02Value = ((highestScore - team02Value) / highestScore);
		}
		else
		{
			team01Value = team01Value == 0 ? 1 : 0;
			team02Value = team02Value == 0 ? 1 : 0;
		}

		yield return new WaitForSeconds(1f); //Delay Before The Distribution Start.

		m_ParticleST01.Play();
		m_ParticleST02.Play();

		while(team01Value <= 1 || team02Value <= 1)
		{
			if(team01Value <= 1f)
			{
				TeamManager.Instance.ModifyGameScore(0, (Time.deltaTime / m_DistributionTime) * highestScore);
				team01Value += Time.deltaTime / m_DistributionTime;
			}
			else
			{
				m_ParticleST01.Stop();
			}

			if(team02Value <= 1f)
			{
				TeamManager.Instance.ModifyGameScore(1, (Time.deltaTime / m_DistributionTime) * highestScore);				
				team02Value += Time.deltaTime / m_DistributionTime;
			}
			else
			{
				m_ParticleST02.Stop();
			}

			m_ScoreSliderTeam01.value = TeamManager.Instance.GetGameScore(0);
			m_ScoreSliderTeam02.value = TeamManager.Instance.GetGameScore(1);

			yield return null;
		}

		m_ParticleST01.Stop();
		m_ParticleST02.Stop();

		ShowText();
		TeamManager.Instance.ResetLevelScores();
		m_DistributionRoutine = null;
	}
}
