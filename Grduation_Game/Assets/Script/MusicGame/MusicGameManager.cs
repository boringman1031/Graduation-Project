using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameManager : MonoBehaviour
{  
    public bool startPlaying;

    public BeatScroller theBS;

    private void Start()
    {
        theBS = FindObjectOfType<BeatScroller>();
    }

    private void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;
                GetComponent<AudioDefination>().PlayAudioClip();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
    }

    public void NoteMiss()
    {
        Debug.Log("Missed");
    }
}
