/*---------------BY017-----------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("事件廣播")]
    public EnemyEventSO OnEnemyDied;

    [Header("基礎數值")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector]public float currentSpeed;
    public Vector3 faceDir;//面向方向
    public Transform attacker;//攻擊者
    public float HitForce;//受傷擊退力度

    [Header("狀態")]
    public bool isHit;
    public bool isDead;

    private BaseState currentState;//當前狀態
    protected BaseState patrolState;//巡邏狀態
    protected BaseState chaseState;//追擊狀態
   
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    public void Update()
    {
        faceDir =new Vector3(- transform.position.x, 0, 0); 
        currentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        if (!isHit & !isDead) 
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }

    public void OnTakeDamage(Transform attackTran)
    {
        attacker = attackTran;
        //受傷後面向攻擊者
        if (attackTran.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTran.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        //受傷被擊退
        isHit = true;
        anim.SetTrigger("Hit");
        Vector2 dir = new Vector2(transform.position.x-attackTran.position.x,0).normalized;
       StartCoroutine(OnHit(dir));
    }

    IEnumerator OnHit(Vector2 dir)
    {
        rb.AddForce(dir * HitForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    public void OnDead()
    {     
        gameObject.layer = 2;
        anim.SetBool("Dead", true);
        isDead = true;
        OnEnemyDied.Raise(this);
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}
