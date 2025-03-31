using System.Collections;
using UnityEngine;

public class SkillCangYingPaiAnim : MonoBehaviour, ISkillEffect
{
    private Transform origin;
    private Animator anim;

    [Header("�ޯ�Ѽ�")]
    public float energyCost = 20f;
    public float damage = 50f;
    public float speedBuff = 10f;
    public float buffDuration = 3f;

    [Header("����")]
    public AudioClip triggerSound;  // Ĳ�o�ޯ�ɼ���
    public AudioClip impactSound;   // �����a�O�ɼ���

    [Header("�S��")]
    public GameObject impactEffectPrefab; // �����a�O�ɲ��ͪ��S��

    // ��ޯ�ʵe�����ɡA�z�L Animation Event �I�s�����
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    // ��ʵe���������a�O������V�ɡA�ǥ� Animation Event �I�s����k
    public void OnImpact()
    {
        // ����������
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }
        // �ͦ������S��
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
        // �i�ھڻݭn�A�Y�P�ɤ]�n�y���ˮ`�A�i�H�b���B�I�s OnTriggerEnter2D �޿�Ψ�L��k
        Destroy(gameObject);
    }

    // ISkillEffect ��@�G�]�w�ޯ�o�X��
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // ������q�]���]���a����q�޲z�b CharactorBase ���^
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            playerChar.CurrentPower -= energyCost;
            if (playerChar.CurrentPower < 0)
                playerChar.CurrentPower = 0;
            playerChar.OnHealthChange?.Invoke(playerChar);
        }
    }

    // ISkillEffect ��@�G�]�w���a�ʵe�]�p���ݭn�^
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
        if (origin == null)
        {
            Debug.LogError("SkillCangYingPaiAnim: origin is not set!");
            Destroy(gameObject);
            return;
        }

        // ���ޯ� prefab �������a���l����A�o�˥��|���H���a����
        transform.SetParent(origin);
        // �p�G�A���ʵe�O�H���a�y�м���A�h���m���m���s�]�Ψ̻ݨD�վ�^
        transform.localPosition = Vector3.zero;

        // ����ޯ�Ĳ�o�ɭ���
        if (triggerSound != null)
        {
            AudioSource.PlayClipAtPoint(triggerSound, transform.position);
        }

        // �����a�u�ȼW�[�t��
        PlayerController pc = origin.GetComponent<PlayerController>();
        if (pc != null)
        {
            float originalSpeed = pc.Speed;
            pc.Speed += speedBuff;
            StartCoroutine(RestoreSpeed(pc, originalSpeed, buffDuration));
        }
    }

    private IEnumerator RestoreSpeed(PlayerController pc, float originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        pc.Speed = originalSpeed;
    }

    // ��ޯફ��I����ĤH�ɰ���ˮ`�P�w
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ư����a�]���]���a�� Tag �� "Player"�^
        if (collision.CompareTag("Player"))
            return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            // �����ˮ`�޿�
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
