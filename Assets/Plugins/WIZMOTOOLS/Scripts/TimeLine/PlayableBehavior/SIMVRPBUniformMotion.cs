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

public enum SIMVRMotionSixAxis
{
    ROLL = 1,
    PITCH = 2,
    YAW = 3,
    HEAVE = 4,
    SWAY = 5,
    SURGE = 6
}

public enum SIMVRMotionDirecion
{
   POSITIVE = 1,
   NEGATIVE = 2
}

public struct SIMVRPAUniformMotionParam
{
    private SIMVRMotionSixAxis m_axis;
    public SIMVRMotionSixAxis SIXAXIS
    {
        set { m_axis = value; }
        get { return m_axis; }
    }
    private SIMVRMotionDirecion m_direction;
    public SIMVRMotionDirecion DIRECTION
    {
        set { m_direction = value; }
        get { return m_direction; }
    }

    [SerializeField]
    private float m_position;
    public float POTISION
    {
        get { return m_position; }
        set { m_position = value; }
    }
}




// A behaviour that is attached to a playable
public class SIMVRPBUniformMotion : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public SIMVRPAUniformMotionParam m_param;
    private SixBehaviorForces m_preforces;
    private SixBehaviorForces m_setforces;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
       
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

        //現在の位置をSIMVR Controllerから取得する
        m_preforces.RollBehaviorForce = m_mover.CTRL.roll;
        m_preforces.PitchBehaviorForce = m_mover.CTRL.pitch;
        m_preforces.YawBehaviorForce = m_mover.CTRL.yaw;
        m_preforces.HeaveBehaviorForce = m_mover.CTRL.heave;
        m_preforces.SwayBehaviorForce = m_mover.CTRL.sway;
        m_preforces.SurgeBehaviorForce = m_mover.CTRL.surge;



    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        //毎フレーム実行
        var currentTime = (float)playable.GetTime() / (float)playable.GetDuration();
       
        //データを毎フレーム入れ込む
        switch(m_param.SIXAXIS)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_setforces.RollBehaviorForce = Mathf.Lerp(m_preforces.RollBehaviorForce, m_param.POTISION, currentTime);
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_setforces.PitchBehaviorForce = Mathf.Lerp(m_preforces.PitchBehaviorForce, m_param.POTISION, currentTime);
                break;

            case SIMVRMotionSixAxis.YAW:
                m_setforces.YawBehaviorForce = Mathf.Lerp(m_preforces.YawBehaviorForce, m_param.POTISION, currentTime);
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_setforces.HeaveBehaviorForce = Mathf.Lerp(m_preforces.HeaveBehaviorForce, m_param.POTISION, currentTime);
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_setforces.SwayBehaviorForce = Mathf.Lerp(m_preforces.SwayBehaviorForce, m_param.POTISION, currentTime);
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_setforces.SurgeBehaviorForce = Mathf.Lerp(m_preforces.SurgeBehaviorForce, m_param.POTISION, currentTime);
                break;

        }

        m_mover.MoverTimeLineCall(m_setforces, m_param.SIXAXIS);

    }
}
