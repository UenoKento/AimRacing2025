using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPAUniformMotion : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;

    [SerializeField]
    SIMVRMotionSixAxis m_axis;

    //[SerializeField]
    //SIMVRMotionDirecion m_direction;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    float m_position;


    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBUniformMotion();
        var handle = ScriptPlayable<SIMVRPBUniformMotion>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;

        SIMVRPAUniformMotionParam setparam = new SIMVRPAUniformMotionParam();
        setparam.SIXAXIS = m_axis;
        //setparam.DIRECTION = m_direction;
        setparam.POTISION = m_position;

        handle.GetBehaviour().m_param = setparam;
        return handle;
	}
}
