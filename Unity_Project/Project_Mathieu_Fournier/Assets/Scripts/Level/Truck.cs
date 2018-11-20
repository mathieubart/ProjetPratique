using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour 
{
	[SerializeField]
	[Range(0, 1)]
	private int m_TeamAssigned;
    public int GetTeamAssigned()
    {
        return m_TeamAssigned;
    }

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

    [SerializeField]
    private Transform m_DiamondPile;

    [SerializeField]
    private Vector3 m_DiamondPileGrowAmount = Vector3.up;
    private Vector3 m_DiamondPileBaseSize;

    private void Awake()
    {
        m_DiamondPile.localScale = new Vector3(m_DiamondPile.localScale.x, 0f, m_DiamondPile.localScale.z);
        m_DiamondPileBaseSize = m_DiamondPile.localScale;
        ResetDiamondPile();
    }

    public void CoinDeposit(CoinCase a_CoinCase)
    {
        int coins = a_CoinCase.Coins;

        if (coins > 0)
        {
            a_CoinCase.CoinDeposit();
            TeamManager.Instance.ModifyLevelScore(m_TeamAssigned, coins);
            GrowDiamondPileSizeBy(coins);
        }
    }

    public void GrowDiamondPileSizeBy(int a_Size)
    {
        m_DiamondPile.localScale += a_Size * m_DiamondPileGrowAmount;
    }

    private void ResetDiamondPile()
    {
        m_DiamondPile.localScale = m_DiamondPileBaseSize;
    }
}
