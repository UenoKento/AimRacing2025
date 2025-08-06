using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPASinWave : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;

    [SerializeField]
    SIMVRMotionSixAxis m_axis;

    [SerializeField]
    [Range(0, 360)]
    private float m_step = 1.0f;

    [SerializeField]
    [Range(0, 360)]
    private float m_startAngle = 0.0f;

    [SerializeField]
    [Range(0, 360)]
    private float m_endAngle = 0.0f;

    [SerializeField]
    private bool m_randFlg;

    [SerializeField]
    private bool m_pingpongFlg;


    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPBSinWave();
        var handle = ScriptPlayable<SIMVRPBSinWave>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());

        SIMVRPBSinWaveParam setparam = new SIMVRPBSinWaveParam();

        setparam.SixAxis = m_axis;
        setparam.m_anglestep = m_step;
        setparam.m_startangle = m_startAngle;
        setparam.m_endangle = m_endAngle;
        setparam.m_randflg = m_randFlg;
        setparam.m_pingpongflg = m_pingpongFlg;

        handle.GetBehaviour().m_sinwavebehaviorparam = setparam;
        return handle;
	}
}
