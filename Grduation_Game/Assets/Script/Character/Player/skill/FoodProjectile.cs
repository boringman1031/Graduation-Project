using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProjectile : MonoBehaviour
{
    // 五個不同食物圖片，請在 Inspector 指派
    public Sprite[] foodSprites;

    // 命中特效預置物（撞擊時產生）
    public GameObject hitEffectPrefab;
    // 命中音效
    public AudioClip hitSound;

    // 投擲時的音效
    public AudioClip launchSound;

    // 攻擊傷害
    private float damage;
    // 移動方向與速度
    private Vector2 direction;
    private float speed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // 每秒旋轉角度
    public float rotationSpeed = 180f;

    // 設定攻擊傷害
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    // 設定移動方向與速度
    public void SetDirection(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = this.direction * this.speed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 隨機挑選一個食物圖片
        if (foodSprites != null && foodSprites.Length > 0 && sr != null)
        {
            int index = Random.Range(0, foodSprites.Length);
            sr.sprite = foodSprites[index];
        }
    }

    private void Start()
    {
        // 播放投擲時音效
        if (launchSound != null)
        {
            AudioSource.PlayClipAtPoint(launchSound, transform.position);
        }

        // 自動銷毀預置物 (避免長時間存在)
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // 讓食物持續旋轉
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 排除玩家（假設玩家 Tag 為 "Player"）
        if (collision.CompareTag("Player"))
            return;

        // 檢查碰撞對象是否有 CharactorBase 組件
        CharactorBase target = collision.GetComponent<CharactorBase>();

        if (target != null)
        {
            // 播放命中音效
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            // 生成命中特效
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, target.transform.position, Quaternion.identity);
            }

            // 扣除傷害
            float newHealth = target.CurrentHealth - damage;
            if (newHealth > 0)
            {
                target.CurrentHealth = newHealth;
                target.OnTakeDamage?.Invoke(transform);
            }
            else
            {
                target.CurrentHealth = 0;
                target.OnDead?.Invoke();
            }
            target.OnHealthChange?.Invoke(target);

            // 撞擊後銷毀預置物
            Destroy(gameObject);
        }
    }
}
