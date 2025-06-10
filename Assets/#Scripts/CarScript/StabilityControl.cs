using UnityEngine;

public class StabilityControl : MonoBehaviour
{
    [SerializeField]
    VehicleController2024 m_vehicle;
    [SerializeField]
    WheelController2024 m_fl;
    [SerializeField]
    WheelController2024 m_fr;

    [SerializeField, ShowInInspector]
    float m_yawRate;
    [SerializeField,ShowInInspector]
    float desireYaw;
    [SerializeField, ShowInInspector]
    float m_a;
    [SerializeField, ShowInInspector]
    float m_aleft;
    [SerializeField,ShowInInspector]
    Vector2 m_fronts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_yawRate = m_vehicle.Rigidbody.angularVelocity.y;

        float v = m_vehicle.Rigidbody.linearVelocity.magnitude;
        Debug.Log("Vel" + v);

        // lf = 1.331 - 0.6 = 0.731
        // lr = 1.114 + 0.6 = 1.7114
        desireYaw = v * m_fl.SteerAngle * Mathf.Deg2Rad;
        desireYaw /= 2.75f + (1120f * v * v * 0.983f / (2f * 30000f * 2.75f));
        m_a = desireYaw / 8;

        float Mbf = 1.58f / 2f * (m_fr.LongForce - m_fl.LongForce);

        float dFxf = 2 * Mbf / 1.58f;

        m_aleft = desireYaw - m_yawRate;

        m_fronts.x = 0.5f * dFxf * m_fl.Radius;
        m_fronts.y = 0.5f * dFxf * m_fr.Radius;
    }
}
