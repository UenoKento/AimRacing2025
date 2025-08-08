    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SIMVRPlayableBehaviour : PlayableBehaviour
{
    public WIZMOTimeLineMover m_mover;
    public int m_csvrownum;
    public SixBehaviorForces m_sixbehavior;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {
		
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
        if (m_mover == null)
        {
            return;
        }

    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {

        if (m_mover == null)
        {
            return;
        }

        m_mover.MoverTimeLineCall(m_sixbehavior);

        SIMVRPBSpeedParam speed = new SIMVRPBSpeedParam();
        SIMVRPBAccelerationParam accel = new SIMVRPBAccelerationParam();
        speed.Speed = m_sixbehavior.Speed;
        accel.Accel = m_sixbehavior.Accel;

        m_mover.MoverTimeLineSetSpeed(speed);
        m_mover.MoverTimelineSetAccelerator(accel);

    }
}
