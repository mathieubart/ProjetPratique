using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private BaseEffect[] m_PowerUps = new BaseEffect[2];

    [HideInInspector]
    public bool m_HisHeld {get; set;}
    [HideInInspector]
    public bool m_IsInAJar {get; set;}

    private Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    [HideInInspector]
    public Transform m_Jar;
    private Transform m_Parent;

    public Action<int> OnPointChanged;
    public Action<int, PowerupType> OnPowerupAdded;
    public Action<int> OnPowerupRemoved;

    public GameObject m_MusicFeedback;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        m_Jar = null;
        m_Parent = null;
        m_BaseBagScale = m_MoneyBag.transform.localScale;
    }

    protected override void Update()
    {
        if (Input.GetAxis("Forward_" + m_ID.ToString()) > 0f || Input.GetAxis("LeftAnalogY_" + m_ID.ToString()) > 0f
            && !RaycastPlayerForward())
        {
            m_MoveDirection = transform.forward * m_Speed;
        }
        else if(Input.GetAxis("Forward_" + m_ID.ToString()) < 0f || Input.GetAxis("LeftAnalogY_" + m_ID.ToString()) < 0f)
        {
            m_MoveDirection = -transform.forward * m_Speed;
        }
        else
        {
            m_MoveDirection = Vector3.zero;
        }	

        if(m_Parent != null)
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
            if(!m_IsInAJar)
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

        if(Input.GetButtonDown("Powerup01_" + m_ID.ToString()) && m_PowerUps[0] != null)
        {
            ActivatePowerUp(0);
        }

        if(Input.GetButtonDown("Powerup02_" + m_ID.ToString()) && m_PowerUps[1] != null)
        {
            ActivatePowerUp(1);
        }
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
                if(m_PowerUps[i] == null)
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
                if(m_PowerUps[i] == null)
                {
                    AddPowerUp(i, PowerupType.Saxophone);
                    aCol.gameObject.SetActive(false);
                    break;
                }
            }
        }
        /* Boots Are Not In The Game Yet. MathF
        else if(aCol.tag == "Boot")
        {
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == null)
                {
                    AddPowerUp(i, PowerupType.Boots);
                    aCol.gameObject.SetActive(false);
                }
            }        
        }
        */
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
        m_Direction = Vector3.zero;

        if(Input.GetAxis("LeftAnalogX_" + m_ID.ToString()) != 0f)
        {
            m_RotationSpeed = m_BaseRotationSpeed * m_Sensitivity * Mathf.Abs(Input.GetAxis("LeftAnalogX_" + m_ID.ToString()));
        }
        else
        {
            m_RotationSpeed = m_BaseRotationSpeed;
        }

        if(!m_HisHeld && IsGrounded() && !m_IsInAJar)
        {
            if (Input.GetAxis("Horizontal_" + m_ID.ToString()) < 0f || Input.GetAxis("LeftAnalogX_" + m_ID.ToString()) < 0f)
            {
                m_Direction -= transform.right;
            }
            if (Input.GetAxis("Horizontal_" + m_ID.ToString()) > 0f || Input.GetAxis("LeftAnalogX_" + m_ID.ToString()) > 0f)
            {
                m_Direction += transform.right;
            }
        }
    }

    //Grow The Bag Size or reset the Size if the bool is true.
    public void GrowBag()
    {      
        if(m_MoneyBag.transform.localScale.y <= 1f)
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
        }
    }

    //Reset the player parameters when it is released by a player or exit the jar
    public void OnRelease()
    {
        if(m_Parent.tag == "Jar")
        {
            transform.position = m_Parent.position + new Vector3(0f, 1f, 0f);
            m_Parent.GetComponent<Jar>().m_IsHiddingThePlayer = false;
        }

        m_Visual.SetActive(true);

        m_HisHeld = false;
        m_Rigid.isKinematic = false;
        m_Parent = null;

        gameObject.layer = LayerMask.NameToLayer("PlayerFlee");
        m_TokenTrigger.layer = LayerMask.NameToLayer("Token");
    }

    //Add a powerup to the player if a slot (UI Slot see **PlayerFleeUI**) is empty.
    public void AddPowerUp(int a_Slot, PowerupType a_Type)
    {
        switch (a_Type)
        {
            case PowerupType.Saxophone:
            {
                m_PowerUps[a_Slot] = gameObject.AddComponent<SaxophoneEffect>();
                OnPowerupAdded(a_Slot, PowerupType.Saxophone);
                break;
            }  
            /* No Boots In The Game Yet MathF              
            case PowerupType.Boots:
            {
                m_PowerUps[a_Slot] = gameObject.AddComponent<BootEffect>();

                OnPowerupAdded(a_Slot, PowerupType.Boots);
                break;
            }
            */  
        }
    }

    //Activate a powerup if there is a powerup in the input corresponding Slot.
    private void ActivatePowerUp(int a_Slot)
    {
        m_PowerUps[a_Slot].PlayEffect();
        m_PowerUps[a_Slot] = null;
        OnPowerupRemoved(a_Slot);
    }

    public int GetPoints()
    {
        return m_Points;
    }
}