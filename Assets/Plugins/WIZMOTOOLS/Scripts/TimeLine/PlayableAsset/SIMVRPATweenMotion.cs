using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPATweenMotion : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;

    [SerializeField]
    SIMVRMotionSixAxis m_axis;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    float m_startPosition;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    float m_endPosition;

    [SerializeField]
    bool m_continuousFlg;


    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBUniformMotion();
        var handle = ScriptPlayable<SIMVRPBTweenMotion>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;
        //setparam.DIRECTION = m_direction;

        handle.GetBehaviour().m_startpos = m_startPosition;
        handle.GetBehaviour().m_endpos = m_endPosition;
        handle.GetBehaviour().m_axis = m_axis;
        handle.GetBehaviour().m_continuousflg = m_continuousFlg;
        return handle;
	}
}
