using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VoidEventSO backToMenuEvent;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    public void OnVideoEnd(VideoPlayer vp)
    {
        backToMenuEvent.OnEventRaised();
    }

}
