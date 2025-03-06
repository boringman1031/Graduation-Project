using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOrLoseAI : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10; // �����O
    public float lifetime = 5f; // �s�b�ɶ�
    public float attackRange = 0.5f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private SpriteRenderer spriteRenderer;

    private Transform target;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        target = FindClosestEnemy();
        Destroy(gameObject, lifetime); // �L������
        if (target != null)
        {
            // �P�_�ؼЦb�����٬O�k��
            if (target.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1); // ½�� X �b
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > attackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    void Attack()
    {
        animator.SetBool("attack", true);
        //target.GetComponent<Enemy>().TakeDamage(damage);
        print("attack");
    }
    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }
}