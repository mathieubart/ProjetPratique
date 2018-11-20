using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int m_Value = 1;
    public int Value
    {
        get { return m_Value; }
    }	
}
