/*------------------by 017-----------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    [Header("事件監聽")]
    public SceneLoadEventSO SceneloadEvent;//場景加載事件
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayerInput playerInput;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;

    [Header("玩家物理數值")]
    public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce;//玩家受到傷害擊退力

    [Header("特效")]
    public GameObject attackEffectPrefab;//攻擊特效
    public GameObject DeadEffectPrefab;//死亡特效
    public Transform attackEffectPos;//攻擊特效生成位置

    [Header("玩家狀態")]
    public bool ishurt;//是否受傷
    public bool isDead;//是否死亡
    public bool isAttack;//是否攻擊

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();

        playerInput = new PlayerInput();
        //跳躍事件
        playerInput.GamePlay.Jump.started += Player_Jump;
        //攻擊事件
        playerInput.GamePlay.Attack.started += Player_Attack;
       

    }

    private void OnEnable()
    {      
        SceneloadEvent.LoadRequestEvent += OnLoadEvent;//場景加載時停止玩家控制
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;//場景加載完成開起玩家控制
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//讀取遊戲進度事件
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;//返回主選單事件
    }

    private void OnDisable()
    {
        playerInput.Disable();
        SceneloadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void Update()
    {
        inputDirection = playerInput.GamePlay.Move.ReadValue<Vector2>();
    } 

    private void FixedUpdate()
    {
        if (!ishurt && !isAttack)
            Player_Move();
    }
    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)//場景加載時停止玩家控制
    {
        playerInput.GamePlay.Disable();          
    }
    private void OnLoadDataEvent()//讀取遊戲進度事件
    {
        isDead = false;
    }

    private void OnAfterSceneLoadEvent()//場景加載完成開起玩家控制
    {
        playerInput.GamePlay.Enable();     
    }
    public void Player_Move()
    {       
        rb.velocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.velocity.y);

        //人物翻轉
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = -1;
        else if (inputDirection.x < 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }



    public void Player_Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jampforce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    public void Player_Attack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.OnPlayerAttack();    
        isAttack = true;
    }

    public void Player_AttackEffect()//攻擊特效生成
    {
        Instantiate(attackEffectPrefab, attackEffectPos.position, Quaternion.identity);
    }

    public void Player_DeadEffect()//死亡特效生成
    {
        Instantiate(DeadEffectPrefab, transform.position, Quaternion.identity);
    }
    #region  以下為在UnityEvent中執行部分
    public void Player_GetHurt(Transform _attacker)//受傷擊飛
    {
        ishurt = true;
        rb.velocity = Vector2.zero;
        Vector2 die = new Vector2((transform.position.x - _attacker.position.x), 0).normalized;

        rb.AddForce(die * Hurtforce, ForceMode2D.Impulse);
    }

    public void Player_Dead()
    {
        isDead = true;
        playerInput.GamePlay.Disable();
    }
    #endregion
}
