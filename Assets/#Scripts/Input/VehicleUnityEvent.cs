/**
 * @file    VehicleUnityEvent.cs
 * @brief   InputSysyemからコントローラーの入力を受け取る
 * @author  豊田達也
 * @date    2024/06/21
 */

using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleUnityEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject m_vehicle;
    private VehicleController2024 m_vehicleController;
    private Transmission2024 m_transmission;
    [SerializeField]
    private ChairController2024 Chair;

	

	// Start is called before the first frame update
	void Start()
    {
        m_vehicleController = m_vehicle.GetComponent<VehicleController2024>();
        m_transmission = m_vehicleController.Transmission;
    }


    public void OnHandle(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        m_vehicleController.Steering = value;
        //Debug.Log("Handle "+value);
    }

    public void OnAccel(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        m_vehicleController.Accel = value;
        //Debug.Log("Accel " + value);
    } 
    
    public void OnAccelPedal(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        value = 1 - (value + 1) / 2;
        m_vehicleController.Accel = value;
        //Debug.Log("Accel " + value);
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        m_vehicleController.Brake = value;
        //Debug.Log("Breke " + value);
    }
    public void OnBrakePedal(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        //Debug.Log("Breke " + value);
        value = 1 - (value + 1) / 2;
        m_vehicleController.Brake = value;
    }

    public void OnClutch(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
		value = 1 - (value + 1) / 2;
        m_vehicleController.Clutch = value;
		//Debug.Log("Clutch "+ value);
    }

    public void OnShiftUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            m_transmission.ShiftUp();
            //Debug.Log("ShiftUp ");
        }
    }

    public void OnShiftDown(InputAction.CallbackContext context)
    { 
        if (context.phase == InputActionPhase.Started)
        {
            m_transmission.ShiftDown();
            //Debug.Log("ShiftDown ");
        }
    }
}
