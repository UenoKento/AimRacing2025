/**
 * @file    DebugUnityEvents.cs
 * @brief   Debug用コールバック関数群
 * @author  豊田達也
 * @date    2024/09/12
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugUnityEvents : MonoBehaviour
{
    [SerializeField]
    private VehicleController2024 m_vehicleController;

    [SerializeField]
    private ChairController2024 m_chairController;

    [SerializeField]
    private Text m_DebugTargetText;

    [SerializeField]
    private Text m_DebugTargetValueText;
    private string m_targetName;
    private string m_targetValue;

    private enum DebugTarget
    {
        vehicle = 0,
        chair,
    }private DebugTarget m_target;

    private void Start()
    {
        //m_DebugTargetText.text = "Vehicle";
        //m_DebugTargetValueText.text = "Noting:Noting";

    }

    void Update()
    {
        //m_DebugTargetText.text = m_targetName + ":" + m_targetValue;
    }


    public void OnEast(InputAction.CallbackContext context)
    {
        switch (m_target)
        {
            case DebugTarget.vehicle:
                if(context.performed) 
                {
                    //m_vehicleController
                }
                break;
            case DebugTarget.chair:
                if (context.performed)
                {
                   //m_chairController
                }
                break;
        }
    }

    public void OnNorth(InputAction.CallbackContext context)
    {

    }

    public void OnWast(InputAction.CallbackContext context)
    {

    }

    public void OnSouth(InputAction.CallbackContext context)
    {

    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        
    }
 
    public void OnRight(InputAction.CallbackContext context)
    {

    }

    public void OnUp(InputAction.CallbackContext context)
    {

    }

    public void OnDown(InputAction.CallbackContext context)
    {

    }

    public void OnPuls(InputAction.CallbackContext context)
    {

    }

    public void OnMinus(InputAction.CallbackContext context)
    {

    }

    public void OnL2(InputAction.CallbackContext context)
    {

    }

    public void OnL3(InputAction.CallbackContext context)
    {

    }

    public void OnR2(InputAction.CallbackContext context)
    {

    }

    public void OnR3(InputAction.CallbackContext context)
    {

    }

    public void OnShare(InputAction.CallbackContext context)
    {

    }

    public void OnOption(InputAction.CallbackContext context)
    {

    }

    public void OnDial(InputAction.CallbackContext context)
    {

    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        //switch(m_target)
        //{
        //    case DebugTarget.vehicle:
        //        m_target = DebugTarget.chair;
        //        m_DebugTargetText.text = "Chair";
        //        break;
        //    case DebugTarget.chair:
        //        m_target = DebugTarget.vehicle;
        //        m_DebugTargetText.text = "Vehicle";
        //        break;
        //}
    }
}
