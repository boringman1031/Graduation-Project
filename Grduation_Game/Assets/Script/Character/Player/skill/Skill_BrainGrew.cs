using UnityEngine;

public class Skill_BrainGrew : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float attackIncrease = 10f;      // �W�[�����O
    public float healAmount = 50f;          // �^�_��q
    public float energyCost = 30f;          // ���ӯ�q
    public float duration = 10f;             // ����ɶ�

    [Header("���ĻP�S��")]
    public AudioDefination audioPlayer;
    public AudioClip activationSound;
    public GameObject specialEffectPrefab;
    public Vector3 effectSpawnOffset = new Vector3(0, 1f, 0);

    public CharacterEventSO powerChangeEvent;

    private Transform origin;
    private CharactorBase character;
    private BuffManager buffManager;

    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        character = origin.GetComponent<CharactorBase>();
        buffManager = origin.GetComponent<BuffManager>();

        if (character == null || buffManager == null)
        {
            Debug.LogWarning("�ʤ� CharactorBase �� BuffManager");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        ActivateSkill();
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void ActivateSkill()
    {
        // ���񭵮�
        if (audioPlayer && activationSound)
        {
            audioPlayer.audioClip = activationSound;
            audioPlayer.PlayAudioClip();
        }

        // ����q�üs����s
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);

        // ����S��
        if (specialEffectPrefab != null)
        {
            GameObject effect = Instantiate(specialEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity, origin);
            Destroy(effect, 2f);
        }

        // �^�_��q
        character.AddHealth(healAmount);

        // �ϥ� BuffManager �W�[�����O
        buffManager.ApplyAttackBuff(attackIncrease, duration);

        Destroy(gameObject);
    }
}
