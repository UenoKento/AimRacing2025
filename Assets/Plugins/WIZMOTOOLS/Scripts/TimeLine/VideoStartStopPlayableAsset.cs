using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class VideoStartStopPlayableAsset : PlayableAsset
{
    [SerializeField]
    public ExposedReference<VideoManager> manager;

    [SerializeField]
    public bool m_startcheck;

    [SerializeField]
    public float m_seektime = 0;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {

        var behaviour = new VideoStartStopPlayableBehaviour();
        var handle = ScriptPlayable<VideoStartStopPlayableBehaviour>.Create(graph);

        handle.GetBehaviour().manager = manager.Resolve(graph.GetResolver());
        handle.GetBehaviour().m_startcheck = m_startcheck;
        handle.GetBehaviour().m_seektime = m_seektime;

        return handle;
    }
}
