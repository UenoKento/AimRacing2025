using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public struct SIMVRPBCosWaveParam
{

    [SerializeField]
    private SIMVRMotionSixAxis m_axis;
    public SIMVRMotionSixAxis SixAxis
    {
        set { m_axis = value; }
        get { return m_axis; }
    }

    public float m_anglestep;
    public float m_startangle;
    public float m_endangle;
    public bool m_randflg;
    public bool m_pingpongflg;

}


// A behaviour that is attached to a playable
public class SIMVRPBCosWave : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public SixBehaviorForces m_sixbehavior;
    public SIMVRPBCosWaveParam m_coswavebehaviorparam;

    private float angle = 0.0f;
    int randstep;
    private bool pingpongmode = true;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {

        randstep = (int)m_coswavebehaviorparam.m_anglestep;
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

        angle = m_coswavebehaviorparam.m_startangle;
    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        if (m_coswavebehaviorparam.m_pingpongflg)
        {
            if (pingpongmode)
            {
                //行き
                angle = Mathf.MoveTowards(angle, m_coswavebehaviorparam.m_endangle, randstep);

                if (angle >= m_coswavebehaviorparam.m_endangle)
                {
                    angle = m_coswavebehaviorparam.m_endangle;
                   
                    pingpongmode = !pingpongmode;

                }

            }
            else
            {
                //帰り
                angle = Mathf.MoveTowards(angle, m_coswavebehaviorparam.m_startangle, randstep);

                if (angle <= m_coswavebehaviorparam.m_startangle)
                {
                    angle = m_coswavebehaviorparam.m_startangle;
                    if (m_coswavebehaviorparam.m_randflg)
                    {
                        randstep = Random.Range(1, (int)m_coswavebehaviorparam.m_anglestep + 1);

                    }
                    pingpongmode = !pingpongmode;

                }
            }
        }
        else
        {

            //角度計算
            angle = Mathf.MoveTowards(angle, m_coswavebehaviorparam.m_endangle, randstep);

            if (angle >= m_coswavebehaviorparam.m_endangle)
            {
                angle = m_coswavebehaviorparam.m_startangle;
                if (m_coswavebehaviorparam.m_randflg)
                {
                    randstep = Random.Range(1, (int)m_coswavebehaviorparam.m_anglestep + 1);
                }

            }
        }

        //データを毎フレーム入れ込む
        switch (m_coswavebehaviorparam.SixAxis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_sixbehavior.RollBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_sixbehavior.PitchBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.YAW:
                m_sixbehavior.YawBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_sixbehavior.HeaveBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_sixbehavior.SwayBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_sixbehavior.SurgeBehaviorForce = Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

        }

        //データを毎フレーム入れ込む
        m_mover.MoverTimeLineCall(m_sixbehavior, m_coswavebehaviorparam.SixAxis);
    }
}
