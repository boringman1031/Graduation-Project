﻿/*-------------BY017--------*/
/*--------玩家動畫控制-----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputSettings;

public class PlayerAnimation : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadedEvent;

    private Animator anim;

   private Rigidbody2D rb;
   private PhysicsCheck physicsCheck;
   private PlayerController playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += UpdateAnimator;
    }
    private void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= UpdateAnimator;
    }
    private void Update()
    {
        SetAnimation();      
    }

    public void UpdateAnimator()//轉場完後更新動畫
    {
        anim.Rebind();
        anim.Update(0);

        // ✅ 清空 trigger，避免攻擊、受傷卡住
        anim.ResetTrigger("Hit");
        anim.ResetTrigger("Attack");

        // ✅ 重設動畫狀態參數
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs( rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
    }

    public void OnPlayerHurt()
    {
        anim.SetTrigger("Hit");
    }
    public void OnPlayerAttack()
    {
        anim.SetTrigger("Attack");      
    }
}

