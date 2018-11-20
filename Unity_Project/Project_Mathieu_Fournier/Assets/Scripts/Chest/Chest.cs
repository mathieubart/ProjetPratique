using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour 
{
	public void Loot(Runner a_Player, int a_Slot)
	{
        int rand = Random.Range(0, 2);

        a_Player.AddPowerUp(a_Slot, (EPowerupType)rand);

		Destroy(this);
	}
}