/*------------------by 017-----------------------*/
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public  PlayerInput playerInput;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;

    [Header("���a���z�ƭ�")]
    public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce;//���a����ˮ`���h�O

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
        playerInput.Enable();   
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        inputDirection = playerInput.GamePlay.Move.ReadValue<Vector2>();
        /* if (rb == null || statsManager == null) return;

         rb.velocity = new Vector2(statsManager.Speed.CurrentValue * inputX, rb.velocity.y);
         animator.SetFloat("yVelocity", rb.velocity.y);

         if (rb.velocity.sqrMagnitude == 0)
         {
             animator.SetBool("isRun", false);
             Player_front.SetActive(true);
             Player_side.SetActive(false);
         }

         float velocityX = rb.velocity.x;
         if ((velocityX < 0 && !isFlip) || (velocityX > 0 && isFlip))
         {
             isFlip = !isFlip;
             transform.Rotate(0, 180, 0);
         }*/
    }

    private void FixedUpdate()
    {
        if(!ishurt && !isAttack)
            Player_Move();             
    }

    public void Player_Move( )
    {       
        rb.velocity = new Vector2(inputDirection.x * Speed*Time.deltaTime, rb.velocity.y);

        //�H��½��
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = -1;
        else if (inputDirection.x < 0)     
            faceDir = 1;      
        transform.localScale = new Vector3(faceDir,1,1);
    }

    public void Player_Jump( InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jampforce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    public void Player_Attack(InputAction.CallbackContext obj)
    {
       playerAnimation.OnPlayerAttack();
        isAttack = true;              
    }

    #region  �H�U���bUnityEvent�����泡��
    public void Player_GetHurt(Transform _attacker)//��������
    {
        ishurt = true;
        rb.velocity=Vector2.zero;
        Vector2 die=new Vector2((transform.position.x - _attacker.position.x), 0).normalized;

        rb.AddForce(die * Hurtforce, ForceMode2D.Impulse);
    }

    public void Player_Dead()
    {
        isDead = true;
        playerInput.GamePlay.Disable();
    }
    #endregion
}
