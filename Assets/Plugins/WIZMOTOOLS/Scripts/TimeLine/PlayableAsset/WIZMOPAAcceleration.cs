using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class WIZMOPAAcceleration : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;
    
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_accelerateAxis = 0.5f;

    [SerializeField]
    private bool m_revertFlg;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBSpeed();
        var handle = ScriptPlayable<SIMVRPBAcceleration>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;
        handle.GetBehaviour().m_revertflg = m_revertFlg;

        SIMVRPBAccelerationParam setparam = new SIMVRPBAccelerationParam();
        //setbehavior.HeaveBehaviorForce = setbehavior.PitchBehaviorForce = setbehavior.RollBehaviorForce = 0;
        setparam.Accel = m_accelerateAxis;

        handle.GetBehaviour().m_param = setparam;
        return handle;
	}
}
