using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public struct SIMVRPBSinWaveParam
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
public class SIMVRPBSinWave : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public SixBehaviorForces m_sixbehavior;
    public SIMVRPBSinWaveParam m_sinwavebehaviorparam;

    private float angle = 0.0f;

    int randstep;
    private bool pingpongmode = true;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {

        randstep = (int)m_sinwavebehaviorparam.m_anglestep;

    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

        angle = m_sinwavebehaviorparam.m_startangle;
    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        if (m_sinwavebehaviorparam.m_pingpongflg)
        {
            if(pingpongmode)
            {
                //行き
                angle = Mathf.MoveTowards(angle, m_sinwavebehaviorparam.m_endangle, randstep);

                if (angle >= m_sinwavebehaviorparam.m_endangle)
                {
                    angle = m_sinwavebehaviorparam.m_endangle;
                   
                    pingpongmode = !pingpongmode;

                }

            }
            else
            {
                //帰り
                angle = Mathf.MoveTowards(angle, m_sinwavebehaviorparam.m_startangle, randstep);

                if (angle <= m_sinwavebehaviorparam.m_startangle)
                {
                    angle = m_sinwavebehaviorparam.m_startangle;
                    if (m_sinwavebehaviorparam.m_randflg)
                    {
                        randstep = Random.Range(1, (int)m_sinwavebehaviorparam.m_anglestep + 1);

                    }
                    pingpongmode = !pingpongmode;

                }
            }
        }
        else
        {

            //角度計算
            angle = Mathf.MoveTowards(angle, m_sinwavebehaviorparam.m_endangle, randstep);

            if (angle >= m_sinwavebehaviorparam.m_endangle)
            {
                angle = m_sinwavebehaviorparam.m_startangle;
                if (m_sinwavebehaviorparam.m_randflg)
                {
                    randstep = Random.Range(1, (int)m_sinwavebehaviorparam.m_anglestep + 1);
                }

            }
        }


        //データを毎フレーム入れ込む
        switch (m_sinwavebehaviorparam.SixAxis)
        {
            case SIMVRMotionSixAxis.ROLL:
                m_sixbehavior.RollBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.PITCH:
                m_sixbehavior.PitchBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.YAW:
                m_sixbehavior.YawBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.HEAVE:
                m_sixbehavior.HeaveBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.SWAY:
                m_sixbehavior.SwayBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

            case SIMVRMotionSixAxis.SURGE:
                m_sixbehavior.SurgeBehaviorForce = Mathf.Sin(angle * Mathf.Deg2Rad);
                break;

        }

        //データを毎フレーム入れ込む
        m_mover.MoverTimeLineCall(m_sixbehavior, m_sinwavebehaviorparam.SixAxis);
    }
}
