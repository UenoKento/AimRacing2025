using UnityEngine;
using FMODUnity;

public class DriveSound : MonoBehaviour
{
    [SerializeField]
    VehicleController2024 m_vehicle;

    [SerializeField]
    EventReference m_driveEventRef;

    FMOD.Studio.EventInstance m_driveEvent;
    Transmission2024 m_mission;

    [SerializeField]
    float m_coastInterval = 2f;
    float m_lastCoastTime;

    private void Start()
    {
        m_mission = m_vehicle.Transmission;
        m_driveEvent = RuntimeManager.CreateInstance(m_driveEventRef);
        m_driveEvent.start();
        RuntimeManager.AttachInstanceToGameObject(m_driveEvent, gameObject.transform);
    }

    private void Update()
    {
        m_driveEvent.setParameterByName("RPM",m_vehicle.EngineRPM);
        m_driveEvent.setParameterByName("GAS", m_vehicle.Accel);
        m_driveEvent.setParameterByName("SPEED", m_vehicle.KPH);
        m_driveEvent.setParameterByName("BRAKE", m_vehicle.Brake);

        // �G���W���~���[�g
        // �V�t�g�`�F���W
        float engineMute = 0f;
        if(m_mission.IsGearChanging)
        {
            engineMute = 1f;
        }
		m_driveEvent.setParameterByName("MUTE", engineMute);
		m_driveEvent.setParameterByName("GearChange", engineMute);

		float coast = 0f;
        // �R�[�X�g(�Đ����s)�̏���
        // �A�N�Z����0.3�ȉ�
        // �G���W��RPM��5500�ȏ�
        if(m_vehicle.Accel <= 0.3f && 5500f <= m_vehicle.EngineRPM)
        {
            if(Time.time - m_lastCoastTime > m_lastCoastTime)
            {
                coast = 1f;
                m_lastCoastTime = Time.time;
            }
            
        }
        m_driveEvent.setParameterByName("COAST", coast);
    }

    void OnDestroy()
    {
        m_driveEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        m_driveEvent.release();
    }
}
