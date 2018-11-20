using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCase : MonoBehaviour
{

    private Runner m_Runner;
    public Action<int> OnPointChanged;

    private int m_Coins = 0;
    public int Coins
    {
        get { return m_Coins; }
        private set { m_Coins = value; }
    }

    private void Start()
    {
        if(GetComponentInParent<Runner>())
        {
            m_Runner = GetComponentInParent<Runner>();
        }
    }

#if CHEATS_ACTIVATED
    private void Update()
    {
        Cheats();     
    }
#endif

    private void OnTriggerEnter(Collider a_Col)
    {
        if (a_Col.GetComponent<Coin>())
        {
            if (m_Runner)
            {
                m_Runner.GrowBag();
            }

            a_Col.gameObject.SetActive(false);
            m_Coins += a_Col.GetComponent<Coin>().Value;
            OnPointChanged(m_Coins);
        }
    }

    public void CoinDeposit()
    {
        if (m_Coins > 0)
        {
            if (m_Runner)
            {
                m_Runner.ResetBag();
            }

            StartCoroutine(PlayCoinDepositSFX(m_Coins, 0.15f));
            m_Coins = 0;
            OnPointChanged(m_Coins);
        }
    }

    private IEnumerator PlayCoinDepositSFX(int a_CoinAmount, float a_WaitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(a_WaitTime);
        Vector3 depositPos = transform.position;

        for (int i = 0; i < a_CoinAmount; i++)
        {
            AudioManager.Instance.PlaySFX(0, "Coin_Deposit", depositPos);

            yield return wait;
        }
    }

#if CHEATS_ACTIVATED
    private void Cheats()
    {
        //Coin deposit in team 01 truck 
        if (Input.GetKeyDown(KeyCode.Alpha2) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Truck[] trucks = GameObject.FindObjectsOfType<Truck>();

            for (int i = 0; i < trucks.Length; i++)
            {
                if (trucks[i].GetTeamAssigned() == 0)
                {
                    trucks[i].GrowDiamondPileSizeBy(m_Coins);
                    TeamManager.Instance.ModifyLevelScore(0, m_Coins);

                    if(m_Runner)
                    {
                        m_Runner.ResetBag();
                    }
                }
            }
        }

        //Coin deposit in team 02 truck 
        if (Input.GetKeyDown(KeyCode.Alpha3) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Truck[] trucks = GameObject.FindObjectsOfType<Truck>();

            for (int i = 0; i < trucks.Length; i++)
            {
                if (trucks[i].GetTeamAssigned() == 1)
                {
                    trucks[i].GrowDiamondPileSizeBy(m_Coins);
                    TeamManager.Instance.ModifyLevelScore(1, m_Coins);

                    if (m_Runner)
                    {
                        m_Runner.ResetBag();
                    }
                }
            }
        }
    }
#endif
}
