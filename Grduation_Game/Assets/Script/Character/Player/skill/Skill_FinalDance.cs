using System.Collections;
using UnityEngine;

public class Skill_FinalDance : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float duration = 10f;            // �ޯ����ɶ� 10 ��
    public float damagePerSecond = 100f;      // �C��ˮ` 100
    public float energyCost = 50f;            // ���ӯ�q 50
    public float auraRadius = 5f;             // �ޯ�v�T�d��

    [Header("���ֻP�S�ĳ]�w")]
    public AudioClip backgroundMusic;         // �ޯ�������񪺭I������
    public GameObject finalDanceEffectPrefab;   // ����S�Ĺw�s��
    public Vector3 effectSpawnOffset = new Vector3(0, -1f, 0); // �S�ĥͦ���m����

    private Transform origin;                // Ĳ�o�ޯ઺���a Transform
    private AudioSource musicSource;         // �ΨӼ���I������
    private GameObject effectInstance;       // ����S�Ī����

    public CharacterEventSO powerChangeEvent;

    void costPower(CharactorBase _Charater) //������q
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    public void SetPlayerAnimator(Animator animator)
    {
        // �i�̻ݨD��@
    }

    // ����k�Ѫ��a�ޯ�ʵe�ƥ�I�s
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // ���o���a�ͩR�P��q�޲z�ե� (�Ҧp CharactorBase)
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
        costPower(character);
        //character.CurrentPower -= energyCost;

        // �ͦ�����S�ġA�ó]�����a���l����������H���a
        if (finalDanceEffectPrefab != null)
        {
            effectInstance = Instantiate(finalDanceEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity);
            effectInstance.transform.SetParent(origin);
        }

        // ����I�����֡G�b���a�W�s�W�@�� AudioSource �Ӽ��񭵼֡A�ó]�m���`��
        if (backgroundMusic != null)
        {
            musicSource = origin.gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // �Ұʫ���ˮ`�ĪG
        StartCoroutine(FinalDanceRoutine());
    }

    private IEnumerator FinalDanceRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            DealAuraDamage();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        // �����ޯ�ᰱ��I������
        if (musicSource != null)
        {
            musicSource.Stop();
            Destroy(musicSource);
        }
        // �P������S��
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
        Destroy(gameObject);
    }

    private void DealAuraDamage()
    {
        // ���o���a�P��d�򤺩Ҧ� Collider2D
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, auraRadius);
        foreach (Collider2D hit in hits)
        {
            // �ư����a�����A��㦳 CharactorBase �ե󪺪���y���ˮ`
            CharactorBase enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null && !enemy.CompareTag("Player"))
            {
                enemy.TakeDamage(damagePerSecond, transform);
            }
        }
    }

    // �b Scene �s�边����ܧޯ�v�T�d��A��K�ո�
    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin.position, auraRadius);
        }
    }
}
