using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class VideoManagerPlayableAsset : PlayableAsset
{
    [SerializeField]
    public ExposedReference<VideoManager> manager;

    [SerializeField]
    public ExposedReference<PlayableDirector> director;


    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {

        var behaviour = new VideoManagerPlayableBehavior();
        var handle = ScriptPlayable<VideoManagerPlayableBehavior>.Create(graph);

        handle.GetBehaviour().manager = manager.Resolve(graph.GetResolver());
        handle.GetBehaviour().director = director.Resolve(graph.GetResolver());

        return handle;
    }
}
