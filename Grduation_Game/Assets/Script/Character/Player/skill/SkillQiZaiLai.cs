using System.Collections;
using UnityEngine;

public class SkillQiZaiLai : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�]�w")]
    public AudioDefination audioPlayer;// ���ļ���
    public AudioClip activationSound;          // �ޯ�Ұʮɭ���
    public float buffDuration = 15f;              // �W�q����ɶ�
    public float speedBuffAmount = 6f;          // �W�[���t�׭�
    public float attackBuffAmount = 66f;         // �W�[��������
    public float energyCost = 30f;               // ���ӯ�q
    public float hpDeductPercentage = 0.1f;      // �������a10%�ͩR

    private Transform origin;
    public CharacterEventSO powerChangeEvent;
    void costPower(CharactorBase _Charater) //������q
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // �N���ޯફ��w��쪱�a��m�A�����]���l����
        transform.position = origin.position;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // ���ޯण�ݨϥΰʵe�Ѽ�
    }

    private void Start()
    {
        ActivateSkill();
    }

    public void ActivateSkill()
    {
        if (origin == null)
        {
            Debug.LogWarning("SkillQiZaiLai: Origin not set");
            return;
        }

        // ���o���a�� CharactorBase �ե�]����P��q�����^
        CharactorBase character = origin.GetComponent<CharactorBase>();

        // �ˬd��q�O�_����
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }
        // ������q
        costPower(character);

        if (character != null)
        {
            // ����10%��q
            float hpDeduct = character.MaxHealth * hpDeductPercentage;
            character.CurrentHealth -= hpDeduct;
            if (character.CurrentHealth < 0)
                character.CurrentHealth = 0;
            character.OnHealthChange?.Invoke(character);
        }
        else
        {
            Debug.LogWarning("SkillQiZaiLai: CharactorBase not found on origin");
        }

        // ����Ұʭ���
        if (audioPlayer != null && activationSound != null)
        {
            audioPlayer.audioClip = activationSound;
            audioPlayer.PlayAudioClip();
        }

        // ���]���a�W���@�� PlayerStats �ե�A�x�s�t�׻P������
        PlayerStats stats = origin.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogWarning("SkillQiZaiLai: PlayerStats not found on origin");
            return;
        }

        // �O����l�ƭ�
        float originalSpeed = stats.speed;
        float originalAttack = stats.attack;

        // �W�[ buff
        stats.speed += speedBuffAmount;
        stats.attack += attackBuffAmount;

        // �Ұ� coroutine �b buff ����ɶ��������٭�ƭ�
        StartCoroutine(RevertBuff(stats, originalSpeed, originalAttack, buffDuration));

        // �P�����ޯફ��]�@���ʧޯ�^
        Destroy(gameObject);
    }

    private IEnumerator RevertBuff(PlayerStats stats, float originalSpeed, float originalAttack, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (stats != null)
        {
            stats.speed = originalSpeed;
            stats.attack = originalAttack;
        }
    }
}
