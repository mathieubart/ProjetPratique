using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookUserInput : MonoBehaviour 
{
    private CinemachineFreeLook m_FreeLookCam;

    [SerializeField]
    private PlayerID m_TargetPlayer;

    [SerializeField]
    private float m_Sensitivity = 5f;
    
    private void Start () 
	{
        m_FreeLookCam = GetComponent<CinemachineFreeLook>();
    }

    private void LateUpdate () 
	{
        m_FreeLookCam.m_XAxis.m_InputAxisValue = Input.GetAxis("RightAnalogX_" + m_TargetPlayer.ToString()) * m_Sensitivity;
        m_FreeLookCam.m_YAxis.m_InputAxisValue = Input.GetAxis("RightAnalogY_" + m_TargetPlayer.ToString()) * m_Sensitivity;
		//Debug.Log(Input.GetAxisRaw("Vertical2"));
		//Debug.Log(Input.GetAxisRaw("Horizontal2"));
    }
}