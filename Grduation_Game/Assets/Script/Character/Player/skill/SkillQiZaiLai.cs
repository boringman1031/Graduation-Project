using System.Collections;
using UnityEngine;

public class SkillQiZaiLai : MonoBehaviour, ISkillEffect
{
    [Header("技能設定")]
    public AudioDefination audioPlayer;               // 音效播放器
    public AudioClip activationSound;                 // 技能啟動時音效
    public float buffDuration = 15f;                  // 增益持續時間
    public float speedBuffAmount = 6f;                // 增加的速度值
    public float attackBuffAmount = 66f;              // 增加的攻擊值
    public float energyCost = 30f;                    // 消耗能量
    public float hpDeductPercentage = 0.1f;           // 扣除玩家10%生命

    private Transform origin;
    public CharacterEventSO powerChangeEvent;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 將本技能物件定位到玩家位置，但不設為子物件
        transform.position = origin.position;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // 此技能不需使用動畫參數
    }

    private void Start()
    {
        ActivateSkill();
    }

    public void ActivateSkill()
    {
        if (origin == null)
        {
            Debug.LogWarning("SkillQiZaiLai: Origin not set");
            Destroy(gameObject);
            return;
        }

        // 取得玩家的 CharactorBase 組件（扣血與能量相關）
        CharactorBase character = origin.GetComponent<CharactorBase>();
        // 假設玩家上有一個 PlayerStats 組件，儲存速度與攻擊值
        PlayerStats stats = origin.GetComponent<PlayerStats>();
        // 使用 BuffManager 管理 buff 邏輯
        BuffManager buffManager = origin.GetComponent<BuffManager>();

        if (character == null || stats == null || buffManager == null)
        {
            Debug.LogWarning("SkillQiZaiLai: 缺少必要組件");
            Destroy(gameObject);
            return;
        }

        // 檢查能量是否足夠
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣除能量
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);

        if (character != null)
        {
            // 扣除10%血量
            float hpDeduct = character.MaxHealth * hpDeductPercentage;
            character.CurrentHealth -= hpDeduct;
            if (character.CurrentHealth < 0)
                character.CurrentHealth = 0;
            character.OnHealthChange?.Invoke(character);
        }
        else
        {
            Debug.LogWarning("SkillQiZaiLai: CharactorBase not found on origin");
        }

        // 播放啟動音效
        if (audioPlayer != null && activationSound != null)
        {
            audioPlayer.audioClip = activationSound;
            audioPlayer.PlayAudioClip();
        }

        // 增加 buff（速度 + 攻擊）
        buffManager.ApplySpeedBuff(speedBuffAmount, buffDuration);
        buffManager.ApplyAttackBuff(attackBuffAmount, buffDuration);

        // 銷毀此技能物件（一次性技能）
        Destroy(gameObject);
    }
}
