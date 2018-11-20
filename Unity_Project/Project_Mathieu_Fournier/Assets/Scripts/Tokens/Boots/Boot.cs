using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : BasePowerup
{
    private void Start()
    {
        m_Type = EPowerupType.Boot;    
    }

    public override void Use()
    {
        Character self = GetComponent<Character>();
        if(self)
        {
            self.gameObject.AddComponent<BootEffect>();
        }

        Destroy(this);
    }
}
