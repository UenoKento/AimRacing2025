using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public struct SIMVRPBSpeedParam
{
    [SerializeField]
    private float m_speed;
    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

}


// A behaviour that is attached to a playable
public class SIMVRPBSpeed : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public SIMVRPBSpeedParam m_speedparam;
    private SIMVRPBSpeedParam m_oldparam;
    public bool m_revertflg;
    private bool m_isplay = false;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
      
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

        if (!Application.isPlaying)
            return;


        if (m_revertflg)
        {
            m_oldparam.Speed = m_mover.CTRL.speed1_all;
            m_isplay = true;
        }

    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {

        if (!Application.isPlaying)
            return;


        if (m_revertflg && m_isplay)
        {
            m_mover.CTRL.speed1_all = m_oldparam.Speed;
        }
    }

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        //毎フレーム実行

        //データを毎フレーム入れ込む
        m_mover.MoverTimeLineSetSpeed(m_speedparam);
    }
}
