using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : Character
{
    [SerializeField]
    private GrabberData m_GrabberData;

    private float m_ThrowForce;
    private float m_ThrowAngle;

    private GameObject m_GrabAbleObject;
    private GameObject m_HeldObject;
    private List<GameObject> m_GrabablePots = new List<GameObject>();

    private bool m_CanGrab = true;
    private const float SILENCED_TIME = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        if(m_GrabberData)
        {
            m_ThrowAngle = m_GrabberData.ThrowAngle;
            m_ThrowForce = m_GrabberData.ThrowForce;
        }
        else
        {
            Debug.LogError("You forgot to put a GrabberData on the character. Mathieu F");
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {      
        base.Update();

        if (Input.GetButtonDown("Action_" + m_ID))
        {
            if(m_HeldObject != null)
            {
                Throw();
            }
	        else if (m_GrabAbleObject != null && m_CanGrab)
            {        
                Grab();            
            }      
        }

#if KEYBOARD_TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_HeldObject != null)
            {
                Throw();
            }
            else if (m_GrabAbleObject != null && m_CanGrab)
            {
                Grab();
            }
        }
#endif
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //Spherecast to find all the pots inside grabable range. return list of pots gameobject
    private void RaycastGrabablePots()
    {
        m_GrabablePots.Clear();
        RaycastHit[] spherecastHifos;
        spherecastHifos = Physics.SphereCastAll(transform.position, 1.5f, transform.up, 0.9f * transform.localScale.y, LayerMask.GetMask("Jar"));
        
        for (int i = 0; i < spherecastHifos.Length; i++)
        {
            m_GrabablePots.Add(spherecastHifos[i].collider.gameObject);
        }
    }
  
    private GameObject RaycastCharacterFlee()
    {
        GameObject characterFound = null;
        RaycastHit[] spherecastHifo;
        spherecastHifo = Physics.SphereCastAll(transform.position, 1.5f, transform.up ,0.9f * transform.localScale.y, LayerMask.GetMask("PlayerFlee"));
        if(spherecastHifo.Length != 0)
        {
            characterFound = spherecastHifo[0].collider.gameObject;
        }
        return characterFound;
    }

    //Take the array of object returned by the sphere cast and assign the Best GrabableObject.
    //Priority CharacterFlee > ClosestPot.
    private GameObject GetGrabableObject()
    {
        GameObject grabableObject = null;

        grabableObject = RaycastCharacterFlee();

        if(grabableObject == null)
        {
            float closestDistance = 100000f;
            RaycastGrabablePots();

            for (int i = 0; i < m_GrabablePots.Count; i++)
            {
                if(Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position) < closestDistance)
                {
                    closestDistance = Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position);
                    grabableObject = m_GrabablePots[i];
                }
            }
        }

        return grabableObject;
    }

    private void OnTriggerEnter(Collider aCol)
    {
        //If the character hold nothing.
        //Look for the closest grabable object.
        if(m_HeldObject == null)
        {
            m_GrabAbleObject = GetGrabableObject();
        }
    }

    private void OnTriggerExit(Collider aCol)
    {
        m_GrabAbleObject = null;

        // look for the closest grabable object.      
        if (m_HeldObject == null)
        {              
             m_GrabAbleObject = GetGrabableObject();
        }
    }

    private void OnCollisionEnter(Collision aCollision)
    {
        if(aCollision.collider.tag == "Jar")
        {       
            foreach (ContactPoint point in aCollision)
            {
                if(point.point.y > 1.2f)
                {
                    gameObject.AddComponent<JarStunEffect>();
                    StartStunned();
                    break;
                }
            }
        }
    }

    private void Grab()
    {
        if(m_GrabAbleObject.name == "Runner")
        {
            m_GrabAbleObject.GetComponent<Runner>().OnHold(transform);
            //m_GrabAbleObject.GetComponent<Renderer>().enabled = true;
            m_HeldObject = m_GrabAbleObject;
            m_GrabAbleObject = null;
        }
        else if(m_GrabAbleObject.tag == "Jar")
        {
            m_GrabAbleObject.GetComponent<Jar>().OnHold(transform);
            m_HeldObject = m_GrabAbleObject;
            m_GrabAbleObject = null;
        }
    }

    private void Throw()
    {
        Vector3 throwDirection = Quaternion.AngleAxis(m_ThrowAngle, -transform.right) * transform.forward;

        if(m_HeldObject.name == "Runner")
        {
            m_HeldObject.GetComponent<Runner>().OnRelease();
        }
        else if(m_HeldObject.tag == "Jar")
        {
            m_HeldObject.GetComponent<Jar>().OnRelease();
        }

        m_HeldObject.GetComponent<Rigidbody>().velocity = m_Rigid.velocity;      
        m_HeldObject.GetComponent<Rigidbody>().AddForce(throwDirection * m_ThrowForce);

        m_HeldObject = null;
    }

    public void Drop()
    {
        if (m_HeldObject.name == "Runner")
        {
            m_HeldObject.GetComponent<Runner>().OnRelease();
        }
        else if (m_HeldObject.tag == "Jar")
        {
            m_HeldObject.GetComponent<Jar>().OnRelease();
        }

        m_HeldObject.GetComponent<Rigidbody>().velocity = m_Rigid.velocity;

        m_HeldObject = null;

        StartCoroutine(WaitForGrabEnable());
    }

    public void StartStunned()
    {
        if (m_Animator.GetBool("Dance"))
        {
            m_Animator.SetBool("Dance", false);
        }
        if (m_Animator.GetBool("Run"))
        {
            m_Animator.SetBool("Run", false);
        }
        if (!m_Animator.GetBool("Stunned"))
        {
            m_Animator.SetBool("Stunned", true);
        }
    }

    public void OnStunnedEnd()
    {
        if (m_Animator.GetBool("Stunned"))
        {
            m_Animator.SetBool("Stunned", false);
        }
    }

    public override void StartDance()
    {
        base.StartDance();
        if (m_Animator.GetBool("Stunned"))
        {
            m_Animator.SetBool("Stunned", false);
        }
    }

    //The player cant grab anything for a amount of time
    private IEnumerator WaitForGrabEnable()
    {
        m_CanGrab = false;
        yield return new WaitForSeconds(SILENCED_TIME);
        m_CanGrab = true;
    }
}