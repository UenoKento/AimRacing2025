using UnityEngine;
using FMODUnity;

public class WindSound : MonoBehaviour
{
    [SerializeField]
    VehicleController m_vehicle;

    [SerializeField]
    EventReference m_eventRef_Wind;

    FMOD.Studio.EventInstance m_windEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_windEvent = RuntimeManager.CreateInstance(m_eventRef_Wind);
        m_windEvent.start();
        RuntimeManager.AttachInstanceToGameObject(m_windEvent, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        m_windEvent.setParameterByName("SPEED", m_vehicle.KPH);
    }

    void OnDestroy()
    {
        m_windEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        m_windEvent.release();
    }
}
