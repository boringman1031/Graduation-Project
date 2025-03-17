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
    [Header("�ƥ��ť")]
    public SceneLoadEventSO SceneloadEvent;//�����[���ƥ�
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayerInput playerInput;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;

    [Header("���a���z�ƭ�")]
    public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce;//���a����ˮ`���h�O

    [Header("�S��")]
    public GameObject attackEffectPrefab;//�����S��
    public GameObject DeadEffectPrefab;//���`�S��
    public Transform attackEffectPos;//�����S�ĥͦ���m

    [Header("���a���A")]
    public bool ishurt;//�O�_����
    public bool isDead;//�O�_���`
    public bool isAttack;//�O�_����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();

        playerInput = new PlayerInput();
        //���D�ƥ�
        playerInput.GamePlay.Jump.started += Player_Jump;
        //�����ƥ�
        playerInput.GamePlay.Attack.started += Player_Attack;
       

    }

    private void OnEnable()
    {      
        SceneloadEvent.LoadRequestEvent += OnLoadEvent;//�����[���ɰ���a����
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;//�����[�������}�_���a����
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//Ū���C���i�רƥ�
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;//��^�D���ƥ�
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
    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)//�����[���ɰ���a����
    {
        playerInput.GamePlay.Disable();          
    }
    private void OnLoadDataEvent()//Ū���C���i�רƥ�
    {
        isDead = false;
    }

    private void OnAfterSceneLoadEvent()//�����[�������}�_���a����
    {
        playerInput.GamePlay.Enable();     
    }
    public void Player_Move()
    {       
        rb.velocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.velocity.y);

        //�H��½��
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

    public void Player_AttackEffect()//�����S�ĥͦ�
    {
        Instantiate(attackEffectPrefab, attackEffectPos.position, Quaternion.identity);
    }

    public void Player_DeadEffect()//���`�S�ĥͦ�
    {
        Instantiate(DeadEffectPrefab, transform.position, Quaternion.identity);
    }
    #region  �H�U���bUnityEvent�����泡��
    public void Player_GetHurt(Transform _attacker)//��������
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
