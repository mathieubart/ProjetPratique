using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> m_Shards = new List<Rigidbody>();
    [SerializeField]
    private float m_MinForce = 50f;
    [SerializeField]
    private float m_MaxForce = 500f;

    [SerializeField]
    private GameObject m_Dynamite;
    [SerializeField]
    private List<ParticleSystem> m_Explosions = new List<ParticleSystem>();

    private bool m_IsBroken = false;

    private void Start()
    {
        for(int i = 0; i < m_Explosions.Count; i++)
        {
            m_Explosions[i].Stop();
        }

        for (int i = 0; i < m_Shards.Count; i++)
        {
            m_Shards[i].isKinematic = true;
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BreakWall();
        }
    }


    public void BreakWall()
    {
        if (!m_IsBroken)
        {
            m_IsBroken = true;

            m_Dynamite.SetActive(false);

            for (int i = 0; i < m_Shards.Count; i++)
            {
                m_Shards[i].isKinematic = false;
                m_Shards[i].AddForce(-transform.forward * Random.Range(m_MinForce, m_MaxForce));
            }

            StartCoroutine(WaitToDestroy(2.5f));
        }
    }

    private IEnumerator WaitToDestroy(float a_WaitTime)
    {
        for (int i = 0; i < m_Explosions.Count; i++)
        {
            m_Explosions[i].Play();
        }

        yield return new WaitForSeconds(a_WaitTime);
        Destroy(gameObject);
    }
}
