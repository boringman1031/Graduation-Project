using UnityEngine;

public class SuoZuiProjectile : MonoBehaviour
{
    public float speed;           // 移動速度
    public float damage;         // 傷害值
    public float lifeTime;        // 預置物存在時間

    // 根據玩家朝向，1 表示向右，-1 表示向左
    public float direction = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 設定一段時間後自動銷毀
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 沿著預置物當前的右邊方向移動，但乘上 direction，以達到依據朝向決定移動方向
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 忽略玩家（假設玩家的 Tag 為 "Player"）
        if (collision.CompareTag("Player"))
            return;

        // 嘗試取得敵人的腳本（假設敵人使用 CharactorBase）
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (enemy != null)
        {
            // 造成傷害：這邊可以根據你的 Attack 邏輯進行調整

            enemy.TakeDamage(damage, transform);

            // 可在此生成擊中特效、播放命中音效
            // (例如：Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);)

            // 命中後銷毀投射物
            Destroy(gameObject);
        }
    }
}
