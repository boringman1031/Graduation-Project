using UnityEngine;

public class MinionFollower : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float detectRange = 15f;
    public float attackRange = 1.5f;
    public float attackInterval = 1f;
    public float duration = 10f;
    public float baseDamage = 30f;

    private float lastAttackTime;
    private Transform target;

    private Transform origin; // ✅ 玩家來源
    private PlayerStats playerStats;

    private Animator anim;

    public void SetOrigin(Transform _origin)
    {
        origin = _origin;
        if (origin != null)
        {
            playerStats = origin.GetComponent<PlayerStats>();
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        Invoke(nameof(SelfDestroy), duration);
    }

    private void Update()
    {
        if (target == null || Vector2.Distance(transform.position, target.position) > detectRange)
            FindClosestEnemy();

        if (target != null)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist > attackRange)
            {
                anim?.SetBool("IsAttacking", false);
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;

                // 轉向
                float dirX = target.position.x - transform.position.x;
                if (dirX != 0)
                    transform.localScale = new Vector3(-Mathf.Sign(dirX) *1.6f, 1.6f, 1);
            }
            else
            {
                anim?.SetBool("IsAttacking", true);

                if (Time.time - lastAttackTime > attackInterval)
                {
                    lastAttackTime = Time.time;

                    

                    float finalDamage = baseDamage;
                    if (playerStats != null)
                        finalDamage += playerStats.attack;

                    var enemy = target.GetComponent<CharactorBase>();
                    if (enemy != null)
                        enemy.TakeDamage(finalDamage, transform);
                    
                }
            }
        }
    }

    void FindClosestEnemy()
    {
        CharactorBase[] enemies = GameObject.FindObjectsOfType<CharactorBase>();
        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in enemies)
        {
            if (!enemy.CompareTag("Enemy")) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy.transform;
            }
        }

        target = closest;
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
