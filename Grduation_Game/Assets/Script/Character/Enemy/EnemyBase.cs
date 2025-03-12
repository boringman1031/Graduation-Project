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

    [Header("�ƥ�s��")]
    public EnemyEventSO OnEnemyDied;

    [Header("��¦�ƭ�")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;//���V��V
    public Transform attacker;//������
    public float HitForce;//�������h�O��

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
    [HideInInspector] public float waitTimeCounter;
    [HideInInspector] public bool isWait; 
    public float lostTime;
    [HideInInspector] public float lostTimeCounter;

    private BaseState currentState;//��e���A
    protected BaseState patrolState;//���ު��A
    protected BaseState chaseState;//�l�����A

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
        var newState = _state switch//�ھڲ{�����A�����ĤH���A(switch���y�k�}�g�k)
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    #region �ƥ�����k
    public void OnTakeDamage(Transform attackTran)
    {
        attacker = attackTran;
        //���˫᭱�V������
        if (attackTran.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTran.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
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
