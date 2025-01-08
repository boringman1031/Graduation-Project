using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem   ;
using Zenject;
/*------------------by 017-----------------------*/
public class PlayerBase : MonoBehaviour
{
    [SerializeField] private GameObject Player_front;
    [SerializeField] private GameObject Player_side;

    private PlayerStats stats;//玩家屬性
    private Rigidbody2D rb;
    private Animator animator;

    private float inputX;
    private int attackState = 0;//攻擊狀態
    private bool isFlip = true;

    public int PlayerHp { get; private set; }
    public int PlayerPower { get; private set; }

    //--------事件宣告區------------
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

    [Inject]
    public void Construct(PlayerStats injectedStats)
    {
        stats = injectedStats;
        InitializeStats();
    }

    private void InitializeStats()
    {
        PlayerHp = stats.Health + stats.Defence;
        PlayerPower = stats.Power;
        Debug.Log($"Player Stats Initialized: HP={PlayerHp}, Power={PlayerPower}");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleFlip();
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(stats.Speed * inputX, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.sqrMagnitude == 0)
        {
            animator.SetBool("isRun", false);
            Player_front.SetActive(true);
            Player_side.SetActive(false);
        }
    }

    private void HandleFlip()
    {
        float velocityX = rb.velocity.x;
        if ((velocityX < 0 && !isFlip) || (velocityX > 0 && isFlip))
        {
            isFlip = !isFlip;
            transform.Rotate(0, 180, 0);
        }
    }

    public void Player_Move(InputAction.CallbackContext context)
    {
        Player_front.SetActive(false);
        Player_side.SetActive(true);
        animator.SetBool("isRun", true);
        inputX = context.ReadValue<Vector2>().x;
    }

    public void Player_Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetBool("isGround", false);
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    public void Player_Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (attackState)
            {
                case 0:
                    animator.SetTrigger("Attack1");
                    attackState += 1;
                    break;
                case 1:
                    animator.SetTrigger("Attack2");
                    attackState += 1;
                    break;
                case 2:
                    animator.SetTrigger("Attack3");
                    attackState = 0;
                    break;
            }
        }
    }

    public void Player_Hit(int damage)
    {
        PlayerHp -= damage;
        OnPlayerHit?.Invoke(damage);

        if (PlayerHp <= 0)
        {
            Player_Die();
        }
    }

    private void Player_Die()
    {
        Debug.Log("Player Died");
        OnPlayerDie?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isGround", true);
        }
    }
}
