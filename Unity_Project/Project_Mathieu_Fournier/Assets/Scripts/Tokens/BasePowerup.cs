using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePowerup : MonoBehaviour
{
    protected EPowerupType m_Type;
    public EPowerupType Type
    {
        get { return m_Type; }
    }

    public virtual void Use()
    {

    }
}
