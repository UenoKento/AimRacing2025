using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public struct SIMVRPBQuakeParam
{

    [SerializeField]
    private SIMVRMotionSixAxis m_axis;
    public SIMVRMotionSixAxis SixAxis
    {
        set { m_axis = value; }
        get { return m_axis; }
    }

    [SerializeField]
    private float m_quakeMax;
    public float QuakeMax
    {
        get { return m_quakeMax; }
        set { m_quakeMax = value; }
    }

    [SerializeField]
    private float m_quakeMin;
    public float QuakeMin
    {
        get { return m_quakeMin; }
        set { m_quakeMin = value; }
    }

}


// A behaviour that is attached to a playable
public class SIMVRPBQuake : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public SixBehaviorForces m_sixbehavior;
    public SIMVRPBQuakeParam m_quakebehaviorparam;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
        //Debug.Log("OnGraphStart");
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
        
        if (m_mover == null)
        {
            Debug.Log("moverが入ってない");
            return;
        }

     
    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        //毎フレーム実行
        //データを毎フレーム入れ込む
        switch (m_quakebehaviorparam.SixAxis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_sixbehavior.RollBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_sixbehavior.PitchBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

            case SIMVRMotionSixAxis.YAW:
                m_sixbehavior.YawBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_sixbehavior.HeaveBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_sixbehavior.SwayBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_sixbehavior.SurgeBehaviorForce = Random.Range(m_quakebehaviorparam.QuakeMin, m_quakebehaviorparam.QuakeMax);
                break;

        }

        //データを毎フレーム入れ込む
        m_mover.MoverTimeLineCall(m_sixbehavior, m_quakebehaviorparam.SixAxis);
    }
}
