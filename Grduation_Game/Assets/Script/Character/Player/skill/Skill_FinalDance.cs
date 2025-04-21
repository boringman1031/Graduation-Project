using System.Collections;
using UnityEngine;

public class Skill_FinalDance : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float duration = 10f;
    public float baseDamagePerSecond = 100f;
    public float energyCost = 50f;
    public float auraRadius = 5f;

    [Header("音效與特效")]
    public AudioDefination audioPlayer;
    public AudioClip backgroundMusic;
    public GameObject finalDanceEffectPrefab;
    public Vector3 effectSpawnOffset = new Vector3(0, -1f, 0);

    public CharacterEventSO powerChangeEvent;

    private Transform origin;
    private CharactorBase character;
    private PlayerStats stats;

    private GameObject effectInstance;

    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        character = origin.GetComponent<CharactorBase>();
        stats = origin.GetComponent<PlayerStats>();

        if (character == null || stats == null)
        {
            Debug.LogWarning("Skill_FinalDance 缺少必要元件");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣能量
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);

        // 播放特效
        if (finalDanceEffectPrefab != null)
        {
            effectInstance = Instantiate(finalDanceEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity, origin);
        }

        // 播音樂
        if (audioPlayer && backgroundMusic)
        {
            audioPlayer.audioClip = backgroundMusic;
            audioPlayer.PlayAudioClip();
        }

        // 啟動持續傷害
        StartCoroutine(FinalDanceRoutine());
    }

    public void SetPlayerAnimator(Animator animator) { }

    private IEnumerator FinalDanceRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            DealAuraDamage();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        if (effectInstance != null)
            Destroy(effectInstance);

        Destroy(gameObject);
    }

    private void DealAuraDamage()
    {
        float finalDPS = baseDamagePerSecond + stats.attack;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, auraRadius);
        foreach (Collider2D hit in hits)
        {
            CharactorBase target = hit.GetComponent<CharactorBase>();
            if (target != null && !target.CompareTag("Player"))
            {
                target.TakeDamage(finalDPS, transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(origin.position, auraRadius);
        }
    }
}
