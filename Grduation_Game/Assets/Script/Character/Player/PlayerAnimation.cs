/*-------------BY017--------*/
/*--------玩家動畫控制-----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
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
        // 🔁 重啟 SpriteSkin，重綁骨架
        SpriteSkin skin = GetComponent<SpriteSkin>();
        if (skin != null)
        {
            skin.enabled = false;
            skin.enabled = true;
        }

        // ✅ 強制播放 Idle 動畫（注意名稱一定要正確）
        anim.Play("Player_Idle", 0, 0f); // 第二個參數為 Layer，第三為時間（從頭播）

        Debug.Log("已強制播放 Idle 動畫");

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

