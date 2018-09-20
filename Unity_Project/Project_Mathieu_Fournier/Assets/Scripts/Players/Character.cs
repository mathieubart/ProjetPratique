﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Character : MonoBehaviour 
{
    [SerializeField]
    private CharacterData m_CharacterData;
    protected PlayerID m_ID;
    public void SetID(PlayerID a_ID)
    {
        m_ID = a_ID;
    }

    [SerializeField]
    protected Transform m_CamTransform;

    protected float m_Speed;
    public float Speed
    {
        get{ return m_Speed;}
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
        //Set the direction of the player movement
        SetMoveDirection();

        //Set the direction where the player is facing
        SetDirection();

        //Assign the direction soo the player rotate to the direction he is moving.
        Rotate();
	}

    protected virtual void FixedUpdate()
    {
         Move();     
    }

    protected void SetMoveDirection()
    {
        m_MoveDirection = Vector3.zero;

        if (Input.GetAxis("Forward_" + m_ID.ToString()) != 0f)
        {
            Vector3 fwrd = Vector3.Normalize(m_CamTransform.forward);
            fwrd.y = 0f;
            m_MoveDirection += fwrd * m_Speed * Input.GetAxis("Forward_" + m_ID.ToString());
        }

        if (Input.GetAxis("LeftAnalogY_" + m_ID.ToString()) != 0f)
        {
            Vector3 fwrd = m_CamTransform.forward;
            fwrd.y = 0f;
            fwrd = Vector3.Normalize(fwrd);
            m_MoveDirection += fwrd * m_Speed * Input.GetAxis("LeftAnalogY_" + m_ID.ToString());
        }

        if (Input.GetAxis("LeftAnalogX_" + m_ID.ToString()) != 0f)
        {
            Vector3 right = m_CamTransform.right;
            right.y = 0f;
            right = Vector3.Normalize(right);
            m_MoveDirection += right * m_Speed * Input.GetAxis("LeftAnalogX_" + m_ID.ToString());
        }
    }

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

    //Return true if the player touch the ground
    protected bool IsGrounded()
    {
        bool isGrounded = false;
        if(!isGrounded)
        {
            isGrounded = Physics.Raycast(m_GroundRaycaster.position + new Vector3(0f, 0.2f, 0f), -transform.up, 0.53f, ~LayerMask.GetMask("PlayerGrab"));
        }
        return isGrounded;
    }

    //Return true if something is in front of the player
    protected bool RaycastPlayerForward()
    {
        bool raycastPlayerForward = false;

        for (int i = 0; i < m_FrontRaycasters.Count; i++)
        {
            Ray frontRay = new Ray(m_FrontRaycasters[i].position, gameObject.transform.forward);
            if(Physics.Raycast(frontRay, 0.75f, LayerMask.NameToLayer("Default")))
            {
                raycastPlayerForward = true;
                continue;
            }
        }
        return raycastPlayerForward;
    }	

	public void SetSpeed(float a_NewSpeed)
    {
        m_Speed = a_NewSpeed;
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
}