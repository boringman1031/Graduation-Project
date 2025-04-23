using System.Collections;
using UnityEngine;

public class Skill_YoBattle : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float baseDamage = 100f;
    public float energyCost = 50f;
    public float attackRadius = 2f;
    public float interval = 0.2f;
    public Vector2[] slashOffsets = new Vector2[] {
        new Vector2(-1.5f, 0f),
        new Vector2(1.5f, 0f),
        new Vector2(-2.5f, 0f),
        new Vector2(2.5f, 0f),
    };

    [Header("特效與音效")]
    public GameObject slashEffectPrefab;
    public AudioClip battleSound;
    public AudioDefination audioPlayer;

    private Transform origin;
    private PlayerStats stats;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        transform.position = origin.position;

        var character = origin.GetComponent<CharactorBase>();
        stats = origin.GetComponent<PlayerStats>();

        if (character == null || stats == null)
        {
            Debug.LogWarning("缺少角色組件");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足");
            Destroy(gameObject);
            return;
        }

        character.AddPower(-energyCost);

        if (audioPlayer && battleSound)
        {
            audioPlayer.audioClip = battleSound;
            audioPlayer.PlayAudioClip();
        }

        StartCoroutine(SlashRoutine());

        Destroy(gameObject, interval * 4 + 0.5f);
    }

    IEnumerator SlashRoutine()
    {
        float finalDamage = baseDamage + stats.attack;

        for (int i = 0; i < slashOffsets.Length; i++)
        {
            Vector3 offset = (Vector3)slashOffsets[i];
            Vector3 pos = origin.position + offset;

            if (slashEffectPrefab)
            {
                GameObject fx = Instantiate(slashEffectPrefab, pos, Quaternion.identity);
                Destroy(fx, 1f);
            }

            // 範圍傷害
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, attackRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player")) continue;

                CharactorBase enemy = hit.GetComponent<CharactorBase>();
                if (enemy != null)
                    enemy.TakeDamage(finalDamage, transform);
            }

            yield return new WaitForSeconds(interval);
        }
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.red;
        foreach (Vector2 offset in slashOffsets)
        {
            Gizmos.DrawWireSphere(origin.position + (Vector3)offset, attackRadius);
        }
    }
}
