using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Resources;
using InControl;


public class Character : MonoBehaviour 
{
    [SerializeField]
    private CharacterData m_CharacterData;
    protected PlayerID m_ID;
    public PlayerID ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    [SerializeField]
    protected Transform m_CamTransform;

    protected float m_Speed;
    public float Speed
    {
        get{ return m_Speed;}
        set{ m_Speed = value; }
    }
    protected float m_RotationSpeed;
    protected float m_BaseRotationSpeed;

    protected float m_Sensitivity;

    [SerializeField]
    private Transform m_GroundRaycaster;
    [SerializeField]
    private List<Transform> m_FrontRaycasters = new List<Transform>();

    private float m_RotationStep;
    private Vector3 m_NewDir;
    protected Vector3 m_Direction;
    protected Vector3 m_MoveDirection;
	protected Rigidbody m_Rigid;

    [SerializeField]
    protected Animator m_Animator;

    protected virtual void Awake()
    {
        if(m_CharacterData != null)
        {
            m_Speed = m_CharacterData.Speed;
            m_RotationSpeed = m_CharacterData.RotationSpeed;
            m_Sensitivity = m_CharacterData.Sensitivity;
        }
        else
        {
            Debug.LogError("You forgot to put assign a CharacterData in the inspector. Mathieu F");
        }

#if CHEATS_ACTIVATED
        if (CheatManager.Instance && CheatManager.Instance.m_AreCheatsActive)
        {
            CheatManager.Instance.AddText("Press 4 to Spawn a Coin \n");
            CheatManager.Instance.AddText("Press 5 to Spawn a Jar \n");
            CheatManager.Instance.AddText("Press 6 to Spawn a Saxophone \n");
            CheatManager.Instance.AddText("Press 7 to Spawn a Boot \n");
        }
#endif
    }

    protected virtual void Start()
	{
        m_BaseRotationSpeed = m_RotationSpeed;
		m_Direction = Vector3.zero;
        m_MoveDirection = Vector3.zero;
		m_Rigid = GetComponent<Rigidbody>();
	}

	protected virtual void Update()
	{
        m_MoveDirection = Vector3.zero;

#if KEYBOARD_TEST
        SetKeyBoardDirection();
#else
        //Set the direction of the player movement
        SetMoveDirection();
#endif

        //Set the direction where the player is facing
        SetDirection();

        //Assign the direction soo the player rotate to the direction he is moving.
        Rotate();

#if CHEATS_ACTIVATED
        Cheats();
#endif
    }

    protected virtual void FixedUpdate()
    {
         Move();     
    }

    protected void SetMoveDirection()
    {
        if (ControllerManager.Instance.GetPlayerDevice(m_ID).GetControl(InputControlType.LeftStickY) != 0f)
        {
            Vector3 fwrd = m_CamTransform.forward;
            fwrd.y = 0f;
            fwrd = Vector3.Normalize(fwrd);
            m_MoveDirection += fwrd * m_Speed * ControllerManager.Instance.GetPlayerDevice(m_ID).GetControl(InputControlType.LeftStickY);
        }

        if (ControllerManager.Instance.GetPlayerDevice(m_ID).GetControl(InputControlType.LeftStickX) != 0f)
        {
            Vector3 right = m_CamTransform.right;
            right.y = 0f;
            right = Vector3.Normalize(right);
            m_MoveDirection += right * m_Speed * ControllerManager.Instance.GetPlayerDevice(m_ID).GetControl(InputControlType.LeftStickX);
        }
    }

#if KEYBOARD_TEST
    protected void SetKeyBoardDirection()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (((m_ID == PlayerID.PlayerOne || m_ID == PlayerID.PlayerThree) && Input.GetKey(KeyCode.D))
        || ((m_ID == PlayerID.PlayerTwo || m_ID == PlayerID.PlayerFour) && Input.GetKey(KeyCode.RightArrow)))
        {
            horizontal += 0.5f;
        }
        if(((m_ID == PlayerID.PlayerOne || m_ID == PlayerID.PlayerThree) && Input.GetKey(KeyCode.A))
        || ((m_ID == PlayerID.PlayerTwo || m_ID == PlayerID.PlayerFour) && Input.GetKey(KeyCode.LeftArrow)))
        {
            horizontal -= 0.5f;
        }

        if (((m_ID == PlayerID.PlayerOne || m_ID == PlayerID.PlayerThree) && Input.GetKey(KeyCode.W))
        || ((m_ID == PlayerID.PlayerTwo || m_ID == PlayerID.PlayerFour) && Input.GetKey(KeyCode.UpArrow)))
        {
            vertical += 0.5f;
        }
        if (((m_ID == PlayerID.PlayerOne || m_ID == PlayerID.PlayerThree) && Input.GetKey(KeyCode.S))
        || ((m_ID == PlayerID.PlayerTwo || m_ID == PlayerID.PlayerFour) && Input.GetKey(KeyCode.DownArrow)))
        {
            vertical -= 0.5f;
        }

        if (vertical != 0f)
        {           
            Vector3 fwrd = Vector3.Normalize(m_CamTransform.forward);
            fwrd.y = 0f;
            m_MoveDirection += fwrd * m_Speed * vertical;
        }

        if (horizontal != 0f)
        {
            Vector3 right = m_CamTransform.right;
            right.y = 0f;
            right = Vector3.Normalize(right);
            m_MoveDirection += right * m_Speed * horizontal;
        }
        
    }
#endif

    //Move the player forward or backward
    public void Move()
    {
        if(m_MoveDirection != Vector3.zero && !m_Animator.GetBool("Run"))
        {
            m_Animator.SetBool("Run", true);        
        }
        else if(m_MoveDirection == Vector3.zero && m_Animator.GetBool("Run"))
        {
            m_Animator.SetBool("Run", false);
        }

        float velocityY = m_Rigid.velocity.y;

        m_MoveDirection.y = velocityY;
        m_Rigid.velocity = m_MoveDirection;
        
    }

    //Rotate the player to face his direction
    protected void Rotate()
    {
        if (m_MoveDirection != Vector3.zero)
        {
            m_RotationStep = m_RotationSpeed * Time.deltaTime;
            m_NewDir = Vector3.RotateTowards(transform.forward, m_Direction, m_RotationStep, 0.0f);

            if (m_Direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(m_NewDir, transform.up);
            }
            else
            {
                m_Rigid.angularVelocity = Vector3.zero;
            }
        }
    }

	//GetAxis And Set The direction
    protected virtual void SetDirection()
    {
        if(m_MoveDirection != Vector3.zero)
        {
            m_Direction = m_MoveDirection;
        }
    }


    //Set the speed value to his base speed value.
    public void ResetSpeed()
    {
        m_Speed = m_CharacterData.Speed;
    }

    //Return true if the player touch the ground
    protected bool IsGrounded()
    {
        return Physics.Raycast(m_GroundRaycaster.position + new Vector3(0f, 0.2f, 0f), -transform.up, 0.53f, ~LayerMask.GetMask("PlayerGrab"));
    }

    //Return true if something is in front of the player
    protected bool RaycastPlayerForward()
    {
        for (int i = 0; i < m_FrontRaycasters.Count; i++)
        {
            Ray frontRay = new Ray(m_FrontRaycasters[i].position, gameObject.transform.forward);
            if (Physics.Raycast(frontRay, 0.75f, LayerMask.NameToLayer("Default")))
            {
                return true;
            }
        }
        return false;
    }	

    public virtual void StartDance()
    {
        if (!m_Animator.GetBool("Dance"))
        {
            m_Animator.SetBool("Dance", true);
        }
        if (m_Animator.GetBool("Run"))
        {
            m_Animator.SetBool("Run", false);
        }
    }

    public void OnDanceEnd()
    {
        if (m_Animator.GetBool("Dance"))
        {
            m_Animator.SetBool("Dance", false);
        }
    }

#if CHEATS_ACTIVATED
    private void Cheats()
    {
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