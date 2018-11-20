using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutodestructionTimer : MonoBehaviour
{
    [SerializeField]
    private float m_DestructionTime = -1f;
    private float m_CurrentTime = 0f;


	public void Init (float a_AutodestructionTime)
    {
        m_DestructionTime = a_AutodestructionTime;
	}
	
	private void Update ()
    {
        m_CurrentTime += Time.deltaTime;
        if(m_DestructionTime < 0f)
        {
            Debug.LogError("You Must Give A Destruction Time To The AutodestructionTimer Or It Wont Know The Time To Wait Before Destruction. Mathf");
            Destroy(gameObject);
        }
        else if(m_CurrentTime >= m_DestructionTime)
        {
            Destroy(gameObject);
        }
	}
}
