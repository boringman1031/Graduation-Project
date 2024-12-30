using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private int EnemyAttack = 10; // ����
    [SerializeField]
    private int EnemyHealth = 100; // ��q
    [SerializeField]
    private float EnemySpeed = 5.0f; // ���ʳt��
    [SerializeField]
    private float MoveThreshold = 5.0f; // �P���a���Z���H��

    private PlayerBase player;
    private Transform playerTransform;

    public delegate void EnemyGetHit(int damage);
    public event EnemyGetHit OnEnemyHit;

    public delegate void EnemyMove();
    public event EnemyMove OnEnemyMove;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerBase>();
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > MoveThreshold)
            {
                Enemy_Move();
            }
        }
    }

    public void Enemy_Move()
    {
        OnEnemyMove?.Invoke(); // Ĳ�o�ƥ�

        // �����޿�G�¦V���a��m����
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // �p���V
            transform.position += direction * EnemySpeed * Time.deltaTime; // ����
        }
    }

    public void Enemy_Attack()
    {
        // �o�̩�������{���X
    }

    public void Enemy_Hit(int damage)
    {
        EnemyHealth -= damage;
        OnEnemyHit?.Invoke(damage); // Ĳ�o�ƥ�
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject); // �ĤH���`
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ˬd�O�_�I�쪱�a
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Player_Hit(EnemyAttack);
                Debug.Log($"Enemy attacked Player for {EnemyAttack} damage.");
            }
        }
    }
}
