using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//public struct SIMVRMotionDirection
//{
//    [SerializeField]
//    private float m_accelAxis123;
//    public float AccelAxis123
//    {
//        get { return m_accelAxis123; }
//        set { m_accelAxis123 = value; }
//    }

//    [SerializeField]
//    private float m_accelAxis4;
//    public float AccelAxis4
//    {
//        get { return m_accelAxis4; }
//        set { m_accelAxis4 = value; }
//    }

//}




// A behaviour that is attached to a playable
public class SIMVRPBSpring : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public float m_power;
    public float m_target;
    public float m_damping;
    public float m_accelFactor;
    public SIMVRMotionSixAxis m_axis;

    private SixBehaviorForces m_setforces;
    private float m_velocity;
    private float m_accel;
    private float pos;


    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
        // Debug.Log("OnGraphStart");
        m_target = 0;
        m_velocity = m_accel = 0;
        pos = m_power;
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
        m_target = 0;
        m_velocity = m_accel = 0;
        pos = m_power;

    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        float diff = m_target - pos;
        m_accel = diff * m_accelFactor;
        m_velocity += m_accel;
        m_velocity *= m_damping;
        pos += m_velocity;
  

        //データを毎フレーム入れ込む
        switch (m_axis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_setforces.RollBehaviorForce = pos;
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_setforces.PitchBehaviorForce = pos;
                break;

            case SIMVRMotionSixAxis.YAW:
                m_setforces.YawBehaviorForce = pos;
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_setforces.HeaveBehaviorForce = pos;
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_setforces.SwayBehaviorForce = pos;
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_setforces.SurgeBehaviorForce = pos;
                break;

        }

        m_mover.MoverTimeLineCall(m_setforces, m_axis);

    }
}
