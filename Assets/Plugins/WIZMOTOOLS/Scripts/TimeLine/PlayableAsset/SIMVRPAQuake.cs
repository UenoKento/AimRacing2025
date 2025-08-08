using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPAQuake : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;

    [SerializeField]
    SIMVRMotionSixAxis m_axis;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Max = 1.0f;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Min = -1.0f;

  

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBQuake();
        var handle = ScriptPlayable<SIMVRPBQuake>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());

        SIMVRPBQuakeParam setparam = new SIMVRPBQuakeParam();

        setparam.SixAxis = m_axis;
        setparam.QuakeMax = m_Max;
        setparam.QuakeMin = m_Min;
        handle.GetBehaviour().m_quakebehaviorparam = setparam;
        return handle;
	}
}
