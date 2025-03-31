using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillZheCanAA : MonoBehaviour, ISkillEffect
{
    // 玩家來源
    private Transform origin;
    private Animator playerAnimator;

    // 指派食物攻擊預置物（例如包含攻擊判定的預置物）
    public GameObject foodProjectilePrefab;
    // 預置物移動速度（可依需求調整）
    public float projectileSpeed = 10f;
    // 食物攻擊造成的傷害
    public float damage = 50f;
    // 技能消耗能量
    public float energyCost = 20f;

    // ISkillEffect 實作：設定技能發出者
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 消耗玩家能量：這邊假設玩家使用 CharactorBase 管理能量
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            playerChar.CurrentPower -= energyCost;
            if (playerChar.CurrentPower < 0)
                playerChar.CurrentPower = 0;
            // 若有能量更新事件，可在此呼叫
        }
    }

    // ISkillEffect 實作：設定玩家動畫（如有需要）
    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        // 確保 origin 已設定，否則錯誤提示並銷毀
        if (origin == null)
        {
            Debug.LogError("SkillZheCanAA: origin is not set!");
            Destroy(gameObject);
            return;
        }

        // 讓技能預置物跟隨玩家：直接設為玩家的子物件
        transform.position = origin.position;
        transform.SetParent(origin);

        // 向左生成食物攻擊
        SpawnProjectile(Vector2.left);
        // 向右生成食物攻擊
        SpawnProjectile(Vector2.right);

        // 生成後可銷毀此技能預置物（延遲一個 frame 以確保生成完畢）
        Destroy(gameObject, 0.1f);
    }

    // 根據方向生成食物攻擊預置物
    private void SpawnProjectile(Vector2 direction)
    {
        if (foodProjectilePrefab != null)
        {
            // 生成位置以玩家位置為基準，也可加入偏移量
            Vector3 spawnPosition = origin.position;
            GameObject projectile = Instantiate(foodProjectilePrefab, spawnPosition, Quaternion.identity);

            // 嘗試取得預置物上的自定義攻擊腳本（例如 FoodProjectile）
            FoodProjectile fp = projectile.GetComponent<FoodProjectile>();
            if (fp != null)
            {
                fp.SetDamage(damage);
                fp.SetDirection(direction, projectileSpeed);
            }
            else
            {
                // 如果沒有 FoodProjectile 腳本，則嘗試用 Rigidbody2D 設定速度
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }
            }
        }
        else
        {
            Debug.LogWarning("SkillZheCanAA: foodProjectilePrefab is not assigned!");
        }
    }
}
