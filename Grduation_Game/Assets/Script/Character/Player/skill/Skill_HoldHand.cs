using System.Collections;
using UnityEngine;

public class SkillHoldHand : MonoBehaviour, ISkillEffect
{
    [Header("技能數值")]
    public float damage = 50f;
    public float energyCost = 10f;

    [Header("效果設定")]
    public AudioClip skillSound;
    public ParticleSystem skillEffect;
    public Animator animator;

    [Header("其他設定")]
    public Vector3 spawnOffset; // 例如 (1, 0, 0)
    public Transform origin;
    public Animator playerAnimator;

    public CharacterEventSO powerChangeEvent;

    private bool isActivated = false;

    void costPower(CharactorBase character)
    {
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null || character.CurrentPower < energyCost)
        {
            Debug.LogWarning("技能施放失敗：能量不足或找不到角色元件");
            Destroy(gameObject);
            return;
        }

        costPower(character);
        isActivated = true;

        int faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);
        transform.localScale = new Vector3(faceDir, 1, 1); // 朝向

        if (skillSound != null) AudioSource.PlayClipAtPoint(skillSound, transform.position);
        if (skillEffect != null) skillEffect.Play();
        if (playerAnimator != null)
            playerAnimator.SetTrigger("Skill1");
        else if (animator != null)
            animator.SetTrigger("Skill1");

        // ✅ 開始短時間內的攻擊碰撞
        StartCoroutine(ActivateAttackCollider());
    }

    public void SetPlayerAnimator(Animator animator)
    {
        this.playerAnimator = animator;
    }

    IEnumerator ActivateAttackCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }

        yield return new WaitForSeconds(0.3f); // 可以命中敵人的時間
        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 0.5f); // 延遲銷毀（讓特效跑完）
    }
}
