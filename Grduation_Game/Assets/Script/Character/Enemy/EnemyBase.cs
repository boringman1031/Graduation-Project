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

    [Header("�ƥ�s��")]
    public EnemyEventSO OnEnemyDied;

    [Header("��¦�ƭ�")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;//���V��V
    public Transform attacker;//������
    public float HitForce;//�������h�O��
    public float attackRange;//�����d��
    public float attackCooldown; // �����N�o�ɶ�

    [Header("�˴�")]
    public Vector2 centerOffset;//�˴������I����
    public Vector2 checkSize;//�˴��d��
    public float checkDistance;//�˴��Z��
    public LayerMask targetLayer;//�˴��ؼмh

    [Header("���A")]
    public bool isHit;
    public bool isDead;

    [Header("�p�ɾ�")]
    public float waitTime;//���޵��ݮɶ�
    public float lostTime;//�l�����ѵ��ݮɶ�
    [HideInInspector] public float waitTimeCounter;
    [HideInInspector] public bool isWait;  
    [HideInInspector] public float lostTimeCounter;

    private BaseState currentState;//��e���A
    protected BaseState patrolState;//���ު��A
    protected BaseState chaseState;//�l�����A
    protected BaseState attackerState;//�������A
    [HideInInspector] public BaseState idleState;//�Ŷ����A
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

        // ��l�ƩҦ����A
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
        idleState = new IdleState(); // �s�W�Ŷ����A
    }

    private void OnEnable()
    {       
        // ��l���A�� IdleState
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
        if((physicsCheck.touchLeftWall&&faceDir.x<0)||(physicsCheck.touchRightWall&&faceDir.x>0))//�I�������V      
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
        //���˫᭱�V������
        if (attackTran.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        if (attackTran.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        //���˳Q���h
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
        // **������a��m**
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return false;

        // **�p��P���a���Z��**
        float distance = Vector2.Distance(transform.position, player.position);

        // **�p�G�Z���p������d��A��^ `true`**
        return distance <= attackRange;
    }

    public void SwitchState(EenemyState _state)//�������A
    {
        var newState = _state switch//�ھڲ{�����A�����ĤH���A(switch���y�k�}�g�k)
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
