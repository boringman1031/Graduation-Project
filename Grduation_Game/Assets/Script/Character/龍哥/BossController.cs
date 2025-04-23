using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("第一招：球棒落地攻擊")]
    public Transform[] smashPositions;
    public GameObject warningPrefab;
    public GameObject attackPrefab;
    public AudioClip smashSFX;
    public Animator animator;

    [Header("特效")]
    public GameObject smashVFXPrefab;

    [Header("火焰連鎖爆炸")]
    public GameObject fireBlastPrefab;
    public Transform fireStartPoint;
    public int blastCount = 6;
    public float blastSpacing = 1.2f;
    public float blastInterval = 0.15f;
    public AudioClip fireBlastSFX;

    public float warningDuration = 0.5f;
    public float attackDelay = 0.3f;
    private int currentSmashIndex = 0;

    [Header("第三招：觸手追擊")]
    public GameObject tentaclePrefab;
    public Transform tentacleSpawnPoint;

    [Header("出招控制")]
    public float skillInterval = 3f;
    private float skillTimer;
    private int currentPattern = 0;

    [Header("戰鬥控制")]
    public bool canAct = false;

    [Header("結局對話與轉場")]
    public VoidEventSO bossFinalDialogEndEvent;
    public VoidEventSO dialogEndEvent;

    private bool waitingForDialog = false;
    private bool hasDied = false;

    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnded;
    }

    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnded;
    }

    public void OnBossDeath()
    {
        if (hasDied) return;
        hasDied = true;
        canAct = false;
        if (DialogManager.Instance != null)
        {
            waitingForDialog = true;
            DialogManager.Instance.StartDialog("FinalBoss_End");
        }
        else
        {
            bossFinalDialogEndEvent?.RaiseEvent();
        }
    }

    private void OnDialogEnded()
    {
        if (!waitingForDialog) return;

        bossFinalDialogEndEvent?.RaiseEvent();
        waitingForDialog = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        skillTimer = skillInterval;
    }

    private void Update()
    {
        if (!canAct) return;

        skillTimer -= Time.deltaTime;
        if (skillTimer <= 0f)
        {
            CastNextSkill();
            skillTimer = skillInterval;
        }
    }

    void CastNextSkill()
    {
        switch (currentPattern)
        {
            case 0:
                animator.SetTrigger("Smash1");
                break;
            case 1:
                animator.SetTrigger("Smash2");
                break;
            case 2:
                animator.SetTrigger("Smash3");
                break;
        }
        currentPattern = (currentPattern + 1) % 3;
    }

    public void StartTentacleAttack()
    {
        Debug.Log("\uD83D\uDC19 StartTentacleAttack 被動畫事件呼叫！");
        Instantiate(tentaclePrefab, tentacleSpawnPoint.position, Quaternion.identity);
    }

    public void OnSkillEnd()
    {
        animator.ResetTrigger("Smash1");
        animator.ResetTrigger("Smash2");
        animator.ResetTrigger("Smash3");
    }

    public void OnTentacleDestroyed()
    {
        CharactorBase boss = GetComponent<CharactorBase>();
        boss.TakeDamage(20f);
    }

    public void StartSmashAttack()
    {
        Debug.Log("\uD83D\uDD25 StartSmashAttack 被動畫事件呼叫！");
        StartCoroutine(DoSmashSequence());
    }

    private IEnumerator DoSmashSequence()
    {
        for (int i = 0; i < smashPositions.Length; i++)
        {
            currentSmashIndex = i;
            var warn = Instantiate(warningPrefab, smashPositions[i].position, Quaternion.identity);
            Destroy(warn, warningDuration);

            yield return new WaitForSeconds(warningDuration);

            if (smashVFXPrefab != null)
                Instantiate(smashVFXPrefab, smashPositions[i].position, Quaternion.identity);

            yield return new WaitForSeconds(attackDelay);

            if (smashSFX != null)
                AudioSource.PlayClipAtPoint(smashSFX, smashPositions[i].position);

            Instantiate(attackPrefab, smashPositions[i].position, Quaternion.identity);

            yield return new WaitForSeconds(attackDelay);
        }
    }

    public void StartFireBlastChain()
    {
        Debug.Log("第二招");
        StartCoroutine(FireBlastChainRoutine());
    }

    private IEnumerator FireBlastChainRoutine()
    {
        for (int i = 0; i < blastCount; i++)
        {
            Vector3 spawnPos = fireStartPoint.position + new Vector3(i * blastSpacing, 0f, 0f);

            Instantiate(fireBlastPrefab, spawnPos, Quaternion.identity);

            if (fireBlastSFX != null)
                AudioSource.PlayClipAtPoint(fireBlastSFX, spawnPos);

            yield return new WaitForSeconds(blastInterval);
        }
    }

    public void SpawnSmashAttack()
    {
        var pos = smashPositions[currentSmashIndex].position;
        AudioSource.PlayClipAtPoint(smashSFX, pos);
        Instantiate(attackPrefab, pos, Quaternion.identity);
    }
}
