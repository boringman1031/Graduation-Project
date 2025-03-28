/*---------------BY017-----------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
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
    public float attackRange;//攻擊範圍
    public float attackCooldown; // 攻擊冷卻時間

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
    public float lostTime;//追擊失敗等待時間
    [HideInInspector] public float waitTimeCounter;
    [HideInInspector] public bool isWait;  
    [HideInInspector] public float lostTimeCounter;

    private BaseState currentState;//當前狀態
    protected BaseState patrolState;//巡邏狀態
    protected BaseState chaseState;//追擊狀態
    protected BaseState attackerState;//攻擊狀態
    [HideInInspector] public BaseState idleState;//空閒狀態
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

        // 初始化所有狀態
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
        idleState = new IdleState(); // 新增空閒狀態
    }

    private void OnEnable()
    {       
        // 初始狀態為 IdleState
        currentState = idleState;
        currentState.OnEnter(this);
    }
    private void OnDisable()
    {      
        currentState.OnExit();
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
            OnMove();
        currentState.PhysicsUpdate();
    }
   
    public virtual void OnMove()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x*Time.deltaTime, rb.velocity.y);
    }
    public void OnTakeDamage(Transform attackTran)
    {
        attacker = attackTran;
        //受傷後面向攻擊者
        if (attackTran.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        if (attackTran.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
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
        FindObjectOfType<EnemyManager>()?.HandleEnemyDeath(gameObject);
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
                transform.localScale = new Vector3(faceDir.x,1.6f,1);
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

    public bool PlayerInAttackRange()
    {
        // **獲取玩家位置**
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return false;

        // **計算與玩家的距離**
        float distance = Vector2.Distance(transform.position, player.position);

        // **如果距離小於攻擊範圍，返回 `true`**
        return distance <= attackRange;
    }

    public void SwitchState(EenemyState _state)//切換狀態
    {
        var newState = _state switch//根據現有狀態切換敵人狀態(switch的語法糖寫法)
        {
            EenemyState.Idle => idleState,
            EenemyState.Patrol => patrolState,
            EenemyState.Chase => chaseState,
            EenemyState.Attack => attackerState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }
}
