using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteOB : MonoBehaviour
{ 
    public bool canBePressed;

    public KeyCode keyToPress;

    private void Update()
    {
        
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
                GetComponent<MusicGameManager>().NoteHit();
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
            GetComponent<MusicGameManager>().NoteMiss();
        }
    }
}
