using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPASpring : PlayableAsset
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
    [Range(-5.0f, 5.0f)]
    float m_power;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    float m_accelFactor;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    float m_damping;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        

        var handle = ScriptPlayable<SIMVRPBSpring>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;
        //setparam.DIRECTION = m_direction;

        handle.GetBehaviour().m_power = m_power;
        handle.GetBehaviour().m_accelFactor = m_accelFactor;
        handle.GetBehaviour().m_damping = m_damping;
        handle.GetBehaviour().m_axis = m_axis;

        return handle;
	}
}
