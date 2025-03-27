using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [Header("UI組件")]
    public BossHealthUI bossHealthUI;

    [Header("廣播事件")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;
    public VoidEventSO tutorialBossSummonEvent;
    public VoidEventSO tutorialBossAttackEvent;

    [Header("事件監聽")]
    public VoidEventSO AttackBossEvent;
    public VoidEventSO dialogEndEvent; // 對話結束事件


    [Header("基礎數值")]
    public float maxHealth;
    public float currentHealth;

    [Header("狀態")]
    public bool isSummonMinion;   
    public float SuperArmourTime;//霸體時間
    private float SuperArmourTimeCounter;//霸體時間計數器
    public bool SuperArmour;//是否在霸體狀態
    public bool isTalk = false;

    private BossBaseState currentState;
    protected BossBaseState idleState;//閒置狀態
    protected BossBaseState attackState;//攻擊狀態
    protected BossBaseState summonState;//召喚狀態  
    protected BossBaseState summonHeartState;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        isSummonMinion = false;
        // 初始化血條
        if (bossHealthUI != null)
            bossHealthUI.UpdateHealth(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        currentState = idleState;
        currentState.OnEnter(this);
        AttackBossEvent.OnEventRaised += OnTakeDamage;
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }

    private void OnDisable()
    {
        currentState.OnExit();
        AttackBossEvent.OnEventRaised -= OnTakeDamage;
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    private void Update()
    {
        currentState.LogicUpdate();
        if (SuperArmour)
        {
            SuperArmourTimeCounter -= Time.deltaTime;
            if (SuperArmourTimeCounter <= 0)
            {
                SuperArmour = false;
            }
        }
    }
    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    //-----------Boss行為----------------
    public virtual void OnBossShow()//相機震動
    {          
        CameraShakeEvent.RaiseEvent();
        bossHealthUI.gameObject.SetActive(true); // 血條顯示
    }

    public void OnDialogEnd()//對話結束
    {
        if (isTalk)
        {
            SwitchState(BossState.Attack);
        }
    }
    public void OnTakeDamage()//Boss受到傷害
    {
        if(SuperArmour)
        {
            return;
        }  
        if(currentHealth > 0)
        {
            anim.SetTrigger("Hit");
            currentHealth -= 100;
            TriggerSuperArmour();
            // 更新血條
            if (bossHealthUI != null)
                bossHealthUI.UpdateHealth(currentHealth, maxHealth);
            Debug.Log("Boss受到傷害，當前血量：" + currentHealth);
        }     
        else
        {
            Die();
        }
    }
    private void TriggerSuperArmour()//觸發霸體
    {
        if (!SuperArmour)
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }
    public virtual void OnAttack()//Boss攻擊
    {
        anim.SetTrigger("Attack");
        tutorialBossAttackEvent.RaiseEvent();//廣播開啟攻擊事件教學
    }

    public virtual void OnSummon()//Boss召喚
    {
        anim.SetTrigger("Summon");
        tutorialBossSummonEvent.RaiseEvent();//廣播開啟召喚事件教學
    }

    public virtual void SpawnHeartMinion()//生成愛心小怪
    {
        anim.SetTrigger("Hit");          
    }
  
    public void Die()//Boss死亡
    {
        BossDeadEvent.RaiseEvent();
    }

    public void SwitchState(BossState _state)//切換狀態
    {
        var newState = _state switch//根據現有狀態切換敵人狀態(switch的語法糖寫法)
        {
            BossState.Idle => idleState,
            BossState.Attack => attackState,
            BossState.Summon => summonState,
            BossState.SummonHeart => summonHeartState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public bool CheckMinionsExist()
    {
        // 檢查場景內是否還有小怪
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");
        bool hasMinions = minions.Length > 0;     
        return hasMinions; // 如果有小怪則返回 `true`，否則返回 `false`
    }

    public bool CheckHeartMinionsExist()
    {
        // 檢查場景內是否還有愛心小怪
        GameObject[] heartMinions = GameObject.FindGameObjectsWithTag("Heart");
        bool hasHeartMinions = heartMinions.Length > 0;    
        return hasHeartMinions; // 如果有愛心小怪則返回 `true`，否則返回 `false`
    }
}
