using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxophone : BasePowerup
{

    public Saxophone()
    {
        m_Type = EPowerupType.Saxophone;
    }

    public override void Use()
    {
        ApplyEffect();
    }

    private void ApplyEffect()
    {
        Character self = GetComponent<Character>();
        List<Character> characters = GetCloseCharacters();

        //If The List Contains Self, Remove Self From The List
        if (characters.Contains(self))
        {
            characters.Remove(GetComponent<Character>());
        }

        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].gameObject.AddComponent<SaxophoneEffect>();
        }
    }

    //SphereCast Around Self And Return All Found Characters
    private List<Character> GetCloseCharacters()
    {
        List<Character> characters = new List<Character>();

        RaycastHit[] spherecastHifos;

        spherecastHifos = Physics.SphereCastAll(transform.position, 5f, transform.position, 0f, LayerMask.GetMask("PlayerGrab", "PlayerFlee"));

        for(int i = 0; i < spherecastHifos.Length; i++)
        {
            if (spherecastHifos[i].collider.GetComponent<Character>())
            {
                characters.Add(spherecastHifos[i].collider.GetComponent<Character>());
            }
        }

        return characters;
    }
}
