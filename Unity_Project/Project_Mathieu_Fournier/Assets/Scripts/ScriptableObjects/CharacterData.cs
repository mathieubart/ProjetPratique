using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID
{
	PlayerOne = 1,
	PlayerTwo = 2,
	PlayerThree = 3,
	PlayerFour = 4,
}

[CreateAssetMenu(fileName = "new character data", menuName = "ScriptableObjects/Character Data", order = 2)]
public class CharacterData : ScriptableObject
{
	[SerializeField]
    private float m_Speed = 10f;
	public float Speed
	{ 
		get { return m_Speed; }
	}

    [SerializeField]
    private float m_RotationSpeed = 10f; 
	public float RotationSpeed
	{ 
		get { return m_RotationSpeed; }
	}

    [SerializeField]
    private float m_Sensitivity = 40f; 
	public float Sensitivity
	{ 
		get { return m_Sensitivity; }
	}
}
