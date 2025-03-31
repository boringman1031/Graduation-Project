using System.Collections;
using UnityEngine;

public class Skill_SlackOff : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float duration = 10f;         // ����ɶ� 10 ��
    public float damagePerSecond = 10f;    // �C��y�� 10 �ˮ`
    public float energyCost = 20f;         // ���ӯ�q 20
    [Header("�^��]�w")]
    public float healPercentage = 0.1f;    // �^�_ 10% ��q

    [Header("�d��]�w")]
    public float auraRadius = 5f;          // �ޯ�ĪG�d��

    [Header("���ĳ]�w")]
    public AudioClip activationSound;      // �ޯ�E������
    public AudioClip auraDamageSound;      // �C���ˮ`���񪺭��� (�i��)

    [Header("�S�ĳ]�w")]
    public GameObject auraEffectPrefab;    // ������H���a���S��
    // �A�i�H�w�q���k�ⰼ�������q�A�Ҧp�������� (-1,1,0)�A�k������ (1,1,0)
    public Vector3 leftEffectOffset = new Vector3(-1f, 1f, 0);
    public Vector3 rightEffectOffset = new Vector3(1f, 1f, 0);

    private Transform origin;              // Ĳ�o�ޯ઺���a Transform

    public void SetPlayerAnimator(Animator animator)
    {
        // �i�̻ݨD��@
    }

    // ����k�Ѫ��a�ޯ�ʵe�ƥ�I�s
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // ����E������
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // ���o���a���ͩR�P��q�޲z�ե� (�Ҧp CharactorBase)
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("����� CharactorBase �ե�A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // �ˬd��q�O�_����
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }
        // ������q
        character.CurrentPower -= energyCost;

        // �^�_��q�G�^�_ 10% ���̤j��q
        float healAmount = character.MaxHealth * healPercentage;
        character.AddHealth(healAmount);

        // �ͦ��S�ġA�ó]�����a���l����A�����@�����H���a
        if (auraEffectPrefab != null)
        {
            // �ͦ������S�ġAZ�b����90��
            GameObject leftEffect = Instantiate(auraEffectPrefab, origin.position + leftEffectOffset, Quaternion.Euler(0, 0, 90));
            leftEffect.transform.SetParent(origin);

            // �ͦ��k���S�ġAZ�b����-90��
            GameObject rightEffect = Instantiate(auraEffectPrefab, origin.position + rightEffectOffset, Quaternion.Euler(0, 0, -90));
            rightEffect.transform.SetParent(origin);
        }

        // �}�l�ޯ����ĪG�A��P��ĤH�y���ˮ`
        StartCoroutine(AuraEffectRoutine());
    }

    private IEnumerator AuraEffectRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            DealAuraDamage();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        // 10 ���P���ޯફ��
        Destroy(gameObject);
    }

    private void DealAuraDamage()
    {
        // ���o���a�P��d�򤺩Ҧ� Collider2D
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, auraRadius);
        foreach (Collider2D hit in hits)
        {
            // �ˬd��H�O�_���ĤH (���]�ĤH�� CharactorBase �B tag ���O "Player")
            CharactorBase enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null && !enemy.CompareTag("Player"))
            {
                // ����ˮ`���� (�i��)
                if (auraDamageSound != null)
                {
                    AudioSource.PlayClipAtPoint(auraDamageSound, hit.transform.position);
                }
                // �ǻ��ˮ`�ƭ�
                enemy.TakeDamage(damagePerSecond, transform);
            }
        }
    }

    // �b Scene �s�边����ܽd�� (��K�ո�)
    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(origin.position, auraRadius);
        }
    }
}
