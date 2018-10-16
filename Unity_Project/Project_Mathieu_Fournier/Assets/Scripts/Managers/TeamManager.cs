using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team
{
	public bool IsRunner;
	public float GameScore; 
	public int LevelScore;
	public Player Player01;
	public GameObject Runner;
	public Player Player02;
	public GameObject Grabber;
}

public class TeamManager : MonoBehaviour 
{
    [SerializeField]
    private List<Player> m_PlayerPrefabs;
	private List<Team> m_Teams = new List<Team>();

	public Runner Runner(int a_ID)
	{
		switch (a_ID)
		{
			case 1:
			case 3:
			{
				return m_Teams[0].Runner.GetComponentInChildren<Runner>();
			}
			case 2:
			case 4:
			{
				return m_Teams[1].Runner.GetComponentInChildren<Runner>();
			}
		}
		return null;
	}

/* 
	public Grabber Grabber(int a_ID)
	{
		return m_Grabbers[a_ID].GetComponent<Grabber>();
	}
	*/

	private List<Vector3> m_SpawnPositions = new List<Vector3>();

	private static TeamManager m_Instance;
	public static TeamManager Instance
	{
		get { return m_Instance; }
	}

	private void Awake()
	{
		if(m_Instance != null)
		{
			Destroy(this);
		}
		else
		{
			m_Instance = this;
		}
		DontDestroyOnLoad(gameObject);	
	}

	private void Start()
	{
		m_Teams.Add(new Team());
		m_Teams[0].Player01 = m_PlayerPrefabs[0];
		m_Teams[0].Player02 = m_PlayerPrefabs[2];

		m_Teams.Add(new Team());		
		m_Teams[1].Player01 = m_PlayerPrefabs[1];
		m_Teams[1].Player02 = m_PlayerPrefabs[3];
	}

	//Assign a random character to the teams players.
	public void SetRandomCharacters()
	{
		bool runnerIsActive = Random.Range(0, 2) == 0;
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].IsRunner = runnerIsActive;

			runnerIsActive = !runnerIsActive;
		}
	}

	//Activate/Desactivate The Characters to Change The Current Players Characters.  Player01 /02 /03 /04 : Flee <--> Grabber.
	public void SwitchCharacters()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].IsRunner = !m_Teams[i].IsRunner;
		}
	}

	//instantiate all characters in the scene
	public void CreateCharacters()
	{
		int spawnIndex = 0;
		Vector3 spawnPos = Vector3.zero;

		for(int i = 0; i < m_Teams.Count; i++)
		{
			spawnPos = GetSpawnPos(spawnIndex);
			spawnIndex++;

			if(m_Teams[i].IsRunner)
			{
				m_Teams[i].Runner = Instantiate(m_Teams[i].Player01.Runner, spawnPos, Quaternion.identity);
				m_Teams[i].Runner.GetComponentInChildren<Character>().SetID((PlayerID)m_Teams[i].Player01.ID);

				spawnPos = GetSpawnPos(spawnIndex);
				spawnIndex++;

				m_Teams[i].Grabber = Instantiate(m_Teams[i].Player02.Grabber, spawnPos, Quaternion.identity);
				m_Teams[i].Grabber.GetComponentInChildren<Character>().SetID((PlayerID)m_Teams[i].Player02.ID);		
			}
			else
			{
				m_Teams[i].Grabber = Instantiate(m_Teams[i].Player01.Grabber, spawnPos, Quaternion.identity);
				m_Teams[i].Grabber.GetComponentInChildren<Character>().SetID((PlayerID)m_Teams[i].Player01.ID);
							
				spawnPos = GetSpawnPos(spawnIndex);
				spawnIndex++;

				m_Teams[i].Runner = Instantiate(m_Teams[i].Player02.Runner, spawnPos, Quaternion.identity);
				m_Teams[i].Runner.GetComponentInChildren<Character>().SetID((PlayerID)m_Teams[i].Player02.ID);			
			}		
		}
	}

	public void SetSpawnPos(Vector3 a_SpawnPos)
	{
		m_SpawnPositions.Add(a_SpawnPos);
	}

	private Vector3 GetSpawnPos(int a_Index)
	{
		if(m_SpawnPositions.Count >= a_Index)
		{
			return m_SpawnPositions[a_Index];
		}
		return Vector3.zero;
	}

	public void ResetSpawnPos()
	{
		m_SpawnPositions.Clear();
	}

	//Destroy all characters in the scene
	public void DeleteCharacters()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			Destroy(m_Teams[i].Runner);
			Destroy(m_Teams[i].Grabber);
		}
		/* 
		for(int i = 0; i < m_Runners.Count; i++)
		{
			Destroy(m_Runners[i]);	
		}
		m_Runners.Clear();

		for(int i = 0; i < m_Grabbers.Count; i++)
		{
			Destroy(m_Grabbers[i]);	
		}
		m_Grabbers.Clear();
		*/
	}

	public float GetGameScore(int a_Team) 
	{
		return m_Teams[a_Team].GameScore;
	}

	public void ModifyGameScore(int a_Team, float a_Addition)
	{
		m_Teams[a_Team].GameScore += a_Addition;
	}

    public void ResetGameScore()
    {
        for (int i = 0; i < m_Teams.Count; i++)
        {
            m_Teams[i].GameScore = 0;
        }
    }

	public int GetLevelScore(int a_Team) 
	{		
		return m_Teams[a_Team].LevelScore;
	}

	public void ModifyLevelScore(int a_Team, int a_Addition)
	{
		m_Teams[a_Team].LevelScore += a_Addition;	
	}

	public void ResetLevelScores()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].LevelScore = 0;	
		}
	}

	public void AddTeam()
	{
		m_Teams.Add(new Team());
	}

	public Runner GetRunner(int a_ID)
	{
		switch (a_ID)
		{
			case 1:
			{
				return m_Teams[0].Player01.GetRunner();
			}			
			case 2:
			{
				return m_Teams[1].Player01.GetRunner();
			}
			case 3:
			{
				return m_Teams[0].Player02.GetRunner();
			}			
			case 4:
			{
				return m_Teams[1].Player02.GetRunner();
			}
			default:
			{
				return null;				
			}
		}
	}

	public void AssignPlayer() //Mathieu Fournier: Used to set Dynamically the players in one of the Screen Quadrant.
	{
		if(m_Teams[0].Player01 == null)
		{
			//Set The Player Control to The First Camera Quadrant.    Top-Left
		}
		else if(m_Teams[1].Player01 == null)
		{
			//Set The Player Control to The Second Camera Quadrant.   Top-Right
		}
		else if(m_Teams[0].Player02 == null)
		{
			//Set The Player Control to The Third Camera Quadrant.    Bottom-Left
		}
		else if(m_Teams[1].Player02 == null)
		{
			//Set The Player Control to The Fourth Camera Quadrant.   Bottom-Right
		}
	}
}