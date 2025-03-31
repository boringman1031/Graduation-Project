/*------------------by 017-----------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    [Header("廣播事件")]
    public CameraShakeEventSO cameraShakeEvent;//攝影機震動事件

    [Header("事件監聽")]
    public SceneLoadEventSO SceneloadEvent;//場景加載事件
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayerInput playerInput;

    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    [Header("玩家物理數值")]
    public PlayerStats playerStats;
    public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce;//玩家受到傷害擊退力

    [Header("特效")]
    public GameObject attackEffectPrefab;//攻擊特效
    public GameObject DeadEffectPrefab;//死亡特效
    public GameObject HurtEffectPrefab;//受傷特效
    public Transform attackEffectPos;//攻擊特效生成位置


    [Header("玩家狀態")]
    public bool ishurt;//是否受傷
    public bool isDead;//是否死亡
    public bool isAttack;//是否攻擊

    // 技能資料陣列，依序對應 Q, W, E, R
    private SkillData[] currentSkills = new SkillData[4];
    private SkillData activeSkillData;     // 目前正在使用的技能資料
    public Transform effectSpawnPoint;     // 指定特效生成點（例如玩家手部或前方空物件）
    private float[] skillLastUsedTime;  // 儲存每個技能上次使用的時間
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        playerInput = new PlayerInput();
        //跳躍事件
        playerInput.GamePlay.Jump.started += Player_Jump;
        //攻擊事件
        playerInput.GamePlay.Attack.started += Player_Attack;

        // 初始化技能資料，假設 SkillManager 已正確設定技能
        currentSkills[0] = SkillManager.Instance.equippedSkills[0];
        currentSkills[1] = SkillManager.Instance.equippedSkills[1];
        currentSkills[2] = SkillManager.Instance.equippedSkills[2];
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;

        // 初始化冷卻追蹤陣列，長度與技能數量相同
        skillLastUsedTime = new float[currentSkills.Length];
        for (int i = 0; i < skillLastUsedTime.Length; i++)
        {
            // 如果該技能槽位為 null，可以給予 0 或其他預設值
            if (currentSkills[i] != null)
                skillLastUsedTime[i] = -currentSkills[i].cooldownTime;
            else
                skillLastUsedTime[i] = 0; // 或視需求處理
        }

        // 訂閱技能按鍵事件，這裡使用 started 事件（也可以使用 performed，視需求而定）
        playerInput.GamePlay.SkillQ.started += OnSkillQ;
        playerInput.GamePlay.SkillW.started += OnSkillW;
        playerInput.GamePlay.SkillE.started += OnSkillE;
        playerInput.GamePlay.SkillR.started += OnSkillR;
    }
    public void UpdateUltimateSkill()
    {
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;
    }
    void OnSkillQ(InputAction.CallbackContext context)
    {
        Debug.Log("Skill Q Pressed");
        ActivateSkill(0);
    }

    void OnSkillW(InputAction.CallbackContext context)
    {
        Debug.Log("Skill W Pressed");
        ActivateSkill(1);
    }

    void OnSkillE(InputAction.CallbackContext context)
    {
        Debug.Log("Skill E Pressed");
        ActivateSkill(2);
    }

    void OnSkillR(InputAction.CallbackContext context)
    {
        Debug.Log("Skill R Pressed");
        ActivateSkill(3);
    }
    // 激活技能的方法：根據索引從 currentSkills 陣列中取得技能資料，若技能已解鎖則在玩家位置生成技能預製物
    void ActivateSkill(int index)
    {
        SkillData skill = currentSkills[index];
        if (skill != null && skill.isUnlocked)
        {
            // 檢查冷卻：如果現在時間 - 上次使用時間 >= 冷卻時間，則可以使用技能
            if (Time.time - skillLastUsedTime[index] >= skill.cooldownTime)
            {
                // 記錄這次施放的時間
                skillLastUsedTime[index] = Time.time;

                // 記錄目前技能資料，供動畫事件使用（例如 activeSkillData）
                activeSkillData = skill;
                // 觸發角色技能動畫，這裡用技能名稱當 Trigger（你也可以改成其他參數）
                if (!string.IsNullOrEmpty(skill.skillName))
                    animator.SetTrigger(skill.skillName);
            }
            else
            {
                float remaining = skill.cooldownTime - (Time.time - skillLastUsedTime[index]);
                Debug.Log("Skill " + index + " 冷卻中，還有 " + remaining.ToString("F1") + " 秒");
            }
        }
        else
        {
            Debug.Log("Skill " + index + " 未解鎖或未設定");
        }
    }
    // 此方法會由動畫事件呼叫
    public void OnSkillEffectTrigger()
    {
        if (activeSkillData != null && activeSkillData.skillPrefab != null)
        {
            // 根據 activeSkillData 裡面是否需要跟隨玩家來決定是否指定父物件
            Transform parentTransform = activeSkillData.isFollowPlayer ? this.transform : null;
            GameObject skillInstance = Instantiate(activeSkillData.skillPrefab, transform.position, Quaternion.identity, parentTransform);

            ISkillEffect skillScript = skillInstance.GetComponent<ISkillEffect>();
            if (skillScript != null)
            {
                // 這裡傳入玩家 transform 可用來初始位置調整，但生成後是否跟隨取決於是否有指定父物件
                skillScript.SetOrigin(this.transform);
            }
        }
        else
        {
            Debug.LogWarning("OnSkillEffectTrigger: activeSkillData 或 skillPrefab 未設定");
        }
    }
    private void OnEnable()
    {      
        SceneloadEvent.LoadRequestEvent += OnLoadEvent;//場景加載時停止玩家控制
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;//場景加載完成開起玩家控制
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//讀取遊戲進度事件
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;//返回主選單事件
    }

    private void OnDisable()
    {
        // 取消基本動作與技能事件訂閱
        //playerInput.GamePlay.Jump.started -= Player_Jump;
        //playerInput.GamePlay.Attack.started -= Player_Attack;
        //playerInput.GamePlay.SkillQ.started -= OnSkillQ;
        //playerInput.GamePlay.SkillW.started -= OnSkillW;
        //playerInput.GamePlay.SkillE.started -= OnSkillE;
        //playerInput.GamePlay.SkillR.started -= OnSkillR;

        playerInput.Disable();
        SceneloadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void Update()
    {
        inputDirection = playerInput.GamePlay.Move.ReadValue<Vector2>();
    } 

    private void FixedUpdate()
    {
        if (!ishurt && !isAttack)
            Player_Move();
    }

    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)//場景加載時停止玩家控制
    {
        playerInput.GamePlay.Disable();          
    }
    private void OnLoadDataEvent()//讀取遊戲進度事件
    {
        isDead = false;
    }

    private void OnAfterSceneLoadEvent()//場景加載完成開起玩家控制
    {
        playerInput.GamePlay.Enable();     
    }
    public void Player_Move()
    {   
        float currentSpeed = playerStats != null ? playerStats.speed : Speed;  // 若沒有PlayerStats, 使用預設Speed
        
        rb.velocity = new Vector2(inputDirection.x * currentSpeed, rb.velocity.y);

        //rb.velocity = new Vector2(inputDirection.x * Speed * Time.deltaTime , rb.velocity.y);

        //人物翻轉
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = -1;
        else if (inputDirection.x < 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    // 增加玩家速度
    public void ApplySpeedBoost(float boost, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boost, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boost, float duration)
    {
        // 增加玩家速度
        playerStats.speed += boost;
        yield return new WaitForSeconds(duration);
        // 恢復原本速度
        playerStats.speed -= boost;
    }

    public void Player_Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jampforce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    public void Player_Attack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.OnPlayerAttack();    
        isAttack = true;    
    }

    public void CameraShake()//攝影機震動
    {
        cameraShakeEvent.OnEventRaised(0.5f, 1f, 0.15f);
    }

    public void Player_HurtEffect()//玩家受傷
    {
        Instantiate(HurtEffectPrefab, transform.position, Quaternion.identity);
    }

    public void Player_AttackEffect()//攻擊特效生成
    {
        Instantiate(attackEffectPrefab, attackEffectPos.position, Quaternion.identity);
    }

    public void Player_DeadEffect()//死亡特效生成
    {
        Instantiate(DeadEffectPrefab, transform.position, Quaternion.identity);
    }
    #region  以下為在UnityEvent中執行部分
    public void Player_GetHurt(Transform _attacker)//受傷擊飛
    {
        ishurt = true;
        rb.velocity = Vector2.zero;
        Vector2 die = new Vector2((transform.position.x - _attacker.position.x), 0).normalized;

        rb.AddForce(die * Hurtforce, ForceMode2D.Impulse);      
    }

    public void Player_Dead()
    {
        isDead = true;
        playerInput.GamePlay.Disable();
    }
    #endregion
}
