using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ControllerManager : MonoBehaviour
{
    private static ControllerManager m_Instance;
    public static ControllerManager Instance
    {
        get { return m_Instance; }
    }

    private Dictionary<PlayerID, InputDevice> m_PlayerDevices = new Dictionary<PlayerID, InputDevice>();

    private void Awake()
    {
        if (ControllerManager.Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerDevice(PlayerID a_PlayerID, InputDevice a_Device)
    {
        if(!m_PlayerDevices.ContainsKey(a_PlayerID))
        {
            m_PlayerDevices.Add( a_PlayerID, a_Device);
        }
    }

    public InputDevice GetPlayerDevice(PlayerID a_PlayerID)
    {
        if (m_PlayerDevices.ContainsKey(a_PlayerID))
        {
            return m_PlayerDevices[a_PlayerID];
        }

        return null;
    }

    public bool ContainDevice(InputDevice a_Device)
    {
        return m_PlayerDevices.ContainsValue(a_Device);
    }

    public void ResetPlayerDevice()
    {
        m_PlayerDevices = new Dictionary<PlayerID, InputDevice>();
    }
}
