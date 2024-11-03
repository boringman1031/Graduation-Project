using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem   ;

public class PlayerBase : MonoBehaviour,GameData
{
    [SerializeField]
    private GameObject Player_front;
    [SerializeField]
    private GameObject Player_side;

    Rigidbody2D Player_rb;
    Animator Player_animator;
    private float InputX,InputY;
    private bool isFlip=true;
    private int AttackState = 0;
    public int Health { get; set; }
    public int Defence { get; set; }
    public float Speed { get; set; }
    public int Power { get; set; }
   public int Attack { get; set; }

    void Awake()
    {
        Player_rb = GetComponent<Rigidbody2D>();
        Player_animator = GetComponent<Animator>();
        Speed = 5.0f;
    }

    void Update()
    {
        Player_rb.velocity = new Vector2(Speed * InputX, Player_rb.velocity.y);
        Player_animator.SetFloat("yVelocity", Player_rb.velocity.y);
        if (Player_rb.velocity.magnitude == 0)
        {
            Player_animator.SetBool("isRun", false);
            Player_front.SetActive(true);
            Player_side.SetActive(false);          
        }
        if ((Player_rb.velocity.x < 0 && !isFlip) || (Player_rb.velocity.x > 0 && isFlip))
        {
            isFlip = !isFlip;
            transform.Rotate(0, 180, 0);
        }
    }
    public void PlayerMove(InputAction.CallbackContext context)
    {
        Player_front.SetActive(false);
        Player_side.SetActive(true);
        Player_animator.SetBool("isRun", true);
        InputX = context.ReadValue<Vector2>().x;        
    }
    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player_animator.SetBool("isGround", false);
            Player_rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }
    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
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
                default:
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


    private void PlayerHit(int _damage)
    {
        Health -= _damage;
        
    }
    private  void PlayerDie()
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
