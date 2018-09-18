using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{

    private void Start()
    {
//Bloc of pre compilation code.
//Prevent that code to be compilated in a build.
#if UNITY_CHEAT
        Debug.Log("That cheat!");
#endif 
        Debug.Log("That insn't cheat!");
    }

    private void Update()
    {
        
    }
}
