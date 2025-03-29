using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillController : MonoBehaviour
{
    // 技能資料陣列，依序對應 Q, W, E, R
    private SkillData[] currentSkills = new SkillData[4];

    // 共用的 PlayerInput 實例，從 PlayerController 中取得
    private PlayerInput playerInput;

    private void Awake()
    {
        // 透過 GetComponent 取得同一 GameObject 上的 PlayerController
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            //playerInput = pc.MyPlayerInput;
        }
        else
        {
            Debug.LogError("PlayerController not found on the same GameObject!");
        }
    }

    private void Start()
    {
        // 初始化技能資料，假設 SkillManager 已正確設定技能
        currentSkills[0] = SkillManager.Instance.equippedSkills[0];
        currentSkills[1] = SkillManager.Instance.equippedSkills[1];
        currentSkills[2] = SkillManager.Instance.equippedSkills[2];
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;

        // 訂閱技能按鍵事件，這裡使用 started 事件（也可以使用 performed，視需求而定）
        playerInput.GamePlay.SkillQ.started += OnSkillQ;
        playerInput.GamePlay.SkillW.started += OnSkillW;
        playerInput.GamePlay.SkillE.started += OnSkillE;
        playerInput.GamePlay.SkillR.started += OnSkillR;

        Debug.Log("PlayerSkillController 初始化完成，使用共用的 PlayerInput");
    }

    private void OnDisable()
    {
        // 取消事件訂閱，避免重複觸發或記憶體洩漏
        if (playerInput != null)
        {
            playerInput.GamePlay.SkillQ.started -= OnSkillQ;
            playerInput.GamePlay.SkillW.started -= OnSkillW;
            playerInput.GamePlay.SkillE.started -= OnSkillE;
            playerInput.GamePlay.SkillR.started -= OnSkillR;
        }
    }
    public void UpdateUltimateSkill()
    {
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;
        Debug.Log("Ultimate skill updated.");
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
            Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Skill " + index + " 未解鎖或未設定");
        }
    }
}
