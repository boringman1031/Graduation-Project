using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("第一招：球棒落地攻擊")]
    public Transform[] smashPositions; // 三個打擊點
    public GameObject warningPrefab;
    public GameObject attackPrefab;
    public AudioClip smashSFX;
    public Animator animator;
    [Header("特效")]
    public GameObject smashVFXPrefab;

    [Header("火焰連鎖爆炸")]
    public GameObject fireBlastPrefab; // 每個小爆炸
    public Transform fireStartPoint;   // 起始點（地面左側）
    public int blastCount = 6;         // 幾個火焰
    public float blastSpacing = 1.2f;  // 間距
    public float blastInterval = 0.15f; // 爆炸間隔時間
    public AudioClip fireBlastSFX;

    public float warningDuration = 0.5f;
    public float attackDelay = 0.3f;

    private int currentSmashIndex = 0;

    [Header("第三招：觸手追擊")]
    public GameObject tentaclePrefab;
    public Transform tentacleSpawnPoint;

    [Header("出招控制")]
    public float skillInterval = 3f; // 每幾秒出一次招
    private float skillTimer;
    private int currentPattern = 0; // 可做輪播 or 隨機

    [Header("戰鬥控制")]
    public bool canAct = false; // 預設 false，等對話完才設為 true

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
                animator.SetTrigger("Smash1"); // 第一招動畫（動畫中呼叫 StartSmashAttack
                break;
            case 1:
                animator.SetTrigger("Smash2"); // 第二招動畫（動畫事件呼叫 StartFireBlastChain）
                break;
            case 2:
                animator.SetTrigger("Smash3"); // 第三招動畫（動畫事件呼叫 StartTentacleAttack）
                break;
        }
        currentPattern = (currentPattern + 1) % 3;
       // currentPattern = Random.Range(0, 3); // 隨機輪播招式
    }
    public void StartTentacleAttack()
    {
        Debug.Log("🐙 StartTentacleAttack 被動畫事件呼叫！");
        Instantiate(tentaclePrefab, tentacleSpawnPoint.position, Quaternion.identity);
    }
    public void OnSkillEnd()
    {
        animator.ResetTrigger("Smash1");
        animator.ResetTrigger("Smash2");
        animator.ResetTrigger("Smash3");
    }

    // 被打掉時觸發
    public void OnTentacleDestroyed()
    {
        // 找到自己的 CharactorBase 扣血
        CharactorBase boss = GetComponent<CharactorBase>();
        boss.TakeDamage(20f); // 或你想要的扣血量
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // 按 T 測試第一招
        {
            animator.SetTrigger("Smash1");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("Smash2");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Smash3");
        }
    }*/
    public void StartSmashAttack()
    {
        Debug.Log("🔥 StartSmashAttack 被動畫事件呼叫！");
        StartCoroutine(DoSmashSequence());
    }

    private IEnumerator DoSmashSequence()
    {
        for (int i = 0; i < smashPositions.Length; i++)
        {
            currentSmashIndex = i;

            // 紅色預警
            var warn = Instantiate(warningPrefab, smashPositions[i].position, Quaternion.identity);
            Destroy(warn, warningDuration);

            yield return new WaitForSeconds(warningDuration);

            // 2. 播放地面特效（例：光波 or 煙塵）
            if (smashVFXPrefab != null)
                Instantiate(smashVFXPrefab, smashPositions[i].position, Quaternion.identity);

            yield return new WaitForSeconds(attackDelay); // 特效出來一瞬後攻擊

            // 3. 播放音效
            if (smashSFX != null)
                AudioSource.PlayClipAtPoint(smashSFX, smashPositions[i].position);

            // 4. 產生攻擊區域（帶 Attack.cs）
            Instantiate(attackPrefab, smashPositions[i].position, Quaternion.identity);

            yield return new WaitForSeconds(attackDelay); // 攻擊時間後續節奏

        }
    }

    // 第二招
    public void StartFireBlastChain()
    {
        Debug.Log("第二招");
        StartCoroutine(FireBlastChainRoutine());
    }

    // 負責連鎖產生火焰特效（往右爆炸）
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


    // 綁在動畫事件中的方法
    public void SpawnSmashAttack()
    {
        var pos = smashPositions[currentSmashIndex].position;
        AudioSource.PlayClipAtPoint(smashSFX, pos);
        Instantiate(attackPrefab, pos, Quaternion.identity);
    }

}
