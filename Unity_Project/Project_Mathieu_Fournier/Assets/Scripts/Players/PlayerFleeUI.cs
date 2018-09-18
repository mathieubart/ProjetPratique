using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PowerupType
{
	Saxophone = 0,
	//Boots = 1 No Boots Yet In The Game. MathF
}

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

	/* No Boots Yet In The Game
	[SerializeField]
	private GameObject m_UIBoot01;
	[SerializeField]
	private GameObject m_UIBoot02;
	*/
	
	private void Start()
	{
		m_UISax01.SetActive(false);
		m_UISax02.SetActive(false);
		/* No Boots Yet In The Game
		m_UIBoot01.SetActive(false);
		m_UIBoot02.SetActive(false);
		*/

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
                player.OnPointChanged += SetText;
				player.OnPowerupAdded += AddPowerup;
				player.OnPowerupRemoved += RemovePowerUp;
            }
        }	
	}

	private void SetText(int a_Number)
	{
		m_PointTextMesh.text = a_Number.ToString();
	}

	private void AddPowerup(int a_Slot, PowerupType a_Powerup)
	{
		ShowPowerUp(a_Slot, a_Powerup);			
	}

	public void ShowPowerUp(int a_Slot, PowerupType a_Type)
	{
		if(a_Slot == 0)
		{
			switch (a_Type)
			{
				case PowerupType.Saxophone:
				{
					m_UISax01.SetActive(true);
					break;
				}
				/* 	No Boots Yet In The Game. MathF
				case PowerupType.Boots:
				{
					m_UIBoot01.SetActive(true);
					break;
				}
				*/
			}
		}
		else if(a_Slot == 1)
		{
			switch (a_Type)
			{
				case PowerupType.Saxophone:
				{
					m_UISax02.SetActive(true);
					break;
				}
				/* 	No Boots Yet In The Game. MathF
				case PowerupType.Boots:
				{
					m_UIBoot02.SetActive(true);
					break;
				}
				*/
			}
		}
	}

	public void RemovePowerUp(int a_Slot)
	{
		if(a_Slot == 0)
		{
			m_UISax01.SetActive(false);
			//m_UIBoot01.SetActive(false);			
		}
		else if(a_Slot == 1)
		{
			m_UISax02.SetActive(false);
			//m_UIBoot02.SetActive(false);	
		}
	}
}