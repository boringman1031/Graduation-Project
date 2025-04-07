using UnityEngine;

public class Skill_BrainGrew : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float attackIncrease = 10f;   // ���ɧ�����
    public float healAmount = 50f;       // �^�_��q
    public float energyCost = 30f;       // ���ӯ�q

    [Header("���ĳ]�w")]
    public AudioClip activationSound;    // �ޯ�E���ɼ��񪺭���

    [Header("�S�ĳ]�w")]
    public GameObject specialEffectPrefab; // �ޯ�S�� prefab
    public Vector3 effectSpawnOffset = new Vector3(0, 1f, 0); // �S�ĥͦ��������A�w�]���W 1 ���

    private Transform origin; // Ĳ�o�ޯ઺���a Transform

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

        // ���o���a���ͩR�P��q�޲z�ե�]CharactorBase�^
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

        // ����E������
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // �ͦ��S�ġA�S�Ħ�m�[�W����
        if (specialEffectPrefab != null)
        {
            GameObject effect = Instantiate(specialEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity);
            effect.transform.SetParent(origin);
            Destroy(effect, 2f);
        }

        // ������q
        costPower(character);

        // �^�_��q (�ϥ� CharactorBase ���� AddHealth ��k)
        character.AddHealth(healAmount);

        // ���ɧ����G���]���a�� PlayerStats �ե�޲z�����ƭ�
        PlayerStats stats = origin.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.attack += attackIncrease;
        }
        else
        {
            Debug.LogWarning("PlayerStats �ե󥼧��A�L�k���ɧ���");
        }

        // �ޯ�ĪG���槹����A�ߧY�P���ޯફ��
        Destroy(gameObject);
    }
}
