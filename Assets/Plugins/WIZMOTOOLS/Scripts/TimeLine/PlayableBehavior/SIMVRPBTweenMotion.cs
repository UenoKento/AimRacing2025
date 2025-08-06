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
public class SIMVRPBTweenMotion : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public float m_startpos;
    public float m_endpos;
    public SIMVRMotionSixAxis m_axis;
    public bool m_continuousflg;
    private SixBehaviorForces m_preforces;
    private SixBehaviorForces m_setforces;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
       // Debug.Log("OnGraphStart");
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {


        if (m_continuousflg)
        {
            m_preforces.RollBehaviorForce = m_mover.CTRL.roll;
            m_preforces.PitchBehaviorForce = m_mover.CTRL.pitch;
            m_preforces.YawBehaviorForce = m_mover.CTRL.yaw;
            m_preforces.HeaveBehaviorForce = m_mover.CTRL.heave;
            m_preforces.SwayBehaviorForce = m_mover.CTRL.sway;
            m_preforces.SurgeBehaviorForce = m_mover.CTRL.surge;

        }
        else
        {
            //現在の位置をSIMVR Controllerから取得する
            m_preforces.RollBehaviorForce = m_startpos;
            m_preforces.PitchBehaviorForce = m_startpos;
            m_preforces.YawBehaviorForce = m_startpos;
            m_preforces.HeaveBehaviorForce = m_startpos;
            m_preforces.SwayBehaviorForce = m_startpos;
            m_preforces.SurgeBehaviorForce = m_startpos;

        }


    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        //毎フレーム実行
        var currentTime = (float)playable.GetTime() / (float)playable.GetDuration();
       
        //データを毎フレーム入れ込む
        switch(m_axis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_setforces.RollBehaviorForce = Mathf.Lerp(m_preforces.RollBehaviorForce, m_endpos, currentTime);
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_setforces.PitchBehaviorForce = Mathf.Lerp(m_preforces.PitchBehaviorForce, m_endpos, currentTime);
                break;

            case SIMVRMotionSixAxis.YAW:
                m_setforces.YawBehaviorForce = Mathf.Lerp(m_preforces.YawBehaviorForce, m_endpos, currentTime);
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_setforces.HeaveBehaviorForce = Mathf.Lerp(m_preforces.HeaveBehaviorForce, m_endpos, currentTime);
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_setforces.SwayBehaviorForce = Mathf.Lerp(m_preforces.SwayBehaviorForce, m_endpos, currentTime);
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_setforces.SurgeBehaviorForce = Mathf.Lerp(m_preforces.SurgeBehaviorForce, m_endpos, currentTime);
                break;

        }

        m_mover.MoverTimeLineCall(m_setforces, m_axis);

    }
}
