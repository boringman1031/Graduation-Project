using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    PhysicsCheck physicsCheck;

    [Header("��¦�ƭ�")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;//���V��V
    public Transform attacker;//������
    public float HitForce;//�������h�O��

    [Header("���A")]
    public bool isHit;
    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
    }
   
    public void Update()
    {
        faceDir =new Vector3(- transform.position.x, 0, 0);
        if(physicsCheck.touchLeftWall || physicsCheck.touchRightWall)
        {
         transform.localScale = new Vector3(faceDir.x,1,1);
        }
    }

    private void FixedUpdate()
    {
        if (!isHit & !isDead) 
            Move();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }

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
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}
