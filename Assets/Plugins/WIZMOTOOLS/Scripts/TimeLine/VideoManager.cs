using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Video;
using UnityEngine.Playables;

public class VideoManager : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    private float videotime;

    public PlayableDirector timeline;

    [SerializeField]
    float seektime;

    bool seekflg;
    private float setframe;
    private int refreshCount = 0;
    private XRInputSubsystem xrInputSystem = null;

    // Use this for initialization
    void Start()
    {
        videotime = 0.0f;
        seekflg = false;
        videoPlayer.prepareCompleted += SetFrameCount;
        videoPlayer.Prepare();
        videoPlayer.frameReady += OnNewFrame;

        List<UnityEngine.XR.XRInputSubsystem> subsystems = new List<UnityEngine.XR.XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        foreach (var subsystem in subsystems)
        {
            if (subsystem.SubsystemDescriptor.id.Equals("WVR Input Provider"))
            {
                xrInputSystem = subsystem;
                break;
            }
        }
    }

    void SetFrameCount(VideoPlayer vp)
    {
        Debug.Log(videoPlayer.frameCount);
    }

    void SeekCompleted(VideoPlayer vp)
    {
        videoPlayer.Play();
        timeline.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                timeline.Play();
            }
            else
            {
                videoPlayer.Pause();
                timeline.Pause();
            }

        }

        if (videoPlayer.isPlaying && !seekflg)
        {
            videotime += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            videotime += seektime;
            seekflg = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            videotime -= seektime;
            seekflg = true;

            if (videotime < 0)
                videotime = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            videoPlayer.time = videotime;
            seekflg = false;
        }

		if (Input.GetKeyDown (KeyCode.R)) {
            if(xrInputSystem != null)
                xrInputSystem.TryRecenter();
		}
    }

    public void SetTime(float param)
    {
        if (Application.isPlaying)
        {
            float setframe = (float)param * 30;
            videoPlayer.Pause();
            timeline.Pause();

            //videoPlayer.s

            videoPlayer.frame = (long)setframe;
            videoPlayer.Prepare();
            videoPlayer.seekCompleted += SeekCompleted;
        }
    }

    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        refreshCount++;
        // 10フレームに1回更新する
        if (refreshCount >= 10)
        {
           // rawImage.texture = source.texture;
            refreshCount = 0;
        }
        print("OnNewFrame");
    }


    public void VideoStart()
    {
        if (Application.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    public void VideoStart(float seektime)
    {
        if (Application.isPlaying)
        {
            float setframe = (float)seektime * 30;
            videoPlayer.Pause();
            timeline.Pause();

            videoPlayer.frame = (long)setframe;

            videoPlayer.seekCompleted += SeekCompleted;
        }
    }


    public void VideoPause()
    {
        videoPlayer.Pause();
    }

    public void Stop()
    {
        videoPlayer.Stop();
        timeline.Stop();
    }

    public void Pause()
    {
        videoPlayer.Pause();
        timeline.Pause();
    }

    public void Start(double time)
    {
        videotime = (float)time;
        Debug.Log(videotime);
        videoPlayer.time = 0;
        videoPlayer.Play();
        
    }
}
