using System.Collections.Generic;
using UnityEngine;

public class NegativeEnergyEffect : MonoBehaviour
{
    public float damagePerSecond = 10f;  // 每秒傷害（扣除一次時就扣這個數值）
    public float lifeTime = 3f;          // 預置物存在時間

    public GameObject hitEffectPrefab;   // 擊中特效預置物
    public AudioClip hitEffectSound;     // 擊中音效

    // 用來記錄每個碰撞到的敵人上一次扣傷的時間
    private Dictionary<Collider2D, float> damageTimers = new Dictionary<Collider2D, float>();

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 排除玩家（假設玩家的 Tag 為 "Player"）
        if (collision.CompareTag("Player"))
            return;

        // 檢查碰撞對象是否有敵人組件（例如 CharactorBase）
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (enemy != null)
        {
            // 如果沒有紀錄，初始化時間
            if (!damageTimers.ContainsKey(collision))
            {
                damageTimers[collision] = Time.time;
            }

            // 若超過 1 秒，扣一次傷害
            if (Time.time - damageTimers[collision] >= 1f)
            {
                Debug.Log("受到傷害 " + damagePerSecond);
                Attack tempAttack = new Attack();
                tempAttack.Damage = damagePerSecond;
                enemy.TakeDamage(tempAttack);
                

                // 生成擊中特效
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
                }
                // 生成擊中音效
                if (hitEffectSound != null)
                {
                    AudioSource.PlayClipAtPoint(hitEffectSound, transform.position);
                }

                // 更新扣傷時間
                damageTimers[collision] = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 當敵人離開後，清除紀錄
        if (damageTimers.ContainsKey(collision))
        {
            damageTimers.Remove(collision);
        }
    }
}
