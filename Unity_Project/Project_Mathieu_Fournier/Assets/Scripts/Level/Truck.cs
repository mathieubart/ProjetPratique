using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour 
{
	[SerializeField]
	[Range(0, 1)]
	private int m_TeamAssigned;

	[SerializeField]
	private Transform m_Spawn01;
	public Vector3 GetSpawn01Pos()
	{
		return m_Spawn01.position;
	}

	[SerializeField]
	private Transform m_Spawn02;
	public Vector3 GetSpawn02Pos()
	{
		return m_Spawn02.position;
	}

	private void OnTriggerEnter(Collider aCol)
	{	
		if(aCol.name == "Runner" )
		{
			int points = aCol.GetComponent<Runner>().GetPoints();
			TeamManager.Instance.ModifyLevelScore(m_TeamAssigned, points);
			aCol.GetComponent<Runner>().ResetBag();
		}
		else if(aCol.transform.tag == "Jar" && aCol.GetComponent<Jar>().m_IsHiddingThePlayer)
		{
			int points = aCol.GetComponent<Jar>().m_PlayerHidden.GetPoints();
			TeamManager.Instance.ModifyLevelScore(m_TeamAssigned, points);			
		}
	}
}
