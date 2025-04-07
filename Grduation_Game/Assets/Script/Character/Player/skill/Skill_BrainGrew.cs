using UnityEngine;

public class Skill_BrainGrew : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float attackIncrease = 10f;   // 提升攻擊值
    public float healAmount = 50f;       // 回復血量
    public float energyCost = 30f;       // 消耗能量

    [Header("音效設定")]
    public AudioClip activationSound;    // 技能激活時播放的音效

    [Header("特效設定")]
    public GameObject specialEffectPrefab; // 技能特效 prefab
    public Vector3 effectSpawnOffset = new Vector3(0, 1f, 0); // 特效生成的偏移，預設往上 1 單位

    private Transform origin; // 觸發技能的玩家 Transform

    public CharacterEventSO powerChangeEvent;

    void costPower(CharactorBase _Charater) //扣除能量
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    public void SetPlayerAnimator(Animator animator)
    {
        // 可依需求實作
    }

    // 此方法由玩家技能動畫事件呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // 取得玩家的生命與能量管理組件（CharactorBase）
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("未找到 CharactorBase 組件，無法施放技能");
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

        // 播放激活音效
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // 生成特效，特效位置加上偏移
        if (specialEffectPrefab != null)
        {
            GameObject effect = Instantiate(specialEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity);
            effect.transform.SetParent(origin);
            Destroy(effect, 2f);
        }

        // 扣除能量
        costPower(character);

        // 回復血量 (使用 CharactorBase 中的 AddHealth 方法)
        character.AddHealth(healAmount);

        // 提升攻擊：假設玩家有 PlayerStats 組件管理攻擊數值
        PlayerStats stats = origin.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.attack += attackIncrease;
        }
        else
        {
            Debug.LogWarning("PlayerStats 組件未找到，無法提升攻擊");
        }

        // 技能效果執行完畢後，立即銷毀技能物件
        Destroy(gameObject);
    }
}
