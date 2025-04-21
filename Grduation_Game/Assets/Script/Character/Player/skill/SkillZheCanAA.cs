using System.Collections;
using UnityEngine;

public class SkillZheCanAA : MonoBehaviour, ISkillEffect
{
    private Transform origin;                  // 技能施放來源（玩家）
    private Animator playerAnimator;

    [Header("技能設定")]
    public GameObject foodProjectilePrefab;    // 食物攻擊預置物
    public float projectileSpeed = 10f;        // 食物飛行速度
    public float baseDamage = 50f;             // 技能基礎傷害
    public float energyCost = 20f;             // 能量消耗

    public CharacterEventSO powerChangeEvent;  // 扣能量後觸發 UI 更新

    private float finalDamage = 0f;

    public void SetOrigin(Transform originT)
    {
        origin = originT;
        transform.position = origin.position;

        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        PlayerStats playerStats = origin.GetComponent<PlayerStats>();

        if (playerChar == null || playerStats == null)
        {
            Debug.LogWarning("缺少必要組件 CharactorBase 或 PlayerStats，技能取消");
            Destroy(gameObject);
            return;
        }

        // 能量不足則取消技能
        if (playerChar.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣能量並廣播更新
        playerChar.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(playerChar);

        // 傷害 = 技能基礎傷害 + 玩家攻擊力
        finalDamage = baseDamage + playerStats.attack;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        if (origin == null || foodProjectilePrefab == null)
        {
            Debug.LogError("SkillZheCanAA: 必要屬性未設定");
            Destroy(gameObject);
            return;
        }

        // 往左右生成食物攻擊
        SpawnProjectile(Vector2.left);
        SpawnProjectile(Vector2.right);

        // 效果完成即銷毀（稍微延遲確保子物件建立）
        Destroy(gameObject, 0.1f);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(foodProjectilePrefab, origin.position, Quaternion.identity);

        // 嘗試設定攻擊腳本
        FoodProjectile fp = projectile.GetComponent<FoodProjectile>();
        if (fp != null)
        {
            fp.SetDamage(finalDamage);
            fp.SetDirection(direction, projectileSpeed);
        }
        else
        {
            // 若沒攻擊腳本就嘗試用 Rigidbody2D 推動
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}
