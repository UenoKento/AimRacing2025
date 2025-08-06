using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public struct SIMVRPBAccelerationParam
{
    [SerializeField]
    private float m_accel;
    public float Accel
    {
        get { return m_accel; }
        set { m_accel = value; }
    }
}


// A behaviour that is attached to a playable
public class SIMVRPBAcceleration : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public SIMVRPBAccelerationParam m_param;
    private SIMVRPBAccelerationParam m_oldparam;
    public bool m_revertflg;
    private bool m_isplay = false;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {

        if (!Application.isPlaying)
            return;

        if (m_revertflg)
        {
            m_oldparam.Accel= m_mover.CTRL.accel;
            m_isplay = true;
        }
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
        
     
    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {

        if (!Application.isPlaying)
            return;


        if (m_revertflg && m_isplay)
        {
            m_mover.CTRL.accel = m_oldparam.Accel;
        }
    }

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        //毎フレーム実行
        var currentTime = (float)playable.GetTime() / (float)playable.GetDuration();
        //Debug.Log(currentTime);
        //データを毎フレーム入れ込む
        m_mover.MoverTimelineSetAccelerator(m_param);
    }
}
