using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem   ;
using static PlayerBase;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private GameObject Player_front;
    [SerializeField]
    private GameObject Player_side;
    //--------基礎角色數值------------
    [SerializeField]
    private int Health = 100;
    [SerializeField]
    private int Defence = 100;
    [SerializeField]
    private float Speed = 10.0f;
    [SerializeField]
    private int Power = 100;
    [SerializeField]
    private int Attack = 100;
   
    public  int PlayerHp;
    public int PlayerPower;

    Rigidbody2D Player_rb;
    Animator Player_animator;
    private float InputX, InputY;
    private float attack_CD = 1.0f;
    private float attackTimer;
    private int AttackState = 0;//打第幾下了
    private bool isFlip = true;

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

    void Awake()
    {
        Player_rb = GetComponent<Rigidbody2D>();
        Player_animator = GetComponent<Animator>();
        PlayerHp = PlayerHp + Defence;//玩家生命值為血量+防禦力
        PlayerPower= Power;//玩家魔力值
    }

    void Update()
    {
        // 玩家移動邏輯
        Player_rb.velocity = new Vector2(Speed * InputX, Player_rb.velocity.y);
        Player_animator.SetFloat("yVelocity", Player_rb.velocity.y);

        // 攻擊計時器
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            AttackState = 0;
        }

        if (Player_rb.velocity.sqrMagnitude == 0)
        {
            Player_animator.SetBool("isRun", false);
            Player_front.SetActive(true);
            Player_side.SetActive(false);
        }

        // 玩家翻轉邏輯
        float velocityX = Player_rb.velocity.x;
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
        Player_animator.SetBool("isRun", true);
        InputX = context.ReadValue<Vector2>().x;
    }

    public void Player_Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player_animator.SetBool("isGround", false);
            Player_rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    public void Player_Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackTimer = attack_CD;
            switch (AttackState)
            {
                case 0:
                    Player_animator.SetTrigger("Attack1");
                    AttackState += 1;
                    break;
                case 1:
                    Player_animator.SetTrigger("Attack2");
                    AttackState += 1;
                    break;
                case 2:
                    Player_animator.SetTrigger("Attack3");
                    AttackState = 0;
                    break;
            }
        }
    }

    public void PlayerSkill1(InputAction.CallbackContext context)
    {
        Debug.Log("Player Connected");

    }

    public void PlayerSkill2(InputAction.CallbackContext context)
    {
        Debug.Log("Player Skill2");
    }

    public void PlayerSkill3(InputAction.CallbackContext context)
    {
        Debug.Log("Player Skill3");
    }

    public void Player_Hit(int _damage)
    {
        PlayerHp -= _damage;
        OnPlayerHit?.Invoke(_damage);
        // 判斷死亡
        if (PlayerHp <= 0)
        {
            Player_Die();
        }
    }

    private void Player_Die()
    {
        Debug.Log("Player Die");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Player_animator.SetBool("isGround", true);
        }
    }
}
