using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillController : MonoBehaviour
{
    private SkillData[] currentSkills = new SkillData[4]; // Q,W,E,R

    // Input System 绑定
    public InputActionReference skillQ, skillW, skillE, skillR;
    void Start()
    {
        // 初始化技能
        currentSkills[0] = SkillManager.Instance.equippedSkills[0];
        currentSkills[1] = SkillManager.Instance.equippedSkills[1];
        currentSkills[2] = SkillManager.Instance.equippedSkills[2];
        // 初始化 R 技能
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill; // 直接綁定職業大招

        // 绑定输入事件
        skillQ.action.performed += OnSkillQ;
        skillW.action.performed += OnSkillW;
        skillE.action.performed += OnSkillE;
        skillR.action.performed += OnSkillR;
    }
    private void OnEnable()
    {
        // 監聽職業切換事件（需在 SkillManager 中實現事件）
        // SkillManager.Instance.OnClassChanged += UpdateUltimateSkill;
    }
    private void OnDisable()
    {
        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.OnClassChanged -= UpdateUltimateSkill;
        }
        skillQ.action.performed -= OnSkillQ;
        skillW.action.performed -= OnSkillW;
        skillE.action.performed -= OnSkillE;
        skillR.action.performed -= OnSkillR;
    }
    public void UpdateUltimateSkill()
    {
        currentSkills[3] = SkillManager.Instance.selectedClass?.ultimateSkill;
    }
    void OnSkillQ(InputAction.CallbackContext context)
    {
        ActivateSkill(0);
    }
    void OnSkillW(InputAction.CallbackContext context)
    {
        ActivateSkill(1);
    }
    void OnSkillE(InputAction.CallbackContext context)
    {
        ActivateSkill(2);
    }
    void OnSkillR(InputAction.CallbackContext context)
    {
        ActivateSkill(3);
    }
    void ActivateSkill(int index)
    {
        SkillData skill = currentSkills[index];
        if (skill != null && skill.isUnlocked)
        {
            Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
        }
    }
}
