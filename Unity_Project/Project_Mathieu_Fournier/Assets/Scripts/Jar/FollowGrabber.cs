using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGrabber : MonoBehaviour
{
	private Transform m_Parent;
	private Vector3 m_Offset = new Vector3(0f, 1.85f, 0f);

	private void Update () 
	{
		if(m_Parent != null)
		{
			transform.position = m_Parent.position + m_Offset;
			transform.rotation = m_Parent.rotation;
		}
	}

	public void SetParent(Transform a_Parent)
	{
		m_Parent = a_Parent;
	}
}
