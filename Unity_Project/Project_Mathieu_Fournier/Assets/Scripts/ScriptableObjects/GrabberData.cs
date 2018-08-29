using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new grabber data", menuName = "ScriptableObjects/Grabber Data", order = 3)]
public class GrabberData : ScriptableObject
{
	[Tooltip("Angle in degrees between the forward and the wanted throw angle. Starting at the player forward going up.")]
	[SerializeField]
    private float m_ThrowAngle;
	public float ThrowAngle 
	{
		get { return m_ThrowAngle; }
	}

	[SerializeField]
    private float m_ThrowForce;
	public float ThrowForce 
	{
		get { return m_ThrowForce; }
	}
}
