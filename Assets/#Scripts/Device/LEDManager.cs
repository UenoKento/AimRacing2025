using UnityEngine;
using UnityEngine.InputSystem;
using VehiclePhysics;

public class LEDManager : MonoBehaviour
{
    [SerializeField]
    VehicleController2024 m_vehicle;

    [Header("LED")]
    [SerializeField]
    float m_RPM_Min;
    [SerializeField]
    float m_RPM_Max;

    public bool Active
    {
        get; set;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        LogitechGSDK.LogiLedInit();
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Active)
            UpdateLEDs();
        else
            LogitechGSDK.LogiLedSetLighting(0, 0, 0); ;
    }

    private void OnDestroy()
    {
        LogitechGSDK.LogiLedShutdown();
    }

    void UpdateLEDs()
    {
        LogitechGSDK.LogiPlayLeds(0, m_vehicle.EngineRPM, m_RPM_Min, m_RPM_Max);
    }

}
