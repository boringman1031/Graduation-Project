/*------------------by 017-----------------------*/
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Player_front;
    [SerializeField] private GameObject Player_side;

    public  PlayerInput playerInput;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    [Header("玩家基礎數值")]
    public float Speed = 10f;
    public float jampforce = 16.5f;

    public  int attackState = 0;//攻擊狀態
    public int PlayerHp { get; private set; }
    public int PlayerPower { get; private set; }

    /*--------玩家操作事件宣告區------------*/
    public delegate void PlayerGetHit(int damage);
    public event PlayerGetHit OnPlayerHit;

    public delegate void PlayerAttack();
    public event PlayerAttack OnPlayerAttack;

    public delegate void PlayerUseSkill1(int powercoust);
    public event PlayerUseSkill1 OnPlayerUseSkill1;

    public delegate void PlayerUseSkill2(int powercoust);
    public event PlayerUseSkill2 OnPlayerUseSkill2;

    public delegate void PlayerUseSkill3(int powercoust);
    public event PlayerUseSkill3 OnPlayerUseSkill3;

    public delegate void PlayerDie();
    public event PlayerDie OnPlayerDie;
   
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.GamePlay.Jump.started += Player_Jump;
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
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
        Player_Move();     

        if (rb.velocity.sqrMagnitude == 0)
        {
            Player_front.SetActive(true);
            Player_side.SetActive(false);
        }       
    }

    public void Player_Move( )
    {
        Player_front.SetActive(false);
        Player_side.SetActive(true);
        rb.velocity = new Vector2(inputDirection.x * Speed*Time.deltaTime, rb.velocity.y);

        //人物翻轉
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
        }
    }

    public void Player_Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           attackState++;
            if (attackState > 2)
                attackState = 0;
        }
    }

    public void Player_Hit(int damage)
    {
             
            Player_Die();
    }

    private void Player_Die()
    {
        Debug.Log("Player Died");
    }

}
