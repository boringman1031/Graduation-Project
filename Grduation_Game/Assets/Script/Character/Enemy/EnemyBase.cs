/*---------------BY017-----------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;

    [Header("事件廣播")]
    public EnemyEventSO OnEnemyDied;

    [Header("基礎數值")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;//面向方向
    public Transform attacker;//攻擊者
    public float HitForce;//受傷擊退力度

    [Header("檢測")]
    public Vector2 centerOffset;//檢測中心點偏移
    public Vector2 checkSize;//檢測範圍
    public float checkDistance;//檢測距離
    public LayerMask targetLayer;//檢測目標層

    [Header("狀態")]
    public bool isHit;
    public bool isDead;

    [Header("計時器")]
    public float waitTime;//巡邏等待時間
    [HideInInspector] public float waitTimeCounter;
    [HideInInspector] public bool isWait; 
    public float lostTime;
    [HideInInspector] public float lostTimeCounter;

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
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if((physicsCheck.touchLeftWall&&faceDir.x<0)||(physicsCheck.touchRightWall&&faceDir.x>0))//碰到牆壁轉向      
        {
            isWait = true;
            anim.SetBool("Run", false);
        }
        TimeCounter();
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
        rb.velocity = new Vector2(currentSpeed * faceDir.x*Time.deltaTime, rb.velocity.y);
    }
    public void TimeCounter()
    {
        if(isWait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                isWait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 10, 10);
            }
        }
        if (!FindPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }  

    }
    public bool FindPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, targetLayer);
    }

    public void SwitchState(NPCState _state)
    {
        var newState = _state switch//根據現有狀態切換敵人狀態(switch的語法糖寫法)
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    #region 事件執行方法
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
        Vector2 dir = new Vector2(transform.position.x - attackTran.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHit(dir));
    }

    IEnumerator OnHit(Vector2 dir)
    {
        rb.AddForce(dir * HitForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    public virtual void OnDead()
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
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
