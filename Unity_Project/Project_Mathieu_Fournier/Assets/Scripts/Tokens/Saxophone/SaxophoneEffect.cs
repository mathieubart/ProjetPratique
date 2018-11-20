using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxophoneEffect : BaseEffect 
{
    //The characters we apply the effect on;
	private List<Character> m_Characters = new List<Character>();

	private void Awake()
	{
        SaxophoneEffect[] ongoingEffects = GetComponents<SaxophoneEffect>();
        if (ongoingEffects.Length > 1)
        {
            for (int i = 0; i < ongoingEffects.Length; i++)
            {
                if (ongoingEffects[i] != this)
                {
                    ongoingEffects[i].AddTime(m_EffectDuration);
                    Destroy(this);
                }
            }
        }
	}

    public override void PlayEffect()
	{
        Root();
	}

    protected override void StopEffect()
    {
        ResetSpeed();

        Destroy(this);
    }

    private void Root()
    {
        Character self = GetComponent<Character>();
        if(self)
        {
            if (self.Speed != 0f)
            {
                self.Speed = 0f;
                self.StartDance();
            }
        }
    }

	private void ResetSpeed()
	{
        Character self = GetComponent<Character>();
        if (self)
        {
            self.ResetSpeed();
            self.OnDanceEnd();
        }
	}
}