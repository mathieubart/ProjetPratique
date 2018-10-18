using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Resources;

//How can i do this Any other way?
public class PowerupInfo
{
    public int slot;
    public PowerupType powerupType;
}

public class Runner : Character
{
    private int m_Points = 0;
    [SerializeField]
    private GameObject m_Visual;
    [SerializeField]
    private GameObject m_TokenTrigger;
    [SerializeField]
    private GameObject m_MoneyBag;
    private Vector3 m_BaseBagScale;
    private PowerupType[] m_PowerUps = new PowerupType[2];

    [HideInInspector]
    public bool m_HisHeld {get; set;}
    [HideInInspector]
    public bool m_IsInAJar {get; set;}

    private Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    [HideInInspector]
    public Transform m_Jar;
    private Transform m_Parent;
    public Transform GetParent()
    {
        return m_Parent;
    }

    public Action<int> OnPointChanged;
    public Action<int, PowerupType> OnPowerupAdded;
    public Action<int> OnPowerupRemoved;

    public GameObject m_MusicFeedback;
    public GameObject m_BootsFeedback; //TODO : Particle trail at the runners feet5

    protected override void Awake()
    {
        base.Awake();

#if CHEATS_ACTIVATED
        if (CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            CheatManager.Instance.AddText("Press 2 to Deposit coins in Truck 01 \n");
            CheatManager.Instance.AddText("Press 3 to Deposit coins in Truck 02 \n");
        }
#endif
    }

    protected override void Start()
    {
        base.Start();
        m_Jar = null;
        m_Parent = null;
        m_BaseBagScale = m_MoneyBag.transform.localScale;
        m_BootsFeedback.SetActive(false);

        m_PowerUps[0] = PowerupType.Empty;
        m_PowerUps[1] = PowerupType.Empty;
    }

    protected override void Update()
    {
        m_MoveDirection = Vector3.zero;

#if KEYBOARD_TEST
        SetKeyBoardDirection();
#endif

        SetMoveDirection();

        if (m_Parent != null)
        {
            transform.position = m_Parent.transform.position + m_Offset;
            transform.rotation = m_Parent.rotation;
        }
        else
        {
            SetDirection(); 
            Rotate();
        }


        if(Input.GetButtonDown("Action_" + m_ID.ToString()) && m_Jar != null)
        {
            EnterExitJar();
        }

        if (Input.GetButtonDown("Powerup01_" + m_ID.ToString()) && m_PowerUps[0] != PowerupType.Empty)
        {
            ActivatePowerUp(0);
        }

        if(Input.GetButtonDown("Powerup02_" + m_ID.ToString()) && m_PowerUps[1] != PowerupType.Empty)
        {
            ActivatePowerUp(1);
        }

#if CHEATS_ACTIVATED
        Cheats();
#endif

    }

    protected override void FixedUpdate()
    {
        if (!m_HisHeld && IsGrounded() && !m_IsInAJar)
        {
            Move();
        }
    }



    //Grab Tokens/Powerups or take a Jar reference if at range
    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Token")
        {
            GrowBag();

            aCol.gameObject.SetActive(false);
            m_Points++;
            OnPointChanged(m_Points);
        }
        else if(aCol.tag == "Jar")
        {
            m_Jar = aCol.GetComponent<Transform>();
        }
        else if(aCol.tag == "Chest")
        {
            if(aCol.GetComponent<Chest>() != null)
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == PowerupType.Empty)
                {
                    aCol.GetComponent<Chest>().Loot(this, i);
                    break;
                }
            }
        }
        else if(aCol.tag == "Saxophone")
        {
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == PowerupType.Empty)
                {
                    AddPowerUp(i, PowerupType.Saxophone);
                    aCol.gameObject.SetActive(false);
                    break;
                }
            }
        }
        else if(aCol.tag == "Boot")
        {
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == PowerupType.Empty)
                {
                    AddPowerUp(i, PowerupType.Boots);
                    aCol.gameObject.SetActive(false);
                    break;
                }
            }        
        }
    }

    //Remove the jar reference if he is no longer at range
    private void OnTriggerExit(Collider aCol)
    {
        if(gameObject.layer == LayerMask.NameToLayer("PlayerFlee") && aCol.tag == "Jar")
        {
            m_Jar = null;
        }
    }

    //Get Input And Set Direction
    protected override void SetDirection()
    {
        if(!m_HisHeld && IsGrounded() && !m_IsInAJar && m_MoveDirection != Vector3.zero)
        {
            m_Direction = m_MoveDirection;
        }
    }

    //Grow The Bag Size or reset the Size if the bool is true.
    public void GrowBag()
    {     
        AudioManager.Instance.PlaySFX(0, "Coin01", transform.position);

        if (m_MoneyBag.transform.localScale.y <= 1f)
        {
            m_MoneyBag.transform.localScale += new Vector3(0f, 0.1f, 0f); 
        }
        else if(m_MoneyBag.transform.localScale.z <= 1.5f)
        {
            m_MoneyBag.transform.localScale += new Vector3(0.1f, 0f, 0.1f); 
        }     
    }

    public void ResetBag()
    {
        StartCoroutine(PlayCoinDepositSFX(m_Points, 0.15f));
        m_MoneyBag.transform.localScale = m_BaseBagScale;
        m_Points = 0;
        OnPointChanged(m_Points);
    }

    //Set the player parameters when it hide in a jar or when he is grabbed
    public void OnHold(Transform a_Parent)
    {
        m_HisHeld = true;
        m_Rigid.isKinematic = true;
        m_Parent = a_Parent;

        gameObject.layer = LayerMask.NameToLayer("HeldPlayer");
        m_TokenTrigger.layer = LayerMask.NameToLayer("HeldPlayer");

        if(a_Parent.name == "Grabber")
        {
            m_Offset = new Vector3(0f, 1.75f, 0f);
        }
        else if(a_Parent.tag == "Jar")
        {
            m_Offset = new Vector3(0f, 0f, 0f);
            m_Visual.SetActive(false);  
            a_Parent.GetComponent<Jar>().m_IsHiddingThePlayer = true;    
            AudioManager.Instance.PlaySFX(0, "Jar", transform.position);
        }
    }

    //Reset the player parameters when it is released by a player or exit the jar
    public void OnRelease()
    {
        if(m_Parent.tag == "Jar")
        {
            transform.position = m_Parent.position + new Vector3(0f, 1f, 0f);
            m_Parent.GetComponent<Jar>().m_IsHiddingThePlayer = false;
            AudioManager.Instance.PlaySFX(0, "Jar", transform.position);
        }

        m_Visual.SetActive(true);

        m_HisHeld = false;
        m_Rigid.isKinematic = false;
        m_Parent = null;

        gameObject.layer = LayerMask.NameToLayer("PlayerFlee");
        m_TokenTrigger.layer = LayerMask.NameToLayer("Token");
    }

    private void EnterExitJar()
    {
        if (!m_IsInAJar)
        {
            m_Jar.GetComponent<Jar>().m_PlayerHidden = this;
            OnHold(m_Jar);
            m_IsInAJar = true;
        }
        else
        {
            m_Jar.GetComponent<Jar>().m_PlayerHidden = null;
            OnRelease();
            m_IsInAJar = false;
        }
    }

    //Add a powerup to the player if a slot (UI Slot see **PlayerFleeUI**) is empty.
    public void AddPowerUp(int a_Slot, PowerupType a_Type)
    {
        switch (a_Type)
        {
            case PowerupType.Saxophone:
            {
                m_PowerUps[a_Slot] = PowerupType.Saxophone;
                OnPowerupAdded(a_Slot, PowerupType.Saxophone);
                break;
            }              
            case PowerupType.Boots:
            {
                m_PowerUps[a_Slot] = PowerupType.Boots;
                OnPowerupAdded(a_Slot, PowerupType.Boots);
                break;
            } 
        }

        AudioManager.Instance.PlaySFX(0, "Token_Grab", transform.position);
    }

    //Activate a powerup if there is a powerup in the input corresponding Slot.
    private void ActivatePowerUp(int a_Slot)
    {
        switch (m_PowerUps[a_Slot])
        {
            case PowerupType.Saxophone:
                {
                    gameObject.AddComponent<SaxophoneEffect>().PlayEffect();
                    break;
                }
            case PowerupType.Boots:
                {
                    gameObject.AddComponent<BootEffect>().PlayEffect();
                    break;
                }
        }

        m_PowerUps[a_Slot] = PowerupType.Empty;
        OnPowerupRemoved(a_Slot);
    }

    public int GetPoints()
    {
        return m_Points;
    }

    private IEnumerator PlayCoinDepositSFX(int a_CoinAmount, float a_WaitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(a_WaitTime);
        Vector3 depositPos = transform.position;

        for(int i = 0; i < a_CoinAmount; i++)
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
                    trucks[i].GrowDiamondPileSizeBy(m_Points);
                    TeamManager.Instance.ModifyLevelScore(0, m_Points);
                    ResetBag();
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
                    trucks[i].GrowDiamondPileSizeBy(m_Points);
                    TeamManager.Instance.ModifyLevelScore(1, m_Points);
                    ResetBag();
                }
            }
        }

        //Spawn a Coin
        if (Input.GetKeyDown(KeyCode.Alpha4) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Instantiate(Resources.Load("Jewel"), transform.position + 2.0f * transform.forward, Quaternion.identity);
        }

        //Spawn a Jar
        if (Input.GetKeyDown(KeyCode.Alpha5) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Instantiate(Resources.Load("Jar"), transform.position + 2.0f * transform.forward, Quaternion.identity);
        }

        //Spawn a Sax
        if (Input.GetKeyDown(KeyCode.Alpha6) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Instantiate(Resources.Load("Saxophone"), transform.position + 2.0f * transform.forward, Quaternion.identity);
        }

        //Spawn a Boot
        if (Input.GetKeyDown(KeyCode.Alpha7) && CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            Instantiate(Resources.Load("Boots"), transform.position + 2.0f * transform.forward, Quaternion.identity);
        }
    }
#endif

}