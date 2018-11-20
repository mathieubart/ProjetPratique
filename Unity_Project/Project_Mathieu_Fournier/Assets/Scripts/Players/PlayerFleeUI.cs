using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFleeUI : MonoBehaviour 
{
	[SerializeField]
	private PlayerID m_ID;
	[SerializeField]
	private TextMeshProUGUI m_PointTextMesh;
	[SerializeField]
	private GameObject m_UISax01;
	[SerializeField]
	private GameObject m_UISax02;

	[SerializeField]
	private GameObject m_UIBoot01;
	[SerializeField]
	private GameObject m_UIBoot02;
	
	private void Start()
	{
		m_UISax01.SetActive(false);
		m_UISax02.SetActive(false);
		m_UIBoot01.SetActive(false);
		m_UIBoot02.SetActive(false);


		SetText(0);
	}

	public void Init()
	{
	    if(TeamManager.Instance != null)
        {
            if(TeamManager.Instance.Runner((int)m_ID) != null)
            {
                Runner player;
                player = TeamManager.Instance.Runner((int)m_ID);
                if(player.GetComponentInChildren<CoinCase>())
                {
                    player.GetComponentInChildren<CoinCase>().OnPointChanged += SetText;
                }
				player.OnPowerupAdded += AddPowerup;
				player.OnPowerupRemoved += RemovePowerUp;
            }
        }	
	}

	private void SetText(int a_Number)
	{
		m_PointTextMesh.text = a_Number.ToString();
	}

	private void AddPowerup(int a_Slot, BasePowerup a_Powerup)
	{
		ShowPowerUp(a_Slot, a_Powerup);			
	}

	public void ShowPowerUp(int a_Slot, BasePowerup a_Type)
	{
		if(a_Slot == 0)
		{
            if(a_Type is Saxophone)
            {
                m_UISax01.SetActive(true);
            }
            else if(a_Type is Boot)
            {
                m_UIBoot01.SetActive(true);
            }
		}
		else if(a_Slot == 1)
		{
            if (a_Type is Saxophone)
            {
                m_UISax02.SetActive(true);
            }
            else if (a_Type is Boot)
            {
                m_UIBoot02.SetActive(true);
            }
        }
	}

	public void RemovePowerUp(int a_Slot)
	{
		if(a_Slot == 0)
		{
			m_UISax01.SetActive(false);
			m_UIBoot01.SetActive(false);			
		}
		else if(a_Slot == 1)
		{
			m_UISax02.SetActive(false);
			m_UIBoot02.SetActive(false);	
		}
	}
}