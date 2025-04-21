using System.Collections;
using UnityEngine;

public class SkillHoldHand : MonoBehaviour, ISkillEffect
{
    [Header("技能數值")]
    public float baseDamage = 50f;
    public float energyCost = 10f;

    [Header("效果設定")]
    public AudioDefination audioPlayer;
    public AudioClip skillSound;
    public ParticleSystem skillEffect;
    public Animator overrideAnimator; // 可指定用其他動畫控制器

    [Header("生成設定")]
    public Vector3 spawnOffset;

    public CharacterEventSO powerChangeEvent;

    private Transform origin;
    private CharactorBase character;
    private PlayerStats stats;
    private float finalDamage;
    private int faceDir;

    private bool isActivated = false;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        character = origin.GetComponent<CharactorBase>();
        stats = origin.GetComponent<PlayerStats>();

        if (character == null || stats == null)
        {
            Debug.LogWarning("找不到角色組件，技能取消");
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

        isActivated = true;

        // 朝向與生成位置處理
        faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);
        transform.localScale = new Vector3(faceDir, 1, 1);

        // 計算總傷害
        finalDamage = baseDamage + stats.attack;

        // 播放音效與特效
        if (audioPlayer && skillSound)
        {
            audioPlayer.audioClip = skillSound;
            audioPlayer.PlayAudioClip();
        }
        if (skillEffect) skillEffect.Play();

        // 播動畫（可用 overrideAnimator 覆蓋）
        if (overrideAnimator != null)
            overrideAnimator.SetTrigger("Skill1");
        else
        {
            Animator anim = GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Skill1");
        }

        // 啟動碰撞時間
        StartCoroutine(ActivateAttackCollider());
    }

    public void SetPlayerAnimator(Animator animator)
    {
        overrideAnimator = animator;
    }

    IEnumerator ActivateAttackCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        yield return new WaitForSeconds(0.3f); // 攻擊持續時間
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated) return;
        if (collision.CompareTag("Player")) return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            target.TakeDamage(finalDamage, transform);
        }
    }
}
