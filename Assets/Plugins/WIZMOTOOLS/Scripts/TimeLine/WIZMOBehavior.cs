using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SixBehaviorForces
{

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_rollBehaviorForce;
    public float RollBehaviorForce
    {
        get { return m_rollBehaviorForce; }
        set { m_rollBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_pitchBehaviorForce;
    public float PitchBehaviorForce
    {
        get { return m_pitchBehaviorForce; }
        set { m_pitchBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_yawBehaviorForce;
    public float YawBehaviorForce
    {
        get { return m_yawBehaviorForce; }
        set { m_yawBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_heaveBehaviorForce;
    public float HeaveBehaviorForce
    {
        get { return m_heaveBehaviorForce; }
        set { m_heaveBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_swayBehaviorForce;
    public float SwayBehaviorForce
    {
        get { return m_swayBehaviorForce; }
        set { m_swayBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_surgeBehaviorForce;
    public float SurgeBehaviorForce
    {
        get { return m_surgeBehaviorForce; }
        set { m_surgeBehaviorForce = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_speed;
    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_accel;
    public float Accel
    {
        get { return m_accel; }
        set { m_accel = value; }
    }

    //コンストラクタ
    //public SixBehaviorForces()
    //{
    //    m_rollBehaviorForce = 0.0f;
    //    m_pitchBehaviorForce = 0.0f;
    //    m_yawBehaviorForce = 0.0f;
    //    m_heaveBehaviorForce = 0.0f;
    //    m_swayBehaviorForce = 0.0f;
    //    m_surgeBehaviorForce = 0.0f;

    //    //Vector4 nowActuatorParam = SimvrBehavior.Instance.AxisStartParams;
    //    //m_speedAxis123 = nowActuatorParam.x;
    //    //m_accelAxis123 = nowActuatorParam.y;
    //    //m_speedAxis4 = nowActuatorParam.z;
    //    //m_accelAxis4 = nowActuatorParam.w;
    //}
}


public class SIMVRBehavior : MonoBehaviour {

    
    
    protected SixBehaviorForces m_BehaviorForces;
    public SixBehaviorForces BehaviorForces
    {
        get { return m_BehaviorForces; }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
