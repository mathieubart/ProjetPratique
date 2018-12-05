using Cinemachine;
using UnityEngine;
using InControl;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookUserInput : MonoBehaviour 
{
    private CinemachineFreeLook m_FreeLookCam;

    [SerializeField]
    private PlayerID m_TargetPlayer;

    [SerializeField]
    private float m_Sensitivity = 5f;

    private float currentX = 0f;
    private float currentY = 0f;

    private float DEATH_ZONE = 0.25f;
    
    private void Start () 
	{
        m_FreeLookCam = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
#if KEYBOARD_TEST
        KeyBoardInputs();
#else
        currentX = ControllerManager.Instance.GetPlayerDevice(m_TargetPlayer).GetControl(InputControlType.RightStickX);
        currentY = ControllerManager.Instance.GetPlayerDevice(m_TargetPlayer).GetControl(InputControlType.RightStickY);
#endif
    }

#if KEYBOARD_TEST
    private void KeyBoardInputs()
    {
        currentX = 0f;
        currentY = 0f;
        if (Input.GetKey(KeyCode.Keypad4))
        {
            currentX += 0.05f;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            currentX -= 0.05f;
        }
        if (Input.GetKey(KeyCode.Keypad8))
        {
            currentY += 0.05f;
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            currentY -= 0.05f;
        }
    }
#endif

    private void LateUpdate () 
	{

        if (currentX >= DEATH_ZONE || currentX <= -DEATH_ZONE)
        {
            m_FreeLookCam.m_XAxis.m_InputAxisValue = currentX * m_Sensitivity;
        }
        else
        {
            m_FreeLookCam.m_XAxis.m_InputAxisValue = 0f;
        }

        if (currentY >= DEATH_ZONE || currentY <= -DEATH_ZONE)
        {
            m_FreeLookCam.m_YAxis.m_InputAxisValue = currentY * m_Sensitivity;
        }
        else
        {
            m_FreeLookCam.m_YAxis.m_InputAxisValue = 0f;
        }
		//Debug.Log(Input.GetAxisRaw("Vertical2"));
		//Debug.Log(Input.GetAxisRaw("Horizontal2"));
    }
}