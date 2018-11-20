using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck_Trigger : MonoBehaviour
{
    private Truck m_Truck;

    private void Start()
    {
        m_Truck = GetComponentInParent<Truck>();
    }

    private void OnTriggerEnter(Collider aCol)
    {
        m_Truck.CoinDeposit(aCol.GetComponent<CoinCase>());
    }
}
