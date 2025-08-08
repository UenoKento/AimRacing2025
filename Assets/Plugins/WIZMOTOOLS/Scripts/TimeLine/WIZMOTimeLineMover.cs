using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIZMOTimeLineMover : MonoBehaviour {

    SixBehaviorForces m_holdBehavior;
    SixBehaviorForces m_zeroClear;
    private WIZMOController ctrl = null;
    public WIZMOController CTRL
    {
        get { return ctrl; }
    }

    [SerializeField]
    private bool m_speedChange;

    [SerializeField]
    private bool m_accelerateChange;

    private float m_defaultSpeedAxis123;
    private float m_defaultSpeedAxis4;
    private float m_defaultAccelerateAxis123;
    private float m_defaultAccelerateAxis4;

    // Use this for initialization
    void Start () {

        ctrl = this.GetComponent<WIZMOController>();
        if (ctrl == null)
            return;

        m_zeroClear.RollBehaviorForce = 0;
        m_zeroClear.PitchBehaviorForce = 0;
        m_zeroClear.YawBehaviorForce = 0;
        m_zeroClear.HeaveBehaviorForce = 0;
        m_zeroClear.SurgeBehaviorForce = 0;
        m_zeroClear.SwayBehaviorForce = 0;

        m_defaultSpeedAxis123 = ctrl.speed1_all;
        m_defaultAccelerateAxis123 = ctrl.accel;

    }

    // Update is called once per frame
    void Update () {

        if (ctrl == null)
            return;

        ctrl.roll = m_holdBehavior.RollBehaviorForce;
        ctrl.pitch = m_holdBehavior.PitchBehaviorForce;
        ctrl.yaw = m_holdBehavior.YawBehaviorForce;
        ctrl.heave = m_holdBehavior.HeaveBehaviorForce;
        ctrl.sway = m_holdBehavior.SwayBehaviorForce;
        ctrl.surge = m_holdBehavior.SurgeBehaviorForce;

        m_holdBehavior = m_zeroClear;
    }

    //タイムラインから呼び出される関数
    public void MoverTimeLineCall(SixBehaviorForces m_sixbehavior, SIMVRMotionSixAxis axis)
    {
        switch (axis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_holdBehavior.RollBehaviorForce += m_sixbehavior.RollBehaviorForce;
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_holdBehavior.PitchBehaviorForce = m_sixbehavior.PitchBehaviorForce;
                break;

            case SIMVRMotionSixAxis.YAW:
                m_holdBehavior.YawBehaviorForce += m_sixbehavior.YawBehaviorForce;
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_holdBehavior.SurgeBehaviorForce += m_sixbehavior.SurgeBehaviorForce;
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_holdBehavior.SwayBehaviorForce += m_sixbehavior.SwayBehaviorForce;
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_holdBehavior.HeaveBehaviorForce += m_sixbehavior.HeaveBehaviorForce;
                break;

        }
        
       
    }

    public void MoverTimeLineCall(SixBehaviorForces m_sixbehavior)
    {
   
        m_holdBehavior.RollBehaviorForce += m_sixbehavior.RollBehaviorForce;
        m_holdBehavior.PitchBehaviorForce += m_sixbehavior.PitchBehaviorForce;
        m_holdBehavior.YawBehaviorForce += m_sixbehavior.YawBehaviorForce;
        m_holdBehavior.SurgeBehaviorForce += m_sixbehavior.SurgeBehaviorForce;
        m_holdBehavior.SwayBehaviorForce += m_sixbehavior.SwayBehaviorForce;
        m_holdBehavior.HeaveBehaviorForce += m_sixbehavior.HeaveBehaviorForce;

    }

    public void MoverTimeLineSetSpeed(SIMVRPBSpeedParam speedparam)
    {
        if(m_speedChange && Application.isPlaying)
        {
            ctrl.speed1_all = speedparam.Speed;
        }
    }

    public void MoverTimelineSetAccelerator(SIMVRPBAccelerationParam accelerateparam)
    {
        if (m_accelerateChange && Application.isPlaying)
        {
            ctrl.accel = accelerateparam.Accel;
        }
    }
}
