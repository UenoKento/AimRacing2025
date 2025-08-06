using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class VideoManagerPlayableBehavior : PlayableBehaviour
{
    public VideoManager manager;
    private float oldtime;

    public PlayableDirector director { get; set; }

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable) {

       

        //Debug.Log("OnGraphStart");
    }

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable) {

        


        //Debug.Log("OnGraphStop");
    }

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info) {

        //manager.Start(playable.GetTime());
        //manager.VideoStart();
        //Debug.Log("OnBehaviourPlay");

    }

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info) {


      

        //Debug.Log("OnBehaviourPause");
    }

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info) {
        float time = (float)playable.GetTime();

        if(Mathf.Abs(oldtime - time) > 0.1f)
        {
#if UNITY_EDITOR
            director.Pause();

            manager.SetTime(time);
#else
            //director.Pause();
            director.Play();
            //manager.SetTime(time);
#endif

        }
        //director.Play();


        oldtime = time;

    }
}
