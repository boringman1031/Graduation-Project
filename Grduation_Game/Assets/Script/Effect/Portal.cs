using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("¼s¼½¨Æ¥ó")]
    public VoidEventSO ProtalEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           ProtalEvent.OnEventRaised();
        }
    }
}
