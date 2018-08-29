﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	[SerializeField]
	private float m_TransitionTime;
	[SerializeField]
	private Image m_SceneTransitionImage;

	[SerializeField]
	private float m_LoadingSceneTime;

	private static LevelManager m_Instance;
	public static LevelManager Instance
	{
		get { return m_Instance; }
	}

	private void Awake()
	{
		if(LevelManager.Instance == null)
		{
			m_Instance = this;
		}		
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		ChangeScene(EScenes.StartMenu);
	}

	public void ChangeScene(EScenes a_Scene)
	{
        if(SceneManager.GetActiveScene().buildIndex == (int)EScenes.StartMenu)
        {
            TeamManager.Instance.SetRandomCharacters();
        }
		else if(SceneManager.GetActiveScene().buildIndex == (int)EScenes.Levels)
		{
            TeamManager.Instance.SwitchCharacters();	
			TeamManager.Instance.DeleteCharacters();	
		}

		StartCoroutine(FadeInScenes(a_Scene));
	}

	private void OnLoadingDone(Scene a_Scene, LoadSceneMode i_Mode)
    {
        //On enleve la fct de la liste de fct appelees par l'event OnLoadingDone de Unity.
        SceneManager.sceneLoaded -= OnLoadingDone;

		StartCoroutine(FadeOutScenes());

        switch (a_Scene.buildIndex)
        {
            case (int)EScenes.StartMenu:
                {
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }            
			case (int)EScenes.MainMenu:
                {
					TeamManager.Instance.ResetSpawnPos();          
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }
			case (int)EScenes.Levels:
                {
					Truck[] trucks = FindObjectsOfType<Truck>();
					for(int i = 0; i < trucks.Length; i++)
					{
						TeamManager.Instance.SetSpawnPos(trucks[i].GetSpawn01Pos());
						TeamManager.Instance.SetSpawnPos(trucks[i].GetSpawn02Pos());
					}

					TeamManager.Instance.CreateCharacters();

					PlayerFleeUI[] UIs = FindObjectsOfType<PlayerFleeUI>();
					for(int i = 0; i < UIs.Length; i++)
					{
						UIs[i].Init();
					}
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }            
        }
    }

	private IEnumerator FadeInScenes(EScenes a_Scene)
	{
		float opacityValue = 0f;
		Color color = m_SceneTransitionImage.color;

		while(opacityValue < 1)
		{
			m_SceneTransitionImage.color = new Vector4(color.r, color.g, color.b, opacityValue);

			opacityValue +=  Time.deltaTime / m_TransitionTime;

			yield return null;
		}

		SceneManager.LoadScene((int)a_Scene);
		SceneManager.sceneLoaded += OnLoadingDone;
	}

	private IEnumerator FadeOutScenes()
	{
		float opacityValue = 0f;
		Color color = m_SceneTransitionImage.color;

		while(opacityValue < 1)
		{
			m_SceneTransitionImage.color = new Vector4(color.r, color.g, color.b, opacityValue);

			opacityValue +=  Time.deltaTime / m_TransitionTime;
			
			yield return null;
		}
	}
}
