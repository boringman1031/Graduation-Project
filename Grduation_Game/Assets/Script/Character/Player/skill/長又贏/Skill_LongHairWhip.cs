using UnityEngine;

public class Skill_LongHairWhip : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float energyCost = 50f;
    public float damage = 100f;
    public float radius = 2f;
    public float buffDuration = 10f;
    public float speedBuff = 30f;
    public float healthBuff = 10f;

    [Header("�S��")]
    public GameObject effectPrefab;
    public Vector3 effectOffset;
    public AudioClip whipSound;
    public AudioDefination audioPlayer;

    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        transform.position = origin.position;

        var character = origin.GetComponent<CharactorBase>();
        var stats = origin.GetComponent<PlayerStats>();
        var buffManager = origin.GetComponent<BuffManager>();

        if (character == null || stats == null || buffManager == null)
        {
            Debug.LogWarning("�ʤ֥��n����");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // ����q
        character.AddPower(-energyCost);

        // ����S��
        if (effectPrefab != null)
        {
            GameObject fx = Instantiate(effectPrefab, origin.position + effectOffset, Quaternion.identity);
            fx.transform.SetParent(origin); // �j�w���a�i��
            Destroy(fx, 2f);
        }

        // ������
        if (audioPlayer && whipSound)
        {
            audioPlayer.audioClip = whipSound;
            audioPlayer.PlayAudioClip();
        }

        // AoE �ˮ`
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player")) continue;

            var enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform);

                // �����޿�]�i��^
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockback = (enemy.transform.position - origin.position).normalized * 5f;
                    rb.AddForce(knockback, ForceMode2D.Impulse);
                }
            }
        }

        // Buff�G�t�� + ��q
        buffManager.ApplySpeedBuff(speedBuff, buffDuration);
        buffManager.ApplyMaxHealthBuff(healthBuff, buffDuration);

        Destroy(gameObject); // �ޯ���槹���۾P
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (origin != null)
            Gizmos.DrawWireSphere(origin.position, radius);
    }
}
