using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SIMVRPlayableAsset : PlayableAsset
{
    [SerializeField]
    public ExposedReference<WIZMOTimeLineMover> m_mover;

    [SerializeField]
    int m_csvrownum;


    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_rollBehaviorForce = 0.0f;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_pitchBehaviorForce = 0.0f;

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_yawBehaviorForce = 0.0f;
  
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_heaveBehaviorForce = 0.0f;
    
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_swayBehaviorForce = 0.0f;
    
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_surgeBehaviorForce = 0.0f;
    
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_speed = 0.667f;
    
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_accel = 0.5f;
    

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        
        var behaviour = new SIMVRPlayableBehaviour();
        var handle = ScriptPlayable<SIMVRPlayableBehaviour>.Create(graph);
        

        handle.GetBehaviour().m_mover = m_mover.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_csvrownum = m_csvrownum;

        SixBehaviorForces setbehavior = new SixBehaviorForces();
      
        setbehavior.RollBehaviorForce = m_rollBehaviorForce;
        setbehavior.PitchBehaviorForce = m_pitchBehaviorForce;
        setbehavior.YawBehaviorForce = m_yawBehaviorForce;
        setbehavior.HeaveBehaviorForce = m_heaveBehaviorForce;
        setbehavior.SurgeBehaviorForce = m_surgeBehaviorForce;
        setbehavior.SwayBehaviorForce = m_swayBehaviorForce;

        setbehavior.Speed = m_speed;
        setbehavior.Accel = m_accel;

        handle.GetBehaviour().m_sixbehavior = setbehavior;
        return handle;
	}
}
