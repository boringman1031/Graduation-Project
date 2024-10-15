using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem   ;

public class PlayerBase : MonoBehaviour,GameData
{   
    Rigidbody2D Player_rb;
    private float InputX,InputY;
    private bool isFlip = false;
    public int Health { get; set; }
    public int Defence { get; set; }
    public float Speed { get; set; }
    public int Power { get; set; }
   public int Attack { get; set; }

    void Awake()
    {
        Player_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Player_rb.velocity = new Vector2( Speed*InputX, Speed*InputY);
    }   
    public void PlayerMove(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        InputX = context.ReadValue<Vector2>().x;
        InputY = context.ReadValue<Vector2>().y;
        if((Player_rb.velocity.x  < 0 && !isFlip) || (Player_rb.velocity.x > 0 && isFlip))
        {
            isFlip = !isFlip;
            transform.Rotate(0, 180, 0);
        }
    }
    public void PlayerAttack()
    {
        Debug.Log("Player Attack");
    }

    private void PlayerHit(int _damage)
    {
        Health -= _damage;
        
    }
    public void PlayerDie()
    {
        Debug.Log("Player Die");
    }       
}
