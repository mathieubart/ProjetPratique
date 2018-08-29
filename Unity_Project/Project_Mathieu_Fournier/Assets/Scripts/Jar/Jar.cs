using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour 
{
	[HideInInspector]
	public bool m_IsHiddingThePlayer = false;
	[HideInInspector]
	public Runner m_PlayerHidden;

	public void OnHold(Transform a_Parent)
	{
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("HeldJar");		
		gameObject.AddComponent<FollowGrabber>();
		GetComponent<FollowGrabber>().SetParent(a_Parent);
	}

	public void OnRelease()
	{
		GetComponent<Rigidbody>().isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Jar");
		Destroy(gameObject.GetComponent<FollowGrabber>());
	}
}
