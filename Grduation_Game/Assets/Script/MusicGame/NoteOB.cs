using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteOB : MonoBehaviour
{
    [Header("¼s¼½¨Æ¥ó")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    public bool canBePressed;

    public KeyCode keyToPress;

    private void Update()
    {
        
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
                onNoteHit.OnEventRaised();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Activator"&&gameObject.activeSelf)
        {
            canBePressed = false;
            onNoteMiss.OnEventRaised();
        }
    }
}
