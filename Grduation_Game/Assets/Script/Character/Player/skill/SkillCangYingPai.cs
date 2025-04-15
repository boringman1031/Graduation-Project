using System.Collections;
using UnityEngine;

/// <summary>
/// �a�ǩ�ʵe�ޯ�G�����a�O�y���ˮ`�A�u�ȴ��ɲ��ʳt�סA����q���ӭ���
/// </summary>
public class SkillCangYingPaiAnim : MonoBehaviour, ISkillEffect
{
    private Transform origin;          // �ޯ�I���
    private Animator anim;             // �ޯફ��W�� Animator

    [Header("�ޯ�Ѽ�")]
    public float energyCost = 20f;     // �I��ޯ���Ӫ���q
    public float damage = 50f;         // �ޯ�y�����ˮ`
    public float speedBuff = 10f;      // �ޯ���������a���t�ץ[��
    public float buffDuration = 3f;    // �t�ץ[������ɶ��]��^

    [Header("����")]
    public AudioDefination audioPlayer; // ���ļ���
    public AudioClip triggerSound;     // �ޯ�Ĳ�o����
    public AudioClip impactSound;      // �����a�O�ɼ��񪺭���

    [Header("�S��")]
    public GameObject impactEffectPrefab; // �����a�O�ɲ��ͪ��S��

    [Header("�ƥ�")]
    public CharacterEventSO powerChangeEvent; // ����q���s UI �Ψƥ�

    private bool isActivated = false;  // �ΨӧP�_�O�_���\�Ұʧޯ�]�������q�^

    /// <summary>
    /// �������a��q�A��Ĳ�o UI ��s�ƥ�
    /// </summary>
    void costPower(CharactorBase _Charater)
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }

    /// <summary>
    /// �ޯ�ʵe���񧹲��ɥ� Animation Event �I�s�A�����ޯ�
    /// </summary>
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// �ʵe�ƥ�G�����a�O�o�ͮɼ��񭵮ĻP�S��
    /// </summary>
    public void OnImpact()
    {
        if (!isActivated) return;

        // ����a�O��������
        if (audioPlayer != null && impactSound != null)
        {
            audioPlayer.audioClip = impactSound;
            audioPlayer.PlayAudioClip();
        }

        // ���ͦa�O�����S��
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // �ޯ൲���]�p�ݩ���Ψ�L�����A�]�i�אּ���� Destroy�^
        Destroy(gameObject);
    }

    /// <summary>
    /// �]�w�ޯ઺�I��ӷ��]���a�^�A���ˬd��q�O�_����
    /// </summary>
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar == null)
        {
            Debug.LogWarning("CharactorBase �����A�ޯ����");
            Destroy(gameObject);
            return;
        }

        // ��q�����A�ޯ����
        if (playerChar.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�I");
            Destroy(gameObject);
            return;
        }

        // ������q�P UI ��s
        costPower(playerChar);
        isActivated = true;
    }

    /// <summary>
    /// �]�w�ʵe����]�ثe�L�ݹ�@�A�i�d�š^
    /// </summary>
    public void SetPlayerAnimator(Animator animator)
    {
        // �p���ݭn�i�H�P�B�ʵe�Ѽ�
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // �Y�����\�Ұʧޯ�]�Ҧp��q�����^�A����ޯ�
        if (!isActivated || origin == null)
        {
            Debug.LogWarning("�ޯॼ�Ұʩ� origin �� null�A�P��");
            Destroy(gameObject);
            return;
        }

        // �]�����a�l����A���H��m
        transform.SetParent(origin);
        transform.localPosition = Vector3.zero;

        // ����ޯ�Ĳ�o����
        if (audioPlayer != null && triggerSound != null)
        {
            audioPlayer.audioClip = triggerSound;
            audioPlayer.PlayAudioClip();
        }

        // ���ɪ��a�t�סA�ñҰʭp�ɾ���_��t��
        PlayerController pc = origin.GetComponent<PlayerController>();
        if (pc != null)
        {
            float originalSpeed = pc.Speed;
            pc.Speed += speedBuff;
            StartCoroutine(RestoreSpeed(pc, originalSpeed, buffDuration));
        }
    }

    /// <summary>
    /// �b buff ����ɶ����_��t��
    /// </summary>
    private IEnumerator RestoreSpeed(PlayerController pc, float originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        pc.Speed = originalSpeed;
    }

    /// <summary>
    /// �ޯ�I����ĤH�ɳy���ˮ`�]�������a�^
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated) return;

        // ���B�z���a�ۤv
        if (collision.CompareTag("Player"))
            return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            float newHealth = target.CurrentHealth - damage;

            if (newHealth > 0)
            {
                target.CurrentHealth = newHealth;
                target.OnTakeDamage?.Invoke(transform);
            }
            else
            {
                target.CurrentHealth = 0;
                target.OnDead?.Invoke();
            }

            target.OnHealthChange?.Invoke(target);
        }
    }
}
