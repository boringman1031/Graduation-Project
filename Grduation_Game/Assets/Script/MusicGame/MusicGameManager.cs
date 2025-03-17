using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameManager : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    [Header("特效")]
    public GameObject hitEffect;
    public GameObject missEffect; 
    public Transform effectPos;

    public bool startPlaying;

    public BeatScroller theBS;

    private void Start()
    {
        theBS = FindObjectOfType<BeatScroller>();
    }
    private void OnEnable()
    {
        onNoteHit.OnEventRaised += NoteHit; // 註冊 NoteHit 事件到 onNoteHit.
        onNoteMiss.OnEventRaised += NoteMiss; // 註冊 NoteMiss 事件到 onNoteMiss.
    }

    private void OnDisable()
    {
        onNoteHit.OnEventRaised -= NoteHit; // 註冊 NoteHit 事件到 onNoteHit.
        onNoteMiss.OnEventRaised -= NoteMiss; // 註冊 NoteMiss 事件到 onNoteMiss.
    }
    private void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;           
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        Instantiate(hitEffect, effectPos.position,Quaternion.identity);
    }

    public void NoteMiss()
    {
        Debug.Log("Missed");
        Instantiate(missEffect, effectPos.position, Quaternion.identity);
    }
}
