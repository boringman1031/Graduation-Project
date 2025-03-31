using System.Collections;
using UnityEngine;

public class SkillEmo : MonoBehaviour, ISkillEffect
{
    private Transform origin;

    [Header("�ޯ�Ѽ�")]
    public float duration = 10f;           // �ޯ����ɶ�
    public float spawnInterval = 0.5f;     // �t���ĪG�ͦ����j
    public float damagePerSecond = 10f;    // �t���ĪG�ˮ`�]�C��^
    public float healthDeductionPercent = 0.1f; // ������q�ʤ���]10%�^

    [Header("�ͦ���m�Ѽ�")]
    public float spawnRangeX;         // �����ͦ��d��]���k�^
    public float spawnOffsetY;      // ���������]�����a��m�^

    [Header("�t����q�ĪG�w�m��")]
    public GameObject negativeEffectPrefab; // �t����q�ĪG prefab
    public AudioClip skillSound;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // �������a 10% ��q�]�H MaxHealth ����ǡ^
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            float deduction = playerChar.MaxHealth * healthDeductionPercent;
            playerChar.CurrentHealth -= deduction;
            if (playerChar.CurrentHealth < 0)
                playerChar.CurrentHealth = 0;
            playerChar.OnHealthChange?.Invoke(playerChar);
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // ���ݨϥ�
    }

    private void Start()
    {
        if (origin == null)
        {
            Debug.LogError("SkillEmo: origin is not set!");
            Destroy(gameObject);
            return;
        }
        // �N�ޯફ��]�����a���l����A���k�s localPosition
        transform.SetParent(origin);
        transform.localPosition = Vector3.zero;

        if (skillSound != null)
        {
            AudioSource.PlayClipAtPoint(skillSound, transform.position);
        }

        // �}�l�ͦ��t����q�ĪG
        StartCoroutine(SpawnNegativeEffects());
        // �����ޯ�ĪG��P���ۨ�
        StartCoroutine(EndSkillAfterDuration());
    }

    private IEnumerator SpawnNegativeEffects()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            SpawnNegativeEffect();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }
    }

    private void SpawnNegativeEffect()
    {
        if (negativeEffectPrefab != null)
        {
            // �H�����ͥ��k����
            float offsetX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = origin.position + new Vector3(offsetX, spawnOffsetY, 0);
            // �ͦ����ĪG�����]�����a���l����A�o�˷|�H���a����
            Instantiate(negativeEffectPrefab, spawnPos, Quaternion.identity);
        }
    }

    private IEnumerator EndSkillAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
