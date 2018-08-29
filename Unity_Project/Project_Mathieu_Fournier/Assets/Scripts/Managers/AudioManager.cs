using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager Instance { get; private set; }

	private void Awake()
	{
		if(AudioManager.Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			AudioManager.Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
}