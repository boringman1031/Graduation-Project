using UnityEngine;

public class Skill_BrainGrew : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float attackIncrease = 10f;      // 增加攻擊力
    public float healAmount = 50f;          // 回復血量
    public float energyCost = 30f;          // 消耗能量
    public float duration = 10f;             // 持續時間

    [Header("音效與特效")]
    public AudioDefination audioPlayer;
    public AudioClip activationSound;
    public GameObject specialEffectPrefab;
    public Vector3 effectSpawnOffset = new Vector3(0, 1f, 0);

    public CharacterEventSO powerChangeEvent;

    private Transform origin;
    private CharactorBase character;
    private BuffManager buffManager;

    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        character = origin.GetComponent<CharactorBase>();
        buffManager = origin.GetComponent<BuffManager>();

        if (character == null || buffManager == null)
        {
            Debug.LogWarning("缺少 CharactorBase 或 BuffManager");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        ActivateSkill();
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void ActivateSkill()
    {
        // 播放音效
        if (audioPlayer && activationSound)
        {
            audioPlayer.audioClip = activationSound;
            audioPlayer.PlayAudioClip();
        }

        // 扣能量並廣播更新
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);

        // 播放特效
        if (specialEffectPrefab != null)
        {
            GameObject effect = Instantiate(specialEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity, origin);
            Destroy(effect, 2f);
        }

        // 回復血量
        character.AddHealth(healAmount);

        // 使用 BuffManager 增加攻擊力
        buffManager.ApplyAttackBuff(attackIncrease, duration);

        Destroy(gameObject);
    }
}
