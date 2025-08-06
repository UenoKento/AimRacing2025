using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPASpeed : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;
    
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_speedAxis = 0.5f;

    [SerializeField]
    private bool m_revertFlg;
    
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBSpeed();
        var handle = ScriptPlayable<SIMVRPBSpeed>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;

        SIMVRPBSpeedParam setparam = new SIMVRPBSpeedParam();
        setparam.Speed = m_speedAxis;

        handle.GetBehaviour().m_speedparam = setparam;
        handle.GetBehaviour().m_revertflg = m_revertFlg;
        return handle;
	}
}
