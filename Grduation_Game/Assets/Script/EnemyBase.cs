using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private int EnemyAttack = 10; // 攻擊
    [SerializeField]
    private int EnemyHealth = 100; // 血量
    [SerializeField]
    private float EnemySpeed = 5.0f; // 移動速度
    [SerializeField]
    private float MoveThreshold = 5.0f; // 與玩家的距離閾值

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
        OnEnemyMove?.Invoke(); // 觸發事件

        // 移動邏輯：朝向玩家位置移動
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // 計算方向
            transform.position += direction * EnemySpeed * Time.deltaTime; // 移動
        }
    }

    public void Enemy_Attack()
    {
        // 這裡放攻擊的程式碼
    }

    public void Enemy_Hit(int damage)
    {
        EnemyHealth -= damage;
        OnEnemyHit?.Invoke(damage); // 觸發事件
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject); // 敵人死亡
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查是否碰到玩家
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
