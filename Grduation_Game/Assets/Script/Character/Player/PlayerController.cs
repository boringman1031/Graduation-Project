﻿/*------------------by 017-----------------------*/
using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;// 單例模式

    [Header("廣播事件")]
    public CameraShakeEventSO cameraShakeEvent; // 攝影機震動事件

    [Header("事件監聽")]
    public SceneLoadEventSO SceneloadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO disablePlayerEvent;
    public VoidEventSO enablePlayerEvent;

    public PlayerInput playerInput; // 輸入系統

    public Vector2 inputDirection; // 移動方向
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    [Header("玩家物理數值")]
    public PlayerStats playerStats;
    //public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce; // 被擊退力道

    [Header("特效")]
    public GameObject attackEffectPrefab;
    public GameObject DeadEffectPrefab;
    public GameObject HurtEffectPrefab;
    public Transform attackEffectPos;
    public GameObject comboTextPrefab; // 指向 ComboText 的預製體
    public Transform comboTextSpawnPoint; // Combo 文字生成的位置
    private int comboCount = 0; // 目前 Combo 數
    private float lastComboTime = 0f;
    public float comboResetTime = 2f; // 連擊重置時間（秒）

    [Header("玩家狀態")]
    public bool ishurt;
    public bool isDead;
    public bool isAttack;
    public bool canMove = true;

    public SkillCooldownUI[] skillCooldownUIs; // 技能冷卻 UI 陣列（QWER）
    public SkillData activeSkillData; // 當前動畫事件即將觸發的技能
    public Transform effectSpawnPoint; // 技能特效生成位置
    private float[] skillLastUsedTime; // 上次使用技能的時間，用來控制冷卻

    [Header("普攻攻擊區塊")]
    public GameObject[] attackObjects; // 對應 Attack1, Attack2, ...

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 取得組件
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        // 建立輸入對象
        playerInput = new PlayerInput();

        // 註冊輸入事件
        playerInput.GamePlay.Jump.started += Player_Jump;
        playerInput.GamePlay.Attack.started += Player_Attack;
        playerInput.GamePlay.SkillQ.started += ctx => ActivateSkill(0);
        playerInput.GamePlay.SkillW.started += ctx => ActivateSkill(1);
        playerInput.GamePlay.SkillE.started += ctx => ActivateSkill(2);
        playerInput.GamePlay.SkillR.started += ctx => ActivateSkill(3);

        // 初始化技能冷卻時間為可立即使用
        skillLastUsedTime = new float[4];
        for (int i = 0; i < skillLastUsedTime.Length; i++)
        {
            skillLastUsedTime[i] = -10f;
        }
    }

    private void OnEnable()
    {
        // 訂閱場景事件
        SceneloadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        disablePlayerEvent.OnEventRaised += OnDisablePlayer;// 禁用玩家控制
        enablePlayerEvent.OnEventRaised += OnEnablePlayer;// 啟用玩家控制
    }

    private void OnDisable()
    {
        playerInput.Disable();
        // 取消訂閱事件
        SceneloadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        disablePlayerEvent.OnEventRaised -= OnDisablePlayer;
        enablePlayerEvent.OnEventRaised -= OnEnablePlayer;
    }

    private void Update()
    {
        // 更新移動輸入值
        inputDirection = playerInput.GamePlay.Move.ReadValue<Vector2>();

        // Combo 倒數計時
        if (comboCount > 0 && Time.time - lastComboTime > comboResetTime)
        {
            comboCount = 0;
        }

        // 額外支援鍵盤快捷測試
        if (Input.GetKeyDown(KeyCode.Q)) UseSkillByIndex(0);
        if (Input.GetKeyDown(KeyCode.W)) UseSkillByIndex(1);
        if (Input.GetKeyDown(KeyCode.E)) UseSkillByIndex(2);
        if (Input.GetKeyDown(KeyCode.R)) UseSkillByIndex(3);
    }
    // ✅ 當職業被切換時，更新大招技能
    private void OnGUI()
    {
        //GUI.Label(new Rect(10, 10, 400, 20), $"Q: {SkillManager.Instance.equippedSkills[0]?.skillName}");
        //GUI.Label(new Rect(10, 30, 400, 20), $"W: {SkillManager.Instance.equippedSkills[1]?.skillName}");
        //GUI.Label(new Rect(10, 50, 400, 20), $"E: {SkillManager.Instance.equippedSkills[2]?.skillName}");
        //GUI.Label(new Rect(10, 70, 400, 20), $"R: {SkillManager.Instance.selectedClass?.ultimateSkill?.skillName}");
        //GUI.Label(new Rect(10, 100, 500, 20), $"ActiveSkillData: {activeSkillData?.skillName}");
    }

    public void UpdateUltimateSkill()
    {
        var ultimate = SkillManager.Instance.selectedClass?.ultimateSkill;

        if (ultimate == null)
        {
            Debug.Log("Ultimate skill 已更新: NULL");
        }
        else if (!ultimate.isUnlocked)
        {
            Debug.Log("Ultimate skill 已更新: 已存在但未解鎖");
        }
        else
        {
            Debug.Log("Ultimate skill 已更新: " + ultimate.skillName);
        }

        activeSkillData = ultimate;
    }
    private void FixedUpdate()
    {
        if (!canMove || ishurt || isAttack) return;
        Player_Move();
    }

    // 播放技能動畫（動畫觸發 OnSkillEffectTrigger）
    void UseSkillByIndex(int index)
    {
        SkillData skill = index == 3 ? SkillManager.Instance.selectedClass?.ultimateSkill : SkillManager.Instance.equippedSkills[index];
        if (skill == null) return;

        activeSkillData = skill;
        //animator.Play("Skill" + index);
    }

    // 嘗試發動技能：判斷冷卻與是否解鎖
    void ActivateSkill(int index)
    {
        SkillData skill = index == 3 ? SkillManager.Instance.selectedClass?.ultimateSkill : SkillManager.Instance.equippedSkills[index];

        if (skill != null && skill.isUnlocked)
        {
            if (Time.time - skillLastUsedTime[index] >= skill.cooldownTime)
            {
                skillCooldownUIs[index]?.StartCooldown(skill.cooldownTime);
                skillLastUsedTime[index] = Time.time;

                activeSkillData = skill;
                animator.SetTrigger(skill.skillName);
            }
            else
            {
                float remaining = skill.cooldownTime - (Time.time - skillLastUsedTime[index]);
                Debug.Log($"Skill {index} 冷卻中，還有 {remaining:F1} 秒");
            }
        }
        else
        {
            Debug.Log($"Skill {index} 未解鎖或未設定");
        }
    }

    // 技能動畫事件觸發：生成技能特效
    public void OnSkillEffectTrigger()
    {
        if (activeSkillData != null && activeSkillData.skillPrefab != null)
        {
            Transform parentTransform = activeSkillData.isFollowPlayer ? this.transform : null;
            GameObject skillInstance = Instantiate(activeSkillData.skillPrefab, transform.position, Quaternion.identity, parentTransform);

            ISkillEffect skillScript = skillInstance.GetComponent<ISkillEffect>();
            if (skillScript != null)
            {
                skillScript.SetOrigin(this.transform);
            }
        }
        else
        {
            Debug.LogWarning("OnSkillEffectTrigger: activeSkillData 或 skillPrefab 未設定");
        }
    }

    // 場景載入時禁止控制
    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)
    {
        playerInput.GamePlay.Disable();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
        canMove = true;
        playerInput.GamePlay.Enable(); // ← 把輸入打開
    }

    private void OnAfterSceneLoadEvent()
    {
        playerInput.GamePlay.Enable();
    }

    // 玩家移動邏輯
    public void Player_Move()
    {
        float currentSpeed = playerStats.speed;
        rb.velocity = new Vector2(inputDirection.x * currentSpeed, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = -1;
        else if (inputDirection.x < 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    // 速度增益（如技能加速）
    public void ApplySpeedBoost(float boost, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boost, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boost, float duration)
    {
        playerStats.speed += boost;
        yield return new WaitForSeconds(duration);
        playerStats.speed -= boost;
    }

    // 跳躍邏輯
    public void Player_Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jampforce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    // 攻擊事件
    public void Player_Attack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.OnPlayerAttack();
        isAttack = true;     
    }
    public void EnableAttack(int index)
    {
        if (index < 0 || index >= attackObjects.Length) return;

        GameObject atk = attackObjects[index];
        atk.SetActive(true);

        var atkScript = atk.GetComponent<Attack>();
        if (atkScript != null)
        {
            atkScript.Init(this.transform, atkScript.baseDamage, true);
        }
    }

    private void OnDisablePlayer()// 禁用玩家控制
    {
        canMove = false;
        playerInput.GamePlay.Disable();

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }
    }

    private void OnEnablePlayer()// 啟用玩家控制
    {
        canMove = true;
        playerInput.GamePlay.Enable();

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = true;
        }
    }
    public void DisableAttack(int index)
    {
        if (index < 0 || index >= attackObjects.Length) return;
        attackObjects[index].SetActive(false);
    }

    private void ShowComboText(int count)
    {
        if (comboTextPrefab == null || comboTextSpawnPoint == null) return;

        var go = Instantiate(comboTextPrefab, comboTextSpawnPoint.position, Quaternion.identity);
        var particleText = go.GetComponent<CFXR_ParticleText>();

        string text = count == 1 ? "Hit!" : $"Combo x{count}";
        Color color1 = Color.yellow;
        Color color2 = count >= 5 ? Color.red : Color.white;

        particleText.UpdateText(
            newText: text,
            newSize: 1f + count * 0.1f,
            newColor1: color1,
            newColor2: color2,
            newLifetimeMultiplier: 1f
        );

        Destroy(go, 2f); // 自動銷毀
    }

    // 攝影機震動
    public void CameraShake()
    {
        cameraShakeEvent.OnEventRaised(0.5f, 1f, 0.15f);
    }
  
    // 特效生成方法
    public void Player_HurtEffect() => Instantiate(HurtEffectPrefab, transform.position, Quaternion.identity);
    public void Player_AttackEffect() 
    { 
        Instantiate(attackEffectPrefab, attackEffectPos.position, Quaternion.identity);
        comboCount++;
        lastComboTime = Time.time;
        ShowComboText(comboCount);
    }
    public void Player_DeadEffect() => Instantiate(DeadEffectPrefab, transform.position, Quaternion.identity);

    #region UnityEvents
    // ✅ 解鎖技能後重設所有技能冷卻時間與 UI
    public void ResetSkillCooldowns()
    {
        for (int i = 0; i < skillLastUsedTime.Length; i++)
        {
            skillLastUsedTime[i] = -10f; // 表示冷卻已結束
            skillCooldownUIs[i]?.ResetCooldown(); // 重置 UI 冷卻圈圈
        }
    }


    // 玩家受傷：擊退並標記受傷狀態
    public void Player_GetHurt(Transform _attacker)
    {
        ishurt = true;
        rb.velocity = Vector2.zero;
        Vector2 die = new Vector2((transform.position.x - _attacker.position.x), 0).normalized;
        rb.AddForce(die * Hurtforce, ForceMode2D.Impulse);
    }

    // 玩家死亡：禁用輸入
    public void Player_Dead()
    {
        isDead = true;

        var sceneLoader = FindObjectOfType<SceneLoader>();
        sceneLoader.challengeCount = 0;
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(sceneLoader.challengeCount);

        StartCoroutine(ReviveAtHome());
    }
    private IEnumerator ReviveAtHome()
    {
        yield return new WaitForSeconds(1f); // 讓死亡動畫等一下

        isDead = false;     // ✅ 重設死亡狀態
        ishurt = false;
        isAttack = false;

        playerInput.GamePlay.Enable(); // ✅ 開啟控制
        enablePlayerEvent?.RaiseEvent(); // ✅ 廣播啟用事件
    }

    #endregion
}