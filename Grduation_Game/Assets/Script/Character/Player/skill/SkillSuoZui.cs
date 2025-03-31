using UnityEngine;

public class SkillSuoZui : MonoBehaviour, ISkillEffect
{
    [Header("技能設定")]
    public GameObject vomitProjectilePrefab;  // 嘔吐攻擊預置物
    public AudioClip activationSound;           // 技能啟動時的音效

    // 這邊記錄技能是否應該隨著玩家移動，宿醉這招不需要跟隨玩家，所以不設為子物件
    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 將本技能物件放在玩家位置（但不設定父物件）
        transform.position = origin.position;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // 如果需要設定動畫參數可以處理
    }

    private void Start()
    {
        ActivateSkill();
    }

    public void ActivateSkill()
    {
        if (origin == null)
        {
            Debug.LogWarning("SkillSuoZui: Origin not set");
            return;
        }

        // 取得玩家的腳本（假設使用 CharactorBase）
        CharactorBase player = origin.GetComponent<CharactorBase>();
        if (player != null)
        {
            // 扣除玩家10%的生命值
            float hpDeduct = player.MaxHealth * 0.1f;
            player.CurrentHealth -= hpDeduct;
            if (player.CurrentHealth < 0)
                player.CurrentHealth = 0;
            player.OnHealthChange?.Invoke(player);

            // 消耗能量 20
            player.CurrentPower -= 20;
        }

        // 播放技能啟動音效（在玩家位置播放）
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // 根據玩家朝向決定預置物的移動方向（假設 localScale.x 大於0表示朝右）
        float facing = -Mathf.Sign(origin.localScale.x);
        // 設定旋轉角度：朝右為0度，朝左為180度
        Quaternion projectileRotation = Quaternion.Euler(0, 0, (facing < 0 ? 180f : 0f));

        // 生成嘔吐預置物，位置與玩家一致，旋轉根據朝向決定
        GameObject projectile = Instantiate(vomitProjectilePrefab, origin.position, projectileRotation);
        // 注意：不要將生成物設定成玩家子物件，讓其獨立移動
        projectile.transform.parent = null;

        // 如果預置物腳本有 direction 變數，將 facing 賦值進去
        SuoZuiProjectile szProj = projectile.GetComponent<SuoZuiProjectile>();
        if (szProj != null)
        {
            szProj.direction = facing;
        }

        // 最後銷毀這個技能物件（如果它是一次性使用的）
        Destroy(gameObject);
    }
}
