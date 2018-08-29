using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour 
{

	public EPoolType m_PoolType;
	public int m_Quantity;

	private void OnEnable()
	{
		PoolManager.Instance.m_OnChangeScene += ReturnToPool;
	}

	private void OnDisable()
	{
		PoolManager.Instance.m_OnChangeScene -= ReturnToPool;

	}

	public void StartTimer(float a_Time)
	{
		StartCoroutine(WaitAndReturn(a_Time));
	}

	private IEnumerator WaitAndReturn(float a_Time)
	{
		yield return new WaitForSeconds(a_Time);

		ReturnToPool();
	}

	public void ReturnToPool()
	{
		StopAllCoroutines();
		PoolManager.Instance.ReturnToPool(m_PoolType, gameObject);
	}
}
