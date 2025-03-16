using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [Header("¼s¼½¨Æ¥ó")]
    public VoidEventSO AttackBossEvent;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnDead()
    {
        gameObject.layer = 2;
        anim.SetBool("Dead", true);
        AttackBossEvent.OnEventRaised();       
    }

    public void DestoryObject()
    {
        Destroy(gameObject);
    }
}
