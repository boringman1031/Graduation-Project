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
    [Header("�s���ƥ�")]
    public CameraShakeEventSO cameraShakeEvent;//��v���_�ʨƥ�

    [Header("�ƥ��ť")]
    public SceneLoadEventSO SceneloadEvent;//�����[���ƥ�
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayerInput playerInput;

    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    [Header("���a���z�ƭ�")]
    public PlayerStats playerStats;
    public float Speed = 10f;
    public float jampforce = 16.5f;
    public float Hurtforce;//���a����ˮ`���h�O

    [Header("�S��")]
    public GameObject attackEffectPrefab;//�����S��
    public GameObject DeadEffectPrefab;//���`�S��
    public GameObject HurtEffectPrefab;//���˯S��
    public Transform attackEffectPos;//�����S�ĥͦ���m


    [Header("���a���A")]
    public bool ishurt;//�O�_����
    public bool isDead;//�O�_���`
    public bool isAttack;//�O�_����

    // �ޯ��ư}�C�A�̧ǹ��� Q, W, E, R
    private SkillData[] currentSkills = new SkillData[4];
    private SkillData activeSkillData;     // �ثe���b�ϥΪ��ޯ���
    public Transform effectSpawnPoint;     // ���w�S�ĥͦ��I�]�Ҧp���a�ⳡ�Ϋe��Ū���^
    private float[] skillLastUsedTime;  // �x�s�C�ӧޯ�W���ϥΪ��ɶ�
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        playerInput = new PlayerInput();
        //���D�ƥ�
        playerInput.GamePlay.Jump.started += Player_Jump;
        //�����ƥ�
        playerInput.GamePlay.Attack.started += Player_Attack;

        // ��l�Ƨޯ��ơA���] SkillManager �w���T�]�w�ޯ�
        currentSkills[0] = SkillManager.Instance.equippedSkills[0];
        currentSkills[1] = SkillManager.Instance.equippedSkills[1];
        currentSkills[2] = SkillManager.Instance.equippedSkills[2];
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;

        // ��l�ƧN�o�l�ܰ}�C�A���׻P�ޯ�ƶq�ۦP
        skillLastUsedTime = new float[currentSkills.Length];
        for (int i = 0; i < skillLastUsedTime.Length; i++)
        {
            // �p�G�ӧޯ�Ѧ쬰 null�A�i�H���� 0 �Ψ�L�w�]��
            if (currentSkills[i] != null)
                skillLastUsedTime[i] = -currentSkills[i].cooldownTime;
            else
                skillLastUsedTime[i] = 0; // �ε��ݨD�B�z
        }

        // �q�\�ޯ����ƥ�A�o�̨ϥ� started �ƥ�]�]�i�H�ϥ� performed�A���ݨD�өw�^
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
    // �E���ޯ઺��k�G�ھگ��ޱq currentSkills �}�C�����o�ޯ��ơA�Y�ޯ�w����h�b���a��m�ͦ��ޯ�w�s��
    void ActivateSkill(int index)
    {
        SkillData skill = currentSkills[index];
        if (skill != null && skill.isUnlocked)
        {
            // �ˬd�N�o�G�p�G�{�b�ɶ� - �W���ϥήɶ� >= �N�o�ɶ��A�h�i�H�ϥΧޯ�
            if (Time.time - skillLastUsedTime[index] >= skill.cooldownTime)
            {
                // �O���o���I�񪺮ɶ�
                skillLastUsedTime[index] = Time.time;

                // �O���ثe�ޯ��ơA�Ѱʵe�ƥ�ϥΡ]�Ҧp activeSkillData�^
                activeSkillData = skill;
                // Ĳ�o����ޯ�ʵe�A�o�̥Χޯ�W�ٷ� Trigger�]�A�]�i�H�令��L�Ѽơ^
                if (!string.IsNullOrEmpty(skill.skillName))
                    animator.SetTrigger(skill.skillName);
            }
            else
            {
                float remaining = skill.cooldownTime - (Time.time - skillLastUsedTime[index]);
                Debug.Log("Skill " + index + " �N�o���A�٦� " + remaining.ToString("F1") + " ��");
            }
        }
        else
        {
            Debug.Log("Skill " + index + " ������Υ��]�w");
        }
    }
    // ����k�|�Ѱʵe�ƥ�I�s
    public void OnSkillEffectTrigger()
    {
        if (activeSkillData != null && activeSkillData.skillPrefab != null)
        {
            // �ھ� activeSkillData �̭��O�_�ݭn���H���a�ӨM�w�O�_���w������
            Transform parentTransform = activeSkillData.isFollowPlayer ? this.transform : null;
            GameObject skillInstance = Instantiate(activeSkillData.skillPrefab, transform.position, Quaternion.identity, parentTransform);

            ISkillEffect skillScript = skillInstance.GetComponent<ISkillEffect>();
            if (skillScript != null)
            {
                // �o�̶ǤJ���a transform �i�ΨӪ�l��m�վ�A���ͦ���O�_���H���M��O�_�����w������
                skillScript.SetOrigin(this.transform);
            }
        }
        else
        {
            Debug.LogWarning("OnSkillEffectTrigger: activeSkillData �� skillPrefab ���]�w");
        }
    }
    private void OnEnable()
    {      
        SceneloadEvent.LoadRequestEvent += OnLoadEvent;//�����[���ɰ���a����
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;//�����[�������}�_���a����
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//Ū���C���i�רƥ�
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;//��^�D���ƥ�
    }

    private void OnDisable()
    {
        // �����򥻰ʧ@�P�ޯ�ƥ�q�\
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

    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)//�����[���ɰ���a����
    {
        playerInput.GamePlay.Disable();          
    }
    private void OnLoadDataEvent()//Ū���C���i�רƥ�
    {
        isDead = false;
    }

    private void OnAfterSceneLoadEvent()//�����[�������}�_���a����
    {
        playerInput.GamePlay.Enable();     
    }
    public void Player_Move()
    {   
        float currentSpeed = playerStats != null ? playerStats.speed : Speed;  // �Y�S��PlayerStats, �ϥιw�]Speed
        
        rb.velocity = new Vector2(inputDirection.x * currentSpeed, rb.velocity.y);

        //rb.velocity = new Vector2(inputDirection.x * Speed * Time.deltaTime , rb.velocity.y);

        //�H��½��
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = -1;
        else if (inputDirection.x < 0)
            faceDir = 1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    // �W�[���a�t��
    public void ApplySpeedBoost(float boost, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boost, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boost, float duration)
    {
        // �W�[���a�t��
        playerStats.speed += boost;
        yield return new WaitForSeconds(duration);
        // ��_�쥻�t��
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

    public void CameraShake()//��v���_��
    {
        cameraShakeEvent.OnEventRaised(0.5f, 1f, 0.15f);
    }

    public void Player_HurtEffect()//���a����
    {
        Instantiate(HurtEffectPrefab, transform.position, Quaternion.identity);
    }

    public void Player_AttackEffect()//�����S�ĥͦ�
    {
        Instantiate(attackEffectPrefab, attackEffectPos.position, Quaternion.identity);
    }

    public void Player_DeadEffect()//���`�S�ĥͦ�
    {
        Instantiate(DeadEffectPrefab, transform.position, Quaternion.identity);
    }
    #region  �H�U���bUnityEvent�����泡��
    public void Player_GetHurt(Transform _attacker)//��������
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
